using PayCompute.Service;
using WebData.Models;

namespace WebData.Implementation
{
    public interface ICartService : IBaseService<Cart>
    {
    }

    public class CartService : RepositoryBaseService<Cart>, ICartService
    {
        public CartService(quanlykhoahocContext context) : base(context)
        {
        }
    }
}