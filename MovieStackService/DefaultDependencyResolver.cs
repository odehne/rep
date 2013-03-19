using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;
using MediaManager2010;
using MediaManager2010.BLL.Interfaces;
using MovieStackService.Controllers;
namespace MovieStackService
{
    public class DefaultDependencyResolver:IDependencyResolver
    {
        public void Dispose()
        {
            // whatever is to clean up, we do that later
        }

        public IGenreRepository GenreRepository
        {
            get 
            {
                return new BLLGenre();
            }
        }
        public IItemsRepository ItemsRepository 
        {
            get
            {
                return new BLLItems();
            }
        }
        public IFriendsRepository FriendsRepository
        {
            get
            {
                return new BLLFriends();
            }
        }
        public IParticipantsRepository ParticipantsRepository
        {
            get
            {
                return new BLLParticipants();
            }
        }


        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(GenreController))
            {
                return new GenreController(GenreRepository);
            }
            if (serviceType == typeof(FriendsController))
            {
                return new FriendsController(FriendsRepository);
            }
            if (serviceType == typeof(ActorsController))
            {
                return new ActorsController(ParticipantsRepository);
            }
            if (serviceType == typeof(ItemsController))
            {
                return new ItemsController(ItemsRepository);
            }
            return null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return new List<object>();
        }

        public IDependencyScope BeginScope()
        {
            return this;
        }
    }
}