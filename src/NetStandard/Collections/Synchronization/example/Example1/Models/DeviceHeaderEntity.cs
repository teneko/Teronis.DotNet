using System.Diagnostics;

namespace Teronis.Collections.Synchronization.Example1.Models
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class DeviceHeaderEntity : Entity
    {
        public string? Serial { get; set; }
        public string? SerialAlias { get; set; }
        public string? ClientCode { get; set; }
        public string? Language { get; set; }
        public int? Timezone { get; set; }

        public DeviceHeaderStateEntity? State { get; set; }

        private string GetDebuggerDisplay() =>
            $"{GetType()}, {nameof(Serial)} = {Serial}";
    }
}
