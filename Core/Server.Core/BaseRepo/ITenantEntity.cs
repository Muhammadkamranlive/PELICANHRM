﻿using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Core
{
    public interface ITenantEntity
    {
        int TenantId { get; set; }
    }
}
