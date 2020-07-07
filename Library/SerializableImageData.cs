using System;
using System.Collections.Generic;

namespace Microsoft.Research.EyeCatcher.Library
{
    public abstract class SerializableImageData : IImageData
    {
        private readonly IImageData _image;

        protected SerializableImageData(IImageData source)
        {
            _image = source;
        }

        public int CaptureIndex => _image.CaptureIndex;

        public DateTimeOffset Timestamp => _image.Timestamp;

        public string MD5Hash => _image.MD5Hash;

        public DateTimeOffset? BeforeTrackerTimestamp => _image.BeforeTrackerTimestamp;

        public double? BeforeTrackerX => _image.BeforeTrackerX;

        public double? BeforeTrackerY => _image.BeforeTrackerY;

        public DateTimeOffset? AfterTrackerTimestamp => _image.AfterTrackerTimestamp;

        public double? AfterTrackerX => _image.AfterTrackerX;

        public double? AfterTrackerY => _image.AfterTrackerY;

        public IPositionData GetPosition()
        {
            return _image.GetPosition();
        }
    }
}
