using PayCompute.Service;
using WebData.Models;

namespace WebData.Implementation
{
    public interface IChapterService : IBaseService<Chapter>
    {
    }

    public class ChapterService : RepositoryBaseService<Chapter>, IChapterService
    {
        public ChapterService(quanlykhoahocContext context) : base(context)
        {
        }
    }
}