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
    public class GenreController : ApiController
    {

        private IGenreRepository Repository { get; set; }

        public GenreController(IGenreRepository repository)
        {
            Repository = repository;
        }

        // GET api/genre
        public IEnumerable<Genre> Get()
        {
            return Repository.GetGenres();
        }

        // GET api/genre/5
        public Genre Get(int id)
        {
            return Repository.GetGenreByID(id);
        }

        // POST api/genre
        public HttpResponseMessage Post([FromBody]string value)
        {
            var id = Repository.AddGenre(value);
            return id > 0 ? new HttpResponseMessage(HttpStatusCode.OK) : new HttpResponseMessage(HttpStatusCode.NotAcceptable);
        }

        // PUT api/genre/5
        public void Put(int id, [FromBody]string value)
        {
            Repository.UpdateGenre(id, value);
        }

        // DELETE api/genre/5
        public void Delete(int id)
        {
            Repository.DeleteGenre(id);
        }
    }
}
