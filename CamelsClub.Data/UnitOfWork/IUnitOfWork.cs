using CamelsClub.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Data.UnitOfWork
{
    public interface IUnitOfWork 
    {
        void Save();
        
    }
}
