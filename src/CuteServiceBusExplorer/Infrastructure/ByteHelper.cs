using System;

namespace CuteServiceBusExplorer.Infrastructure
{
    public static class ByteHelper
    {
        public static string BytesToFriendlyString(long byteCount)
        {
            string[] suffix = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
            if (byteCount == 0)
                return "0.0" + suffix[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString("N1") + suffix[place];
        }
    }
}