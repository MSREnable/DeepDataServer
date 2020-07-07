using System.Collections.Generic;

namespace Microsoft.Research.EyeCatcher.Library
{
    public class LeafDeserializablePositionData : DeserializablePositionData<LeafDeserializableSessionData, LeafDeserializablePositionData, LeafDeserializableImageData>
    {
        public LeafDeserializablePositionData()
        {
            Session = new LeafDeserializableSessionData();
        }

        public LeafDeserializableSessionData Session
        {
            get { return GetTypedSession(); }
            set { SetTypedSession(value); }
        }
    }
}
