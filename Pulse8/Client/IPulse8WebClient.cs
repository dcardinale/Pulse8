using Pulse8Models;
using System.Collections.Generic;

namespace Pulse8.Client
{
    public interface IPulse8WebClient
    {
        List<MemberInfo> GetAllMemberInfo();
        List<MemberInfo> GetMemberInfo(int id);
    }
}
