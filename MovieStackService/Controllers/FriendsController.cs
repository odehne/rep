using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MediaManager2010;
using MediaManager2010.BLL.Interfaces;
using MediaManager2010.WCFContracts.V1;

namespace MovieStackService.Controllers
{
    public class FriendsController : ApiController
    {
        private IFriendsRepository Repository { get; set; }

        public FriendsController(IFriendsRepository repository)
        {
            Repository = repository;
        }

        // GET api/friends
        public IEnumerable<User> Get()
        {
            return Repository.GetData();
        }

        // GET api/friends/5
        public User Get(int id)
        {
            return Repository.GetUserByID(id);
        }

        // GET api/friends/5
        public User GetUserByName(string name)
        {
            return Repository.GetUserByName(name);
        }

        // POST api/friends
        public HttpResponseMessage Post(NewUserModel newUser)
        {
            var newId = Repository.AddUser(new User() { Email = newUser.Email, Username = newUser.Username, Password = newUser.Password });
            return newId <= 0 ? new HttpResponseMessage(HttpStatusCode.BadRequest) : new HttpResponseMessage(HttpStatusCode.OK);
        }

        // PUT api/friends/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/friends/5
        public void Delete(int id)
        {
        }
    }
    public class NewUserModel
    {
        public string Password { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }
}
