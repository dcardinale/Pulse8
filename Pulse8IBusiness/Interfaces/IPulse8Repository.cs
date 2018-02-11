using System;
using System.Collections.Generic;
using Pulse8Models;


namespace Pulse8IBusiness.Interfaces
{
    public interface IPulse8Repository
    {
        List<MemberInfo> GetAllMembers();
        MemberInfo GetMemberById(int memberId);
    }
}
