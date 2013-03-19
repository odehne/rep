using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MediaManager2010;
using MediaManager2010.BLL.Interfaces;

namespace MovieStackService.Controllers
{
    public class PasswortResetController : ApiController
    {
        private IFriendsRepository Repository { get; set; }

        public PasswortResetController(IFriendsRepository repository)
        {
            Repository = repository;
        }

        // GET api/passwortreset
        public IEnumerable<string> Get()
        {
            return new string[] {"value1", "value2"};
        }

        // GET api/passwortreset/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/passwortreset
        public PasswordResetModel Post([FromBody] string email)
        {
            var user = Repository.GetUserByEmail(email);
            if (user != null)
            {
                return new PasswordResetModel() {Command = System.Guid.NewGuid().ToString(), Email = email};
            }
            return null;
        }

        // PUT api/passwortreset/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/passwortreset/5
        public void Delete(int id)
        {
        }
    }

    public class PasswordResetModel
    {
        public string Command { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
        public string NewPassword { get; set; }
    }
}
