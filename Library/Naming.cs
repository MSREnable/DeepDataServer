using System;
using System.Collections.Generic;

namespace Microsoft.Research.EyeCatcher.Library
{
    public static class Naming
    {
        public const string LayoutVersionString = "200407";
        public const string AnnotationPart = "annotation";
        public const string AnnotationExtension = ".json";
        public const string AnnotationPartFile = AnnotationPart + AnnotationExtension;
        public const string ImagePart = "image";
        public const string ImageExtension = ".jpg";
        public const string ImagePartFile = ImagePart + ImageExtension;

        private static string GuidToString(Guid guid)
        {
            return (guid != Guid.Empty ? guid : Guid.NewGuid()).ToString("N");
        }

        public static string GetDefaultDeviceName(this ISessionData session)
        {
            var name = !string.IsNullOrWhiteSpace(session.HostSku) ? session.HostSku : session.InstanceId.ToString();
            return name;
        }

        public static string GetDefaultUserName(this ISessionData session)
        {
            var name = session.UserName ?? session.AccountName ?? session.UserId.ToString();
            return name;
        }

        public static string[] GetBlobHierarchyNames(Guid instanceId, Guid userId, Guid sessionId, DateTimeOffset timestamp)
        {
            var names = new[]
            {
                GuidToString(instanceId) + "-" + GuidToString(userId),
                timestamp.ToString("yyMMddHHmm-") + GuidToString(sessionId)
            };
            return names;
        }

        public static string[] GetBlobHierarchyNames(ISessionData session)
        {
            var names = GetBlobHierarchyNames(session.InstanceId, session.UserId, session.SessionId, session.SessionTimestamp);
            return names;
        }

        public static string[] GetFileHierarchyNames(ISessionData session, string forcedUserName = null)
        {
            var names = new[]
            {
                LayoutVersionString,
                GetDefaultDeviceName(session),
                forcedUserName ?? GetDefaultUserName(session)
            };
            return names;
        }

        public static string GetFileBaseName()
        {
            var baseName = Guid.NewGuid().ToString();
            return baseName;
        }

        public static string GetAnnotationFileName(string baseName)
        {
            return baseName + AnnotationExtension;
        }

        public static string GetImageFileName(string baseName)
        {
            return baseName + ImageExtension;
        }
    }
}
