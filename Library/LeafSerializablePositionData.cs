namespace Microsoft.Research.EyeCatcher.Library
{
    public class LeafSerializablePositionData : SerializablePositionData
    {
        public LeafSerializablePositionData(IPositionData position)
            : base(position)
        {
        }

        public LeafSerializableSessionData Session
        {
            get { return new LeafSerializableSessionData(GetSession()); }
        }
    }
}
