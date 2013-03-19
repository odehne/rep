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
    public class ActorsController : ApiController
    {

        private IParticipantsRepository Repository { get; set; }

        public ActorsController(IParticipantsRepository repository)
        {
            Repository = repository;
        }
        // GET api/actors
        public IEnumerable<Participant> Get()
        {
            return Repository.GetData();
        }

        // GET api/actors/5
        public Participant Get(int id)
        {
            return Repository.GetParticipantByID(id);
        }

        // POST api/actors
        public void Post([FromBody]string value)
        {
        }

        // PUT api/actors/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/actors/5
        public void Delete(int id)
        {
        }
    }
}
