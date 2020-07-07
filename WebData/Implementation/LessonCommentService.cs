using PayCompute.Service;
using WebData.Models;

namespace WebData.Implementation
{
    public interface ILessonCommentService : IBaseService<LessonComments>
    {
    }

    public class LessonCommentService : RepositoryBaseService<LessonComments>, ILessonCommentService
    {
        public LessonCommentService(quanlykhoahocContext context) : base(context)
        {
        }
    }
}