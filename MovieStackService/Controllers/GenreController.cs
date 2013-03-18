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
    public class GenreController : ApiController
    {
        // GET api/genre
        public IEnumerable<Genre> Get()
        {
            var bll = new BLLGenre();
            return bll.GetGenres();
        }

        // GET api/genre/5
        public Genre Get(int id)
        {
            var bll = new BLLGenre();
            return bll.GetGenreByID(id);
        }

        // POST api/genre
        public void Post([FromBody]string value)
        {
            throw new NotImplementedException();
        }

        // PUT api/genre/5
        public void Put(int id, [FromBody]string value)
        {
            throw new NotImplementedException();
        }

        // DELETE api/genre/5
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
