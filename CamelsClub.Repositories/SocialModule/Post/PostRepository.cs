using CamelsClub.Data.Context;
using CamelsClub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Repositories
{
    public class PostRepository : GenericRepository<Post> , IPostRepository
    {
        public PostRepository(CamelsClubContext context): base(context)
        {

        }
        //public IQueryable<Post> GetPosts()
        //{

        //} 
    }
}
