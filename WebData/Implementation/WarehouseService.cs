using PayCompute.Service;
using System;
using System.Collections.Generic;
using System.Text;
using WebData.Models;

namespace WebData.Implementation
{
    public interface IWareHouseService:IBaseService<WareHouse>
    {

    }
    public class WareHouseService : RepositoryBaseService<WareHouse>, IWareHouseService
    {
        public WareHouseService(quanlykhoahocContext context) : base(context)
        {
        }
    }
}
