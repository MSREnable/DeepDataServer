using System;
using System.Collections.Generic;

namespace Microsoft.Research.EyeCatcher.Library
{
    public class RootSerializableSessionData : SerializableSessionData
    {
        public RootSerializableSessionData(ISessionData session)
            : base(session)
        {
        }

        public IEnumerable<RootSerializablePositionData> Positions
        {
            get
            {
                foreach (var position in GetPositions())
                {
                    yield return new RootSerializablePositionData(position);
                }
            }
        }
    }
}
