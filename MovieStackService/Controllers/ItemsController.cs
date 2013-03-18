using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MediaManager2010;
using MediaManager2010.WCFContracts;
using MediaManager2010.WCFContracts.V1;

namespace MovieStackService.Controllers
{
    public class ItemsController : ApiController
    {
        // GET api/items
        public IEnumerable<MovieItem> Get()
        {
            var bll = new BLLItems();
            return bll.GetItems();
        }

        // GET api/items?borrowedById=100
        public IEnumerable<MovieItem> GetBorrowed(int borrowedById)
        {
            var bll = new BLLItems();
            return bll.GetItemsByBorrowerID(borrowedById);
        }


        // GET api/items?genreId=19
        public IEnumerable<MovieItem> GetByGenre(int genreId)
        {
            var bll = new BLLItems();
            return bll.GetItemsByGenreID(genreId);
        }

        // GET api/items?actorId=166
        public IEnumerable<MovieItem> GetByActorId(int actorId)
        {
            var bll = new BLLItems();
            return bll.GetItemsByActorID(actorId);
        }

        // GET api/items?letter=a
        public IEnumerable<OnlyTitle> GetTitles(string letter)
        {
            var bll = new BLLItems();
            return bll.GetTitlesBeginningWith(letter);
        }

        // GET api/items?lentById=166
        public IEnumerable<MovieItem> GetLent(int lentById)
        {
            var bll = new BLLItems();
            return bll.GetItemsByLenderByID(lentById);
        }

        // GET api/items/5
        public MovieItem Get(int id)
        {
            var bll = new BLLItems();
            return bll.GetItemByID(id);
        }

        // POST api/items
        public HttpResponseMessage Post([FromBody]string ean, [FromBody]int friendId)
        {
            if (!string.IsNullOrEmpty(ean))
            {
                var bll = new BLLItems(); 
                var search = new ItemLookup();
                var lst = search.Search(ean, ItemLookup.SearchTypeE.EAN);
                if (lst.Count > 0)
                {
                    var m = lst.First();
                    m.OwnerID = friendId;
                    var result = bll.UpdateItem(m);
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
