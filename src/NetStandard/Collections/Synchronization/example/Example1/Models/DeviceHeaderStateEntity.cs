using System;

namespace Teronis.Collections.Synchronization.Example1.Models
{
    public class DeviceHeaderStateEntity : Entity
    {
        public bool? IsOnline { get; private set; }
        public DateTime? BootTime { get; private set; }
        public DateTime? LastSeen { get; private set; }
        public float? CPUTempereature { get; private set; }
        public string? Model { get; private set; }
        public bool IsFactory { get; set; }
    }
}
