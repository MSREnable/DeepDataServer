using System;
using System.Collections.Generic;

namespace Microsoft.Research.EyeCatcher.Library
{
    public class LeafDeserializableImageData : DeserializableImageData<LeafDeserializableSessionData, LeafDeserializablePositionData, LeafDeserializableImageData>, IPositionData, ISessionData
    {
        public LeafDeserializableImageData()
        {
            Position = new LeafDeserializablePositionData();
        }

        public LeafDeserializablePositionData Position
        {
            get { return GetTypedPosition(); }
            set { SetTypedPosition(value); }
        }

        ISessionData IPositionData.GetSession()
        {
            var position = GetPosition();
            return position.GetSession();
        }

        IEnumerable<IImageData> IPositionData.GetImages()
        {
            var position = GetPosition();
            return position.GetImages();
        }

        IEnumerable<IPositionData> ISessionData.GetPositions()
        {
            var position = GetPosition();
            var session = position.GetSession();
            return session.GetPositions();
        }

        #region Legacy Properties
        public int PositionIndex { get { return Position.PositionIndex; } set { Position.PositionIndex = value; } }

        public int CaptureCount { get { return Position.CaptureCount; } set { Position.CaptureCount = value; } }

        public double XRaw { get { return Position.XRaw; } set { Position.XRaw = value; } }

        public double YRaw { get { return Position.YRaw; } set { Position.YRaw = value; } }

        public double X { get { return Position.X; } set { Position.X = value; } }

        public double Y { get { return Position.Y; } set { Position.Y = value; } }

        public Guid InstanceId { get { return Position.Session.InstanceId; } set { Position.Session.InstanceId = value; } }

        public Guid UserId { get { return Position.Session.UserId; } set { Position.Session.UserId = value; } }

        public Guid SessionId { get { return Position.Session.SessionId; } set { Position.Session.SessionId = value; } }

        public string UserName { get { return Position.Session.UserName; } set { Position.Session.UserName = value; } }

        public string AccountName { get { return Position.Session.AccountName; } set { Position.Session.AccountName = value; } }

        public string CurrentOrientation { get { return Position.Session.CurrentOrientation; } set { Position.Session.CurrentOrientation = value; } }

        public string PreviewRotation { get { return Position.Session.PreviewRotation; } set { Position.Session.PreviewRotation = value; } }

        public double? DiagonalSizeInInches { get { return Position.Session.DiagonalSizeInInches; } set { Position.Session.DiagonalSizeInInches = value; } }

        public double? Horizontal35mmEquivalentFocalLength { get { return Position.Session.Horizontal35mmEquivalentFocalLength; } set { Position.Session.Horizontal35mmEquivalentFocalLength = value; } }

        public int HorizontalPoints { get { return Position.Session.HorizontalPoints; } set { Position.Session.HorizontalPoints = value; } }

        public string HostManufacturer { get { return Position.Session.HostManufacturer; } set { Position.Session.HostManufacturer = value; } }

        public string HostModel { get { return Position.Session.HostModel; } set { Position.Session.HostModel = value; } }

        public string HostSku { get { return Position.Session.HostSku; } set { Position.Session.HostSku = value; } }

        public int ImageHeight { get { return Position.Session.ImageHeight; } set { Position.Session.ImageHeight = value; } }

        public int ImageWidth { get { return Position.Session.ImageWidth; } set { Position.Session.ImageWidth = value; } }

        public int Margin { get { return Position.Session.Margin; } set { Position.Session.Margin = value; } }

        public int MaxImagesPerPosition { get { return Position.Session.MaxImagesPerPosition; } set { Position.Session.MaxImagesPerPosition = value; } }

        public int MinImagesPerPosition { get { return Position.Session.MinImagesPerPosition; } set { Position.Session.MinImagesPerPosition = value; } }

        public string NativeOrientation { get { return Position.Session.NativeOrientation; } set { Position.Session.NativeOrientation = value; } }

        public double RawDpiX { get { return Position.Session.RawDpiX; } set { Position.Session.RawDpiX = value; } }

        public double RawDpiY { get { return Position.Session.RawDpiY; } set { Position.Session.RawDpiY = value; } }

        public double ScreenHeightInRawPixels { get { return Position.Session.ScreenHeightInRawPixels; } set { Position.Session.ScreenHeightInRawPixels = value; } }

        public double RawPixelsPerViewPixel { get { return Position.Session.RawPixelsPerViewPixel; } set { Position.Session.RawPixelsPerViewPixel = value; } }

        public double ScreenWidthInRawPixels { get { return Position.Session.ScreenWidthInRawPixels; } set { Position.Session.ScreenWidthInRawPixels = value; } }

        public DateTimeOffset SessionTimestamp { get { return Position.Session.SessionTimestamp; } set { Position.Session.SessionTimestamp = value; } }

        public double? Vertical35mmEquivalentFocalLength { get { return Position.Session.Vertical35mmEquivalentFocalLength; } set { Position.Session.Vertical35mmEquivalentFocalLength = value; } }

        public int VerticalPoints { get { return Position.Session.VerticalPoints; } set { Position.Session.VerticalPoints = value; } }

        public string VideoCaptureId { get { return Position.Session.VideoCaptureId; } set { Position.Session.VideoCaptureId = value; } }

        public string VideoCaptureName { get { return Position.Session.VideoCaptureName; } set { Position.Session.VideoCaptureName = value; } }

        public string VideoCapturePanel { get { return Position.Session.VideoCapturePanel; } set { Position.Session.VideoCapturePanel = value; } }

        public double WindowHeight { get { return Position.Session.WindowHeight; } set { Position.Session.WindowHeight = value; } }

        public double WindowWidth { get { return Position.Session.WindowWidth; } set { Position.Session.WindowWidth = value; } }

        public int BackgroundRed { get { return Position.Session.BackgroundRed; } set { Position.Session.BackgroundRed = value; } }

        public int BackgroundGreen { get { return Position.Session.BackgroundGreen; } set { Position.Session.BackgroundGreen = value; } }

        public int BackgroundBlue { get { return Position.Session.BackgroundBlue; } set { Position.Session.BackgroundBlue = value; } }

        public bool IsCustomCameraPosition { get { return Position.Session.IsCustomCameraPosition; } set { Position.Session.IsCustomCameraPosition = value; } }

        public double CameraX { get { return Position.Session.CameraX; } set { Position.Session.CameraX = value; } }

        public double CameraY { get { return Position.Session.CameraY; } set { Position.Session.CameraY = value; } }

        public double CameraMmX { get { return Position.Session.CameraMmX; } set { Position.Session.CameraMmX = value; } }

        public double CameraMmY { get { return Position.Session.CameraMmY; } set { Position.Session.CameraMmY = value; } }

        #endregion Legacy Properties

    }
}
