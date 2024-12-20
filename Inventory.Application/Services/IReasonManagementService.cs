﻿using Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Services
{
    public interface IReasonManagementService
    {
        void CreateReason(Reason reason);
        Reason GetReason(Guid reasonId);
        Task<IList<Reason>> GetReasonsAsync();
    }
}
 