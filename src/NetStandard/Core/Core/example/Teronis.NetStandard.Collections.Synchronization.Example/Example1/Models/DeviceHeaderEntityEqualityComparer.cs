using System.Collections.Generic;

namespace Teronis.Collections.Synchronization.Example1.Models
{
    public class DeviceHeaderEntityEqualityComparer : EqualityComparer<DeviceHeaderEntity>
    {
        public new static readonly DeviceHeaderEntityEqualityComparer Default = new DeviceHeaderEntityEqualityComparer();

        public override bool Equals(DeviceHeaderEntity x, DeviceHeaderEntity y)
        {
            if (x == null && y == null)
                return true;
            else if (x == null || y == null)
                return false;
            else if (x.Serial == y.Serial)
                return true;
            else
                return false;
        }

        public override int GetHashCode(DeviceHeaderEntity obj)
        {
            var hash = 17;
            hash = hash * 17 + obj.Serial.GetHashCode();
            return hash;
        }
    }
}
