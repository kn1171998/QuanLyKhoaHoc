using PayCompute.Service;
using System.Collections.Generic;
using System.Linq;
using WebData.Models;

namespace WebData.Implementation
{
    public interface ICourseCategoryService:IBaseService<CourseCategories>
    {  
        IEnumerable<CourseCategories> GetParentCategory();
        IEnumerable<CourseCategories> GetChildCategory(int? parentId);
    }
    public class CourseCategoryService : RepositoryBaseService<CourseCategories>, ICourseCategoryService
    {
        public CourseCategoryService(quanlykhoahocContext context) : base(context)
        {
        }

        public IEnumerable<CourseCategories> GetParentCategory()
        {
            var parent = _context.CourseCategories.Where(m => m.ParentId == DefineCommon.ParentCategory);
            return parent;
        }
        public IEnumerable<CourseCategories> GetChildCategory(int? parentId)
        => _context.CourseCategories.Where(m => m.ParentId == parentId);
    }
}
