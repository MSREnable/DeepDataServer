using System;
using System.Collections.Generic;

namespace Microsoft.Research.EyeCatcher.Library
{
    public abstract class DeserializableSessionData : ISessionData
    {
        public Guid InstanceId { get; set; }

        public Guid UserId { get; set; }

        public Guid SessionId { get; set; }

        public string UserName { get; set; }

        public string AccountName { get; set; }

        public string CurrentOrientation { get; set; }

        public string PreviewRotation { get; set; }

        public double? DiagonalSizeInInches { get; set; }

        public double? Horizontal35mmEquivalentFocalLength { get; set; }

        public int HorizontalPoints { get; set; }

        public string HostManufacturer { get; set; }

        public string HostModel { get; set; }

        public string HostSku { get; set; }

        public int ImageHeight { get; set; }

        public int ImageWidth { get; set; }

        public int Margin { get; set; }

        public int MaxImagesPerPosition { get; set; }

        public int MinImagesPerPosition { get; set; }

        public string NativeOrientation { get; set; }

        public double RawDpiX { get; set; }

        public double RawDpiY { get; set; }

        public double ScreenHeightInRawPixels { get; set; }

        public double RawPixelsPerViewPixel { get; set; }

        public double ScreenWidthInRawPixels { get; set; }

        public DateTimeOffset SessionTimestamp { get; set; }

        public double? Vertical35mmEquivalentFocalLength { get; set; }

        public int VerticalPoints { get; set; }

        public string VideoCaptureId { get; set; }

        public string VideoCaptureName { get; set; }

        public string VideoCapturePanel { get; set; }

        public double WindowHeight { get; set; }

        public double WindowWidth { get; set; }

        public int BackgroundRed { get; set; }

        public int BackgroundGreen { get; set; }

        public int BackgroundBlue { get; set; }

        public bool IsCustomCameraPosition { get; set; }

        public double CameraX { get; set; }

        public double CameraY { get; set; }

        public double CameraMmX { get; set; }

        public double CameraMmY { get; set; }

        public abstract IEnumerable<IPositionData> GetPositions();
    }

    public abstract class DeserializableSessionData<TPosition> : DeserializableSessionData
        where TPosition : DeserializablePositionData
    {
        private readonly List<TPosition> _positions = new List<TPosition>();

        public override IEnumerable<IPositionData> GetPositions()
        {
            foreach (var position in _positions)
            {
                yield return position;
            }
        }

        public List<TPosition> GetTypedPositions() { return _positions; }
    }
}
