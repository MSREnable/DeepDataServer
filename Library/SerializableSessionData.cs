using System;
using System.Collections.Generic;

namespace Microsoft.Research.EyeCatcher.Library
{
    public abstract class SerializableSessionData : ISessionData
    {
        private readonly ISessionData _session;

        protected SerializableSessionData(ISessionData source)
        {
            _session = source;
        }

        public Guid InstanceId => _session.InstanceId;

        public Guid UserId => _session.UserId;

        public Guid SessionId => _session.SessionId;

        public string UserName => _session.UserName;

        public string AccountName => _session.AccountName;

        public string CurrentOrientation => _session.CurrentOrientation;

        public string PreviewRotation => _session.PreviewRotation;

        public double? DiagonalSizeInInches => _session.DiagonalSizeInInches;

        public double? Horizontal35mmEquivalentFocalLength => _session.Horizontal35mmEquivalentFocalLength;

        public int HorizontalPoints => _session.HorizontalPoints;

        public string HostManufacturer => _session.HostManufacturer;

        public string HostModel => _session.HostModel;

        public string HostSku => _session.HostSku;

        public int ImageHeight => _session.ImageHeight;

        public int ImageWidth => _session.ImageWidth;

        public int Margin => _session.Margin;

        public int MaxImagesPerPosition => _session.MaxImagesPerPosition;

        public int MinImagesPerPosition => _session.MinImagesPerPosition;

        public string NativeOrientation => _session.NativeOrientation;

        public double RawDpiX => _session.RawDpiX;

        public double RawDpiY => _session.RawDpiY;

        public double ScreenHeightInRawPixels => _session.ScreenHeightInRawPixels;

        public double RawPixelsPerViewPixel => _session.RawPixelsPerViewPixel;

        public double ScreenWidthInRawPixels => _session.ScreenWidthInRawPixels;

        public DateTimeOffset SessionTimestamp => _session.SessionTimestamp;

        public double? Vertical35mmEquivalentFocalLength => _session.Vertical35mmEquivalentFocalLength;

        public int VerticalPoints => _session.VerticalPoints;

        public string VideoCaptureId => _session.VideoCaptureId;

        public string VideoCaptureName => _session.VideoCaptureName;

        public string VideoCapturePanel => _session.VideoCapturePanel;

        public double WindowHeight => _session.WindowHeight;

        public double WindowWidth => _session.WindowWidth;

        public int BackgroundRed => _session.BackgroundRed;

        public int BackgroundGreen => _session.BackgroundGreen;

        public int BackgroundBlue => _session.BackgroundBlue;

        public bool IsCustomCameraPosition => _session.IsCustomCameraPosition;

        public double CameraX => _session.CameraX;

        public double CameraY => _session.CameraY;

        public double CameraMmX => _session.CameraMmX;

        public double CameraMmY => _session.CameraMmY;

        public IEnumerable<IPositionData> GetPositions()
        {
            return _session.GetPositions();
        }
    }
}
