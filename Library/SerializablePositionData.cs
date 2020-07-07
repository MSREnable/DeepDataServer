using System;
using System.Collections.Generic;

namespace Microsoft.Research.EyeCatcher.Library
{
    public abstract class SerializablePositionData : IPositionData
    {
        private readonly IPositionData _position;

        public SerializablePositionData(IPositionData position)
        {
            _position = position;
        }

        public int PositionIndex => _position.PositionIndex;

        public int CaptureCount => _position.CaptureCount;

        public double XRaw => _position.XRaw;

        public double YRaw => _position.YRaw;

        public double X => _position.X;

        public double Y => _position.Y;

        public IEnumerable<IImageData> GetImages()
        {
            return _position.GetImages();
        }

        public ISessionData GetSession()
        {
            return _position.GetSession();
        }
    }
}
