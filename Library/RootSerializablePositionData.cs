using System.Collections.Generic;

namespace Microsoft.Research.EyeCatcher.Library
{
    public class RootSerializablePositionData : SerializablePositionData
    {
        public RootSerializablePositionData(IPositionData position)
            : base(position)
        {
        }

        public IEnumerable<RootSerializableImageData> Images
        {
            get
            {
                foreach (var image in GetImages())
                {
                    yield return new RootSerializableImageData(image);
                }
            }
        }
    }
}
