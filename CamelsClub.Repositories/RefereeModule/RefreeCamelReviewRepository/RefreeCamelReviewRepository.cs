using CamelsClub.Data.Context;
using CamelsClub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Repositories
{
    public class RefreeCamelReviewRepository : GenericRepository<RefereeCamelSpecificationReview> , IRefreeCamelReviewRepository
    {
        public RefreeCamelReviewRepository(CamelsClubContext context): base(context)
        {

        }
    }
}
