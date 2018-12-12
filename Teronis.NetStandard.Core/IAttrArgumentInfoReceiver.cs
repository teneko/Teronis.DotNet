using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Teronis.NetStandard
{
    public interface IAttrArgumentInfoReceiver
    {
        void ReceiveMemberInfo(MemberInfo memberInfo);
    }
}
