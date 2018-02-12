using System;
using System.Collections.Generic;
using Pulse8Models;


namespace Pulse8IBusiness.Interfaces
{
    public interface IPulse8Repository
    {
        List<MemberInfo> GetAllMemberInfo();
        List<MemberInfo> GetMemberInfo(int memberId);
    }
}
