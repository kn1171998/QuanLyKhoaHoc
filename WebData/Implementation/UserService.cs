using PayCompute.Service;
using WebData.Models;

namespace WebData.Implementation
{
    public interface IUserService : IBaseService<Users>
    {
    }

    public class UserService : RepositoryBaseService<Users>, IUserService
    {
        public UserService(quanlykhoahocContext context) : base(context)
        {
        }
    }
}