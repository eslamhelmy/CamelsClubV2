using CamelsClub.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Repositories
{
    public class CategoryRepository : GenericRepository<Models.Category> , ICategoryRepository
    {
        public CategoryRepository(CamelsClubContext context): base(context)
        {

        }
    }
}
