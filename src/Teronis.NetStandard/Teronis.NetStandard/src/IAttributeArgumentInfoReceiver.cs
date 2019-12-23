using System.Reflection;

namespace Teronis
{
    public interface IAttributeArgumentInfoReceiver
    {
        void ReceiveMemberInfo(MemberInfo memberInfo);
    }
}
