using System;
using System.Collections.Generic;

namespace Microsoft.Research.EyeCatcher.Library
{
    public class LeafSerializableSessionData : SerializableSessionData
    {
        public LeafSerializableSessionData(ISessionData session)
            : base(session)
        {
        }
    }
}
