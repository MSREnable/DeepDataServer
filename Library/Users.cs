using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DeepDataServer
{

    internal class Users
    {
        private const string UserIdSalt = "NeverPunt-";
        private const string UserConsent = "UserConsent";
        private const string UserId = "UserId";

        internal static string CreateUserHash(string userId)
        {
            using (var hash = MD5.Create())            
            {
                var userHash = Storage.SanitizeFileName(
                    Convert.ToBase64String(
                        hash.ComputeHash(
                            Encoding.UTF8.GetBytes(
                                UserIdSalt + userId.ToLowerInvariant()
                            )
                        )
                    ).Replace('/','_').TrimEnd('=')
                );

                var userDirectory = Storage.UsersDirectory.CreateSubdirectory($"{userHash}");
                var fullPath = Path.Combine(userDirectory.FullName, $"{UserId}");
                if (!(new FileInfo(fullPath).Exists))
                {
                    using (var streamWriter = new StreamWriter(fullPath))
                    {
                        streamWriter.Write(userId);
                    }
                }

                return userHash;
            }
        }

        internal static bool CheckUserConsent(string userId)
        {
            var userHash = CreateUserHash(userId);
            return new FileInfo(Path.Combine(Storage.UsersDirectory.FullName, $"{userHash}", $"{UserConsent}")).Exists;
        }

        internal static void SetUserConsent(string userId)
        {
            var userHash = CreateUserHash(userId);

            var userDirectory = Storage.UsersDirectory.CreateSubdirectory($"{userHash}");
            var fullPath = Path.Combine(userDirectory.FullName, $"{UserConsent}");
            if (!(new FileInfo(fullPath).Exists))
            {
                using (var streamWriter = new StreamWriter(fullPath))
                {
                    streamWriter.Write("I consent!");
                }
            }
        }

    }
}