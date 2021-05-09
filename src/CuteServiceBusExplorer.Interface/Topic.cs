using System;

namespace CuteServiceBusExplorer.Interface
{
    public class Topic
    {
        public string Name { get; set; }
        public long CurrentSizeBytes { get; set; }
        public long MaximumSizeBytes { get; set; }
        public TimeSpan MessageTimeToLive { get; set; }
        public TimeSpan? AutoDelete { get; set; }
        public decimal FreeSpace => 1.0m - (((decimal) CurrentSizeBytes) / ((decimal) MaximumSizeBytes));
    }
}