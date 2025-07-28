using System;
using System.IO;
using System.Threading.Tasks;
using LevelData.Credentials.DIDForge.Extensions;
using LevelData.Credentials.DIDForge.Services;
using Microsoft.Extensions.DependencyInjection;
using OpenCredentialPublisher.Proof;

namespace OpenCredentialPublisher.Tests.Proof;

public class ProofTests
{
    private readonly string _clrTestJsonValid;
    private readonly string _clrTestJsonTampered;
    private readonly IServiceProvider _serviceProvider;

    public ProofTests()
    {
        using var streamValid = new StreamReader(typeof(ProofTests).Assembly.GetManifestResourceStream($"{typeof(ProofTests).Namespace}.Files.clr2-valid.json")!);
        _clrTestJsonValid = streamValid.ReadToEnd();

        using var streamTampered = new StreamReader(typeof(ProofTests).Assembly.GetManifestResourceStream($"{typeof(ProofTests).Namespace}.Files.clr2-tampered.json")!);
        _clrTestJsonTampered = streamTampered.ReadToEnd();

        var services = new ServiceCollection();
        services.AddDidResolvers();
        services.AddTransient<IProofService, ProofService>();
        _serviceProvider = services.BuildServiceProvider();
    }

    [Fact]
    public async Task VerifyProof_ShouldReturnTrue_WhenClrJsonIsValid()
    {
        using var scope = _serviceProvider.CreateScope();
        var proofService = scope.ServiceProvider.GetService<IProofService>();
        var isVerified = await proofService.VerifyProof(_clrTestJsonValid);
        Assert.True(isVerified);
    }

    [Fact]
    public async Task VerifyProof_ShouldReturnFalse_WhenClrJsonHasBeenTampered()
    {
        using var scope = _serviceProvider.CreateScope();
        var proofService = scope.ServiceProvider.GetService<IProofService>();
        var isVerified = await proofService.VerifyProof(_clrTestJsonTampered);
        Assert.False(isVerified);
    }
}