using CamelsClub.Data.Context;
using CamelsClub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CamelsClub.Repositories
{
    public class CommentDocumentRepository : GenericRepository<CommentDocument> , ICommentDocumentRepository
    {
        public CommentDocumentRepository(CamelsClubContext context): base(context)
        {

        }
    }
}
