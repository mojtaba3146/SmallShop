using SmallShop.Infrastructure.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallShop.Services.Goodss.Contracts
{
    public interface GoodsService : Service
    {
        void Add(AddGoodsDto dto);
    }
}
