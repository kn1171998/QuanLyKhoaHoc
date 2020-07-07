using PayCompute.Service;
using System;
using System.Collections.Generic;
using System.Text;
using WebData.Models;

namespace WebData.Implementation
{
    public interface IDiscountService:IBaseService<Discount>
    {
        quanlykhoahocContext GetContext();
    }
    public class DiscountService : RepositoryBaseService<Discount>, IDiscountService
    {
        public DiscountService(quanlykhoahocContext context) : base(context)
        {
        }
        public quanlykhoahocContext GetContext()
        {
            return _context;
        }
    }
}
