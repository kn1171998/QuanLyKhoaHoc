using PayCompute.Service;
using WebData.Models;

namespace WebData.Implementation
{
    public interface ILessonCommentService : IBaseService<LessonComments>
    {
        quanlykhoahocContext GetContext();
    }

    public class LessonCommentService : RepositoryBaseService<LessonComments>, ILessonCommentService
    {
        public LessonCommentService(quanlykhoahocContext context) : base(context)
        {
        }
        public quanlykhoahocContext GetContext()
        {
            return _context;
        }
    }
}