using Pulse8Models;
using Pulse8IBusiness.Interfaces;
using System.Collections.Generic;
using System.Web.Http;
using config = System.Configuration.ConfigurationManager;

namespace Pulse8WebApi.Controllers
{
    public class EntityController:ApiController
    {
        readonly IPulse8Repository repository;

        public EntityController()
        {
            repository = new Pulse8Business.Repositories.EntityRepository();
        }
        public EntityController(IPulse8Repository repository)
        {
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("Entity/getAllMembers")]
        public List<MemberInfo> getAllMembers()
        {
            return repository.GetAllMemberInfo();
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("Entity/getMember")]
        public List<MemberInfo> getMember([FromBody] int memberId)
        {
            return repository.GetMemberInfo(memberId);
        }
    }
}