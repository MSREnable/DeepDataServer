using System;
using System.Collections.Generic;

namespace Microsoft.Research.EyeCatcher.Library
{
    public class LeafSerializableImageData : SerializableImageData
    {
        public LeafSerializableImageData(IImageData image)
            : base(image)
        {
        }

        public LeafSerializablePositionData Position
        {
            get
            {
                return new LeafSerializablePositionData(GetPosition());
            }
        }
    }
}
