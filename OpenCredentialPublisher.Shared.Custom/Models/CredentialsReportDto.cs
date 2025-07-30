using System;

namespace OpenCredentialPublisher.Shared.Custom.Models
{
   public class CredentialsReportDto
   {
      public const string Header = "OfferedUsers,Offers,CredentialedUsers,CredentialPackagesGranted,CredentialsGranted,DateGenerated";
      public const string UsersHeader = "DisplayName,UserName,EmailConfirmed,CreatedAt,ModifiedAt";

      public int OfferedUsers { get; set; }
      public int Offers { get; set; }
      public int CredentialedUsers { get; set; }
      public int CredentialPackagesGranted { get; set; }
      public int CredentialsGranted { get; set; }
      public DateTime DateGenerated { get; set; }

      public CredentialsReportUserDto[] Users { get; set; }

      public string ToCsv()
      {
         var csv = $"{Header}"
            + $"\n{OfferedUsers},{Offers},{CredentialedUsers},{CredentialPackagesGranted},{CredentialsGranted},{DateGenerated:yyyy-MM-dd HH:mm:ss}";

         if (Users != null && Users.Length > 0)
         {
            csv += Environment.NewLine + Environment.NewLine + UsersHeader + Environment.NewLine;
            foreach (var user in Users)
            {
               csv += user.ToCsv() + Environment.NewLine;
            }
         }

         return csv;
      }
   }

   public class CredentialsReportUserDto
   {
      public string DisplayName { get; set; }
      public string UserName { get; set; }
      public bool EmailConfirmed { get; set; }
      public DateTimeOffset CreatedAt { get; set; }
      public DateTimeOffset? ModifiedAt { get; set; }

      public string ToCsv()
      {
         return $"{DisplayName ?? ""},{UserName ?? ""},{EmailConfirmed},{CreatedAt:yyyy-MM-dd HH:mm:ss},{ModifiedAt:yyyy-MM-dd HH:mm:ss}";
      }
   }
}
