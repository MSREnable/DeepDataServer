using System;
using System.IO;

namespace DeepDataServer
{
    internal class Storage
    {
        private const string DataRoot = "/data";
        private const string SchemaVersion = "200407";
        private const string UsersDirectoryName = "private/users";
        internal readonly static DirectoryInfo StorageDirectory = Directory.CreateDirectory($"{DataRoot}/{SchemaVersion}");
        internal readonly static DirectoryInfo UsersDirectory = Directory.CreateDirectory($"{DataRoot}/{SchemaVersion}/{UsersDirectoryName}");

        internal static string SanitizeFileName(string fileName)
        {
            return String.Join("_", fileName.Split(Path.GetInvalidFileNameChars(), StringSplitOptions.RemoveEmptyEntries)).TrimEnd('.');
        }

    }
}