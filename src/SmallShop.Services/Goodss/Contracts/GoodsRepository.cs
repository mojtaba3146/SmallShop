﻿using SmallShop.Entities;
using SmallShop.Infrastructure.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallShop.Services.Goodss.Contracts
{
    public interface GoodsRepository : Repository
    {
        void Add(Goods goods);
        bool IsExistGoodsName(string name, int categoryId);
        List<GetAllGoodsDto> GetAll();
    }
}
