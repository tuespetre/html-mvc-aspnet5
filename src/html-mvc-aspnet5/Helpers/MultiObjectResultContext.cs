using System;
using System.Collections.Generic;

namespace html_mvc_aspnet5.Helpers
{
    public class MultiObjectResultContext
    {
        public IDictionary<string, Type> AdditionalObjects { get; } = new Dictionary<string, Type>();
    }
}
