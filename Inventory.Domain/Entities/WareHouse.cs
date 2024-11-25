﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Entities
{
    public class WareHouse : IEntity<Guid>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public List<StockList> StockList { get; set; } 
    }
}