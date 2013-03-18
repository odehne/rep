using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MediaManager2010;
using MediaManager2010.WCFContracts.V1;

namespace MovieStackService.Controllers
{
    public class FriendsController : ApiController
    {
        // GET api/friends
        public IEnumerable<User> Get()
        {
            var bll = new BLLFriends();
            return bll.GetData();
        }

        // GET api/friends/5
        public User Get(int id)
        {
            var bll = new BLLFriends();
            return bll.GetUserByID(id);
        }

        // GET api/friends/5
        public User GetUserByName(string name)
        {
            var bll = new BLLFriends();
            return bll.GetUserByName(name);
        }

        // POST api/friends
        public HttpResponseMessage Post(NewUserModel newUser)
        {
            var bll = new BLLFriends();
            var newId = bll.AddUser(new User() { Email = newUser.Email, Username = newUser.Username , Password = newUser.Password });
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
