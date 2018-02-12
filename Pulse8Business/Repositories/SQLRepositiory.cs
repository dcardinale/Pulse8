using System;
using System.Collections.Generic;
using Pulse8IBusiness.Interfaces;
using Pulse8Models;
using Pulse8ADO;

namespace Pulse8Business.Repositories
{
    public class SQLRepository : IPulse8Repository
    {
        readonly string connectionString = "";

        public SQLRepository (string connection)
        {
            connectionString = connection;
        }
        public List<MemberInfo> GetAllMemberInfo()
        {
            return new Pulse8ADO.ADOModel(connectionString).getMemberInfo();
        }

        public List<MemberInfo> GetMemberInfo(int memberId)
        {
            return new Pulse8ADO.ADOModel(connectionString).getMemberInfo(memberId);
        }
    }
}
