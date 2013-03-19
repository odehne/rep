using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MediaManager2010;
using MediaManager2010.BLL.Interfaces;
using MediaManager2010.WCFContracts;
using MediaManager2010.WCFContracts.V1;

namespace MovieStackService.Controllers
{
    public class ItemsController : ApiController
    {
        private IItemsRepository Repository { get; set; }

        public ItemsController(IItemsRepository repository)
        {
            Repository = repository;
        }
        
        // GET api/items
        public IEnumerable<MovieItem> Get()
        {
            return Repository.GetItems();
        }

        // GET api/items?borrowedById=100
        public IEnumerable<MovieItem> GetBorrowed(int borrowedById)
        {
            return Repository.GetItemsByBorrowerID(borrowedById);
        }


        // GET api/items?genreId=19
        public IEnumerable<MovieItem> GetByGenre(int genreId)
        {
            return Repository.GetItemsByGenreID(genreId);
        }

        // GET api/items?actorId=166
        public IEnumerable<MovieItem> GetByActorId(int actorId)
        {
            return Repository.GetItemsByActorID(actorId);
        }

        // GET api/items?letter=a
        public IEnumerable<OnlyTitle> GetTitles(string letter)
        {
            return Repository.GetTitlesBeginningWith(letter);
        }

        // GET api/items?lentById=166
        public IEnumerable<MovieItem> GetLent(int lentById)
        {
            return Repository.GetItemsByLenderByID(lentById);
        }

        // GET api/items/5
        public MovieItem Get(int id)
        {
            return Repository.GetItemByID(id);
        }

        // POST api/items
        public HttpResponseMessage Post([FromBody]string ean, [FromBody]int friendId)
        {
            if (!string.IsNullOrEmpty(ean))
            {
                var search = new ItemLookup();
                var lst = search.Search(ean, ItemLookup.SearchTypeE.EAN);
                if (lst.Count > 0)
                {
                    var m = lst.First();
                    m.OwnerID = friendId;
                    var result = Repository.UpdateItem(m);
                    return new HttpResponseMessage() {StatusCode = HttpStatusCode.OK};
                }
                return new HttpResponseMessage() { StatusCode = HttpStatusCode.NotFound };
            }
            return new HttpResponseMessage() { StatusCode = HttpStatusCode.BadRequest }; ;
        }

        // PUT api/items/5
        public HttpResponseMessage Put(int id, [FromBody]int borrowTo, [FromBody]int lentTo)
        {
            //TODO: Hier das ausleihen übernehmen
            return null;
        }

        // PUT api/items/5
        public HttpResponseMessage PutReturnMovie(int id)
        {
            //TODO: Hier Film zurückgeben
            return null;
        }

        // DELETE api/items/5
        public void Delete(int id)
        {
        }
    }
}
