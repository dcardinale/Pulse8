using Pulse8Models;
using Pulse8IBusiness.Interfaces;
using System.Collections.Generic;
using System.Web.Http;
using config = System.Configuration.ConfigurationManager;

namespace Pulse8WebApi.Controllers
{
    public class SQLController:ApiController
    {
        readonly IPulse8Repository repository;
        
        public SQLController()
        {
            repository = new Pulse8Business.Repositories.SQLRepository(config.ConnectionStrings["pulse8"].ConnectionString);
        }
        public SQLController(IPulse8Repository repo)
        {
            repository = repo;
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("SQL/GetAllMembers")]
        public List<MemberInfo> getAllMembers()
        {
            return repository.GetAllMemberInfo();
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("SQL/GetMember")]
        public List<MemberInfo> getMember([FromBody] int memberId)
        {
            return repository.GetMemberInfo(memberId);
        }
    }
}