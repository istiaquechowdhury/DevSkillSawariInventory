﻿using Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.RepositoryPatternInterfaces
{
    public interface IReasonRepository : IRepositoryBase<Reason,Guid>
    {
    }
}
