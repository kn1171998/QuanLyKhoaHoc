using PayCompute.Service;
using WebData.Models;

namespace WebData.Implementation
{
    public interface ICourseLessonService : IBaseService<CourseLessons>
    {
        quanlykhoahocContext GetContext();
    }

    public class CourseLessonService : RepositoryBaseService<CourseLessons>, ICourseLessonService
    {
        public CourseLessonService(quanlykhoahocContext context) : base(context)
        {
        }
        public quanlykhoahocContext GetContext()
        {
            return _context;
        }
    }
}