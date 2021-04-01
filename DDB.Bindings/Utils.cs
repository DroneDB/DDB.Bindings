using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDB.Bindings
{
    public static class Utils
    {
        private static readonly DateTime UnixEpoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime UnixTimestampToDateTime(long timestamp)
        {
            return UnixEpoch.Add(TimeSpan.FromSeconds(timestamp)).ToLocalTime();
        }

    }
}
