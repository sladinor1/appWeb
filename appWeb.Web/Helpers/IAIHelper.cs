﻿using appWeb.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace appWeb.Web.Helpers
{
    public interface IAIHelper
    {
        Task<List<History>> GetBestAndWorst();
    }
}
