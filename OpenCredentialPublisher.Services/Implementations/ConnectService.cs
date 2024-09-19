using System;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenCredentialPublisher.Data.Abstracts;
using OpenCredentialPublisher.Data.Custom.Commands;
using OpenCredentialPublisher.Data.Custom.Contexts;
using OpenCredentialPublisher.Data.Custom.EFModels;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Settings;

namespace OpenCredentialPublisher.Services.Implementations
{
    public class ConnectService
    {
        private const string JsonMediaType = "application/json";
        private const string AuthenticationScheme = "BEARER";
        private readonly WalletDbContext _context;
        private readonly HostSettings _hostSettings;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ConnectService> _logger;
        private readonly LogHttpClientService _logHttpClientService;
        private readonly ETLService _etlService;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly NotificationService _notificationService;

        public ConnectService(WalletDbContext context,
            HostSettings hostSettings,
            IHttpClientFactory httpClientFactory,
            ETLService etlService,
            LogHttpClientService logHttpClientService,
            ILogger<ConnectService> logger,
            UserManager<ApplicationUser> userManager,
            NotificationService notificationService)
        {
            _context = context;
            _hostSettings = hostSettings;
            _httpClientFactory = httpClientFactory;

            _etlService = etlService;
            _logHttpClientService = logHttpClientService;
            _logger = logger;

            _userManager = userManager;
            _notificationService = notificationService;
        }

        public async Task<CredentialResponse> ConnectExternalAsync(ControllerBase controller,
            ConnectRequestCommand command)
        {
            var source = await GetSourceAsync(command);

            var authorization = new Authorization
            {
                Payload = command.Payload,
                Method = command.Method,
                Endpoint = command.Endpoint,
                SourceId = source.Id
            };

            var clrJson = await GetContentStringAsync(source, authorization);
            var clrResult = await _etlService.GetClrCredentialModelAsync(clrJson, controller.HttpContext.Request);

            if (clrResult.credentialResponse.HasError)
                return clrResult.credentialResponse;

            var email = _etlService.GetEmail(clrResult.clrCredentialModel);

            if (email == null)
                throw new ApplicationException("No email found in the payload.  Email is required.");

            var connectUser = await GetOrCreateUserAsync(email, command.Payload);

            authorization.User = connectUser;
            _context.Authorizations.Add(authorization);

            await _context.SaveChangesAsync();

            // Create Notification
            var credentialResponse =
                await _notificationService.AddNotificationAsync(connectUser.Id, clrJson,
                    controller.HttpContext.Request);



            // NOTE: Do not send an email here anymore since awarding service is sending an email

            return credentialResponse;
        }

