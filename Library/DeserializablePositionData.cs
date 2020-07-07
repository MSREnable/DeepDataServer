using System.Collections.Generic;

namespace Microsoft.Research.EyeCatcher.Library
{
    public abstract class DeserializablePositionData : IPositionData
    {
        public int PositionIndex { get; set; }

        public int CaptureCount { get; set; } = int.MaxValue;

        public double XRaw { get; set; }

        public double YRaw { get; set; }

        public double X { get; set; }

        public double Y { get; set; }

        public abstract ISessionData GetSession();

        public abstract IEnumerable<IImageData> GetImages();
    }

    public abstract class DeserializablePositionData<TSession, TPosition, TImage> : DeserializablePositionData
        where TSession : DeserializableSessionData<TPosition>
        where TPosition : DeserializablePositionData
        where TImage : DeserializableImageData
    {
        private TSession _session;
        private readonly List<TImage> _images = new List<TImage>();

        public sealed override ISessionData GetSession() { return _session; }

        public TSession GetTypedSession() { return _session; }

        protected void SetTypedSession(TSession value)
        {
            if (_session != value)
            {
                var typedThis = (TPosition)(object)this;

                if (_session != null)
                {
                    _session.GetTypedPositions().Remove(typedThis);
                }

                _session = value;

                if (_session != null)
                {
                    _session.GetTypedPositions().Add(typedThis);
                }
            }
        }

        public sealed override IEnumerable<IImageData> GetImages()
        {
            foreach (var image in _images)
            {
                yield return image;
            }
        }

        public List<TImage> GetTypedImages() { return _images; }
    }
}
