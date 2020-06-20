using PayCompute.Service;
using WebData.Models;

namespace WebData.Implementation
{
    public interface ICourseService : IBaseService<Courses>
    {
        quanlykhoahocContext GetContext();
    }

    public class CourseService : RepositoryBaseService<Courses>, ICourseService
    {
        public CourseService(quanlykhoahocContext context) : base(context)
        {
        }
        public quanlykhoahocContext GetContext()
        {
            return _context;
        }
    }
}