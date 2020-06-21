using CamelsClub.Models;
using CamelsClub.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Repositories
{
    public interface ICamelCompetitionRepository : IGenericRepository<CamelCompetition>
    {
        bool CheckCamelCompetition(CamelCompetitionCreateViewModel obj);
    }
}
