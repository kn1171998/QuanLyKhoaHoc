using PayCompute.Service;
using WebData.Models;

namespace WebData.Implementation
{
    public interface IOrderDetailService : IBaseService<OrderDetails>
    {
    }

    public class OrderDetailService : RepositoryBaseService<OrderDetails>, IOrderDetailService
    {
        public OrderDetailService(quanlykhoahocContext context) : base(context)
        {
        }
    }
}