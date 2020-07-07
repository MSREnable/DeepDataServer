using System;

namespace Microsoft.Research.EyeCatcher.Library
{
    public abstract class DeserializableImageData : IImageData
    {
        public int CaptureIndex { get; set; }

        public DateTimeOffset Timestamp { get; set; }

        public string MD5Hash { get; set; }

        public DateTimeOffset? BeforeTrackerTimestamp { get; set; }

        public double? BeforeTrackerX { get; set; }

        public double? BeforeTrackerY { get; set; }

        public DateTimeOffset? AfterTrackerTimestamp { get; set; }

        public double? AfterTrackerX { get; set; }

        public double? AfterTrackerY { get; set; }

        public abstract IPositionData GetPosition();
    }

    public abstract class DeserializableImageData<TSession, TPosition, TImage> : DeserializableImageData
        where TSession : DeserializableSessionData<TPosition>
        where TPosition : DeserializablePositionData<TSession, TPosition, TImage>
        where TImage : DeserializableImageData
    {
        private TPosition _position;

        public sealed override IPositionData GetPosition() { return _position; }

        public TPosition GetTypedPosition() { return _position; }

        public void SetTypedPosition(TPosition position)
        {
            var typedThis = (TImage)(object)this;

            if (_position != null)
            {
                _position.GetTypedImages().Remove(typedThis);
            }

            _position = position;

            if (_position != null)
            {
                _position.GetTypedImages().Add(typedThis);
            }
        }
    }
}
