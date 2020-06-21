using CamelsClub.Data.Context;
using CamelsClub.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CamelsClub.Data.UnitOfWork
{
    //test
    public class UnitOfWork: IUnitOfWork
    {
        private readonly CamelsClubContext _context;
        public int UserID { set; get; }
        private bool _disposed;
        public UnitOfWork(CamelsClubContext context)
        {
            _context = context;
            // _dbContext = dbContextFactory.GetDbContext();
            SetUserID();
        }

        private void SetUserID()
        {
            try
            {
                string accessTokenHeaderName = "token";
                string accessToken = "";
                if (!HttpContext.Current.Request.Headers.AllKeys.Any(header => header == accessTokenHeaderName))
                    accessToken = "";
                else if (HttpContext.Current.Request.Headers.AllKeys.Any(header => header == accessTokenHeaderName))
                {
                    accessToken = HttpContext.Current.Request.Headers.GetValues(accessTokenHeaderName).First();
                }
                UserID = int.Parse(SecurityHelper.GetUserIDFromToken(accessToken).ToString());
              

            }
            catch (Exception ex)
            {
                //return 0;
            }

        }


        public void Save()
        {
                _context.SaveChanges();
          
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }
    }
}