        public async Task<ApplicationUser> GetOrCreateUserAsync(string email, string payload)
        {


            var connectUser = await _context.Users.FirstOrDefaultAsync(u => u.NormalizedEmail == email.ToUpperInvariant());
            if (connectUser != null)
            {
                return connectUser;
            }

            var strategy = new SqlServerRetryingExecutionStrategy(_context,
                maxRetryCount: 15,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: new[]
                {
                    2601 // unique constraint 
                });

            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction =
                    await _context.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);
                try
                {
                    connectUser = new ApplicationUser
                    {
                        UserName = email,
                        Email = email,
                        DisplayName = email
                    };

                    var identityResult = await _userManager.CreateAsync(connectUser);
                    if (!identityResult.Succeeded)
                    {
                        if (identityResult.Errors.Any() &&
                            identityResult.Errors.FirstOrDefault(x => x.Code == "DuplicateUserName") != null)
                        {
                            connectUser = await _userManager.FindByEmailAsync(email);
                        }
                        else
                        {
                            throw new ApplicationException($"An account for email {email} could not be created.");
                        }
                    }

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogInformation(
                        "[{ThreadId}] GetOrCreateUserAsync strategy exec catch -- Email: {Email}, Payload: {Payload}",
                        Thread.CurrentThread.ManagedThreadId,
                        email,
                        payload ?? "null");
                    throw;
                }
            });

            return connectUser;
        }

        //private void LogChangeTrackerEntries(string label)
        //{
        //    _context.ChangeTracker.DetectChanges();

        //    foreach (var entry in _context.ChangeTracker.Entries())
        //    {
        //        var entityName = entry.Entity.GetType().Name;
        //        var state = entry.State;

        //        var originalValues = entry.OriginalValues.Properties
        //            .ToDictionary(p => p.Name, p => entry.OriginalValues[p]);
        //        var currentValues = entry.CurrentValues.Properties
        //            .ToDictionary(p => p.Name, p => entry.CurrentValues[p]);

        //        var originalValuesString = string.Join(", ", originalValues.Select(kv => $"{kv.Key}: {kv.Value}"));
        //        var currentValuesString = string.Join(", ", currentValues.Select(kv => $"{kv.Key}: {kv.Value}"));

        //        _logger.LogInformation($"[{Thread.CurrentThread.ManagedThreadId}] {label} -- Entity: {entityName}, State: {state}, OriginalValues: {originalValuesString}, CurrentValues: {currentValuesString}");
        //    }

        //}

        private async Task<Source> GetSourceAsync(ConnectRequestCommand command)
        {
            var source = await _context.Sources.AsNoTracking()
                .FirstOrDefaultAsync(s => s.Url == command.Issuer && s.Scope == command.Scope);

            if (source == null)
            {
                var discoveryDocument = await GetDiscoveryDocumentAsync(command.Issuer);
                if (string.IsNullOrEmpty(discoveryDocument.RegistrationEndpoint))
                {
                    throw new Exception($"This {command.Issuer} api does not allow for dynamic registration");
                }

                var clientRegistration = await RegisterAsync(_hostSettings.ClientName, _hostSettings.DnsName, command.Scope, discoveryDocument);
                source = new Source()
                {
                    ClientId = clientRegistration.ClientId,
                    ClientSecret = clientRegistration.ClientSecret,
                    Scope = command.Scope,
                    Name = discoveryDocument.Issuer,
                    Url = command.Issuer
                };

                await _context.Sources.AddAsync(source);
                await _context.SaveChangesAsync();
            }

            return source;
        }

        private async Task<DynamicClientRegistrationResponse> RegisterAsync(string clientName,
            string clientUri,
            string scope,
            DiscoveryDocumentResponse discoveryDocument)
        {
            using var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");


            var jsonData = new
            {
                client_name = clientName,
                client_uri = clientUri,
                scope,
                token_endpoint_auth_method = OidcConstants.EndpointAuthenticationMethods.BasicAuthentication
            };

            var json = JsonConvert.SerializeObject(jsonData);

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            try
            {
                var response = await client.PostAsync(discoveryDocument.RegistrationEndpoint, content);
                var registrationResponse = await ProtocolResponse.FromHttpResponseAsync<DynamicClientRegistrationResponse>(response);
                if (!registrationResponse.IsError)
                {
                    return registrationResponse;
                }
                throw new Exception(registrationResponse.Error, registrationResponse.Exception);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                return ProtocolResponse.FromException<DynamicClientRegistrationResponse>(ex);
            }
        }

        private async Task<DiscoveryDocumentResponse> GetDiscoveryDocumentAsync(string baseUrl)
        {
            using var client = _httpClientFactory.CreateClient();
            var discoveryDocument = await client.GetDiscoveryDocumentAsync(baseUrl);
            return discoveryDocument;
        }


        private async Task<string> GetContentStringAsync(Source source, Authorization authorization,
            DiscoveryDocumentResponse discoveryDocument = null)
        {
            discoveryDocument ??= await GetDiscoveryDocumentAsync(source.Url);
            using var client = _httpClientFactory.CreateClient();

            var token = await client.RequestTokenAsync(new ScopedTokenRequest
            {
                Address = discoveryDocument.TokenEndpoint,
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                Scope = source.Scope,
                GrantType = OidcConstants.GrantTypes.ClientCredentials
            });


            var endpoint = discoveryDocument.TryGet(authorization.Endpoint);
            if (!string.IsNullOrEmpty(endpoint))
            {
                HttpRequestMessage request = authorization.Method.ToUpper() switch
                {
                    "POST" => BuildPostRequest(endpoint, token, authorization),
                    "GET" => BuildGetRequest(endpoint, token, authorization),
                    _ => throw new NotImplementedException()
                };

                using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                await _logHttpClientService.LogAsync(response);

                response.EnsureSuccessStatusCode();

                if (response.Content is object)
                {
                    if (response.Content.Headers.ContentType.MediaType == JsonMediaType)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                    else if (response.Content.Headers.ContentType.MediaType == MediaTypeNames.Text.Plain)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                }
            }

            throw new Exception("There was an issue processing your connect request.");
        }

        private HttpRequestMessage BuildPostRequest(string endpoint, TokenResponse token,
            Authorization authorization)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
            request.Headers.Authorization =
                new AuthenticationHeaderValue(AuthenticationScheme, token.AccessToken);
            request.Content = new StringContent(authorization.Payload, Encoding.UTF8, JsonMediaType);
            return request;
        }

        private HttpRequestMessage BuildGetRequest(string endpoint, TokenResponse token,
            Authorization authorization)
        {
            var payload = (JObject)JsonConvert.DeserializeObject(authorization.Payload);
            var query = string.Join("&", payload.Children().Cast<JProperty>()
                .Select(jp => jp.Name + "=" + HttpUtility.UrlEncode(jp.Value.ToString())));
            var builder = new UriBuilder(endpoint)
            {
                Query = query
            };

            var request = new HttpRequestMessage(HttpMethod.Get, builder.Uri);
            request.Headers.Authorization =
                new AuthenticationHeaderValue(AuthenticationScheme, token.AccessToken);
            return request;
        }
    }

    public class ScopedDynamicClientRegistrationDocument : DynamicClientRegistrationDocument
    {
        public new string Scope { get; set; }
    }

    public class ScopedTokenRequest : TokenRequest
    {
        public string Scope { get; set; }
    }

    public class CredentialResponse : GenericModel
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }
    }
}
