﻿using CamelsClub.ViewModels;
using CamelsClub.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Services
{
    public interface IConditionsAndTermsService
    {
        ConditionsAndTermsViewModel Get(Languages language = Languages.Arabic);
    }
}
