using PayCompute.Service;
using System;
using System.Collections.Generic;
using System.Text;
using WebData.Models;

namespace WebData.Implementation
{
    public interface IDiscountCourseService : IBaseService<DiscountCourse>
    {

    }
    public class DiscountCourseService : RepositoryBaseService<DiscountCourse>, IDiscountCourseService
    {
        public DiscountCourseService(quanlykhoahocContext context) : base(context)
        {
        }
    }
}
