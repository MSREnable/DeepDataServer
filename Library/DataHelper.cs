namespace Microsoft.Research.EyeCatcher.Library
{
    public static class DataHelper
    {
        public static bool SameData(this IImageData lhs, IImageData rhs)
        {
            var same =
                lhs.CaptureIndex == rhs.CaptureIndex &&
                lhs.Timestamp == rhs.Timestamp &&
                lhs.MD5Hash == rhs.MD5Hash &&
                lhs.BeforeTrackerTimestamp == rhs.BeforeTrackerTimestamp &&
                lhs.BeforeTrackerX == rhs.BeforeTrackerX &&
                lhs.BeforeTrackerY == rhs.BeforeTrackerY &&
                lhs.AfterTrackerTimestamp == rhs.AfterTrackerTimestamp &&
                lhs.AfterTrackerX == rhs.AfterTrackerX &&
                lhs.AfterTrackerY == rhs.AfterTrackerY;

            return same;
        }

        public static bool SameData(this IPositionData lhs, IPositionData rhs)
        {
            var same =
                lhs.PositionIndex == rhs.PositionIndex &&
                lhs.CaptureCount == rhs.CaptureCount &&
                lhs.XRaw == rhs.XRaw &&
                lhs.YRaw == rhs.YRaw &&
                lhs.X == rhs.X &&
                lhs.Y == rhs.Y;

            return same;
        }

        public static bool SameData(this ISessionData lhs, ISessionData rhs)
        {
            var same =
                lhs.InstanceId == rhs.InstanceId &&
                lhs.UserId == rhs.UserId &&
                lhs.SessionId == rhs.SessionId &&
                lhs.UserName == rhs.UserName &&
                lhs.AccountName == rhs.AccountName &&
                lhs.CurrentOrientation == rhs.CurrentOrientation &&
                lhs.PreviewRotation == rhs.PreviewRotation &&
                lhs.DiagonalSizeInInches == rhs.DiagonalSizeInInches &&
                lhs.Horizontal35mmEquivalentFocalLength == rhs.Horizontal35mmEquivalentFocalLength &&
                lhs.HorizontalPoints == rhs.HorizontalPoints &&
                lhs.HostManufacturer == rhs.HostManufacturer &&
                lhs.HostModel == rhs.HostModel &&
                lhs.HostSku == rhs.HostSku &&
                lhs.ImageHeight == rhs.ImageHeight &&
                lhs.ImageWidth == rhs.ImageWidth &&
                lhs.Margin == rhs.Margin &&
                lhs.MaxImagesPerPosition == rhs.MaxImagesPerPosition &&
                lhs.MinImagesPerPosition == rhs.MinImagesPerPosition &&
                lhs.NativeOrientation == rhs.NativeOrientation &&
                lhs.RawDpiX == rhs.RawDpiX &&
                lhs.RawDpiY == rhs.RawDpiY &&
                lhs.ScreenHeightInRawPixels == rhs.ScreenHeightInRawPixels &&
                lhs.RawPixelsPerViewPixel == rhs.RawPixelsPerViewPixel &&
                lhs.ScreenWidthInRawPixels == rhs.ScreenWidthInRawPixels &&
                lhs.SessionTimestamp == rhs.SessionTimestamp &&
                lhs.Vertical35mmEquivalentFocalLength == rhs.Vertical35mmEquivalentFocalLength &&
                lhs.VerticalPoints == rhs.VerticalPoints &&
                lhs.VideoCaptureId == rhs.VideoCaptureId &&
                lhs.VideoCaptureName == rhs.VideoCaptureName &&
                lhs.VideoCapturePanel == rhs.VideoCapturePanel &&
                lhs.WindowHeight == rhs.WindowHeight &&
                lhs.WindowWidth == rhs.WindowWidth &&
                lhs.BackgroundRed == rhs.BackgroundRed &&
                lhs.BackgroundGreen == rhs.BackgroundGreen &&
                lhs.BackgroundBlue == rhs.BackgroundBlue &&
                lhs.IsCustomCameraPosition == rhs.IsCustomCameraPosition &&
                lhs.CameraX == rhs.CameraX &&
                lhs.CameraY == rhs.CameraY &&
                lhs.CameraMmX == rhs.CameraMmX &&
                lhs.CameraMmY == rhs.CameraMmY;

            return same;
        }

        public static void CopyData(this DeserializableImageData lhs, IImageData rhs)
        {
            lhs.CaptureIndex = rhs.CaptureIndex;
            lhs.Timestamp = rhs.Timestamp;
            lhs.MD5Hash = rhs.MD5Hash;
            lhs.BeforeTrackerTimestamp = rhs.BeforeTrackerTimestamp;
            lhs.BeforeTrackerX = rhs.BeforeTrackerX;
            lhs.BeforeTrackerY = rhs.BeforeTrackerY;
            lhs.AfterTrackerTimestamp = rhs.AfterTrackerTimestamp;
            lhs.AfterTrackerX = rhs.AfterTrackerX;
            lhs.AfterTrackerY = rhs.AfterTrackerY;
        }

        public static void CopyData(this DeserializablePositionData lhs, IPositionData rhs)
        {
            lhs.PositionIndex = rhs.PositionIndex;
            lhs.CaptureCount = rhs.CaptureCount;
            lhs.XRaw = rhs.XRaw;
            lhs.YRaw = rhs.YRaw;
            lhs.X = rhs.X;
            lhs.Y = rhs.Y;
        }

        public static void CopyData(this DeserializableSessionData lhs, ISessionData rhs)
        {
            lhs.InstanceId = rhs.InstanceId;
            lhs.UserId = rhs.UserId;
            lhs.SessionId = rhs.SessionId;
            lhs.UserName = rhs.UserName;
            lhs.AccountName = rhs.AccountName;
            lhs.CurrentOrientation = rhs.CurrentOrientation;
            lhs.PreviewRotation = rhs.PreviewRotation;
            lhs.DiagonalSizeInInches = rhs.DiagonalSizeInInches;
            lhs.Horizontal35mmEquivalentFocalLength = rhs.Horizontal35mmEquivalentFocalLength;
            lhs.HorizontalPoints = rhs.HorizontalPoints;
            lhs.HostManufacturer = rhs.HostManufacturer;
            lhs.HostModel = rhs.HostModel;
            lhs.HostSku = rhs.HostSku;
            lhs.ImageHeight = rhs.ImageHeight;
            lhs.ImageWidth = rhs.ImageWidth;
            lhs.Margin = rhs.Margin;
            lhs.MaxImagesPerPosition = rhs.MaxImagesPerPosition;
            lhs.MinImagesPerPosition = rhs.MinImagesPerPosition;
            lhs.NativeOrientation = rhs.NativeOrientation;
            lhs.RawDpiX = rhs.RawDpiX;
            lhs.RawDpiY = rhs.RawDpiY;
            lhs.ScreenHeightInRawPixels = rhs.ScreenHeightInRawPixels;
            lhs.RawPixelsPerViewPixel = rhs.RawPixelsPerViewPixel;
            lhs.ScreenWidthInRawPixels = rhs.ScreenWidthInRawPixels;
            lhs.SessionTimestamp = rhs.SessionTimestamp;
            lhs.Vertical35mmEquivalentFocalLength = rhs.Vertical35mmEquivalentFocalLength;
            lhs.VerticalPoints = rhs.VerticalPoints;
            lhs.VideoCaptureId = rhs.VideoCaptureId;
            lhs.VideoCaptureName = rhs.VideoCaptureName;
            lhs.VideoCapturePanel = rhs.VideoCapturePanel;
            lhs.WindowHeight = rhs.WindowHeight;
            lhs.WindowWidth = rhs.WindowWidth;
            lhs.BackgroundRed = rhs.BackgroundRed;
            lhs.BackgroundGreen = rhs.BackgroundGreen;
            lhs.BackgroundBlue = rhs.BackgroundBlue;
            lhs.IsCustomCameraPosition = rhs.IsCustomCameraPosition;
            lhs.CameraX = rhs.CameraX;
            lhs.CameraY = rhs.CameraY;
            lhs.CameraMmX = rhs.CameraMmX;
            lhs.CameraMmY = rhs.CameraMmY;
        }
    }
}
