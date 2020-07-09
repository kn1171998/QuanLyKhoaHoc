using PayCompute.Service;
using WebData.Models;

namespace WebData.Implementation
{
    public interface IOrderService : IBaseService<Orders>
    {
        quanlykhoahocContext GetContext();
    }

    public class OrderService : RepositoryBaseService<Orders>, IOrderService
    {
        public OrderService(quanlykhoahocContext context) : base(context)
        {
        }
        public quanlykhoahocContext GetContext()
        {
            return _context;
        }
    }
}