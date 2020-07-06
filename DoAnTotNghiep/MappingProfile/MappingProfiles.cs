using AutoMapper;
using DoAnTotNghiep.Models;
using WebData.Models;

namespace DoAnTotNghiep.MappingProfile
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<CourseCategories, CourseCategoryVM>();
            CreateMap<CourseCategoryVM, CourseCategories>();
            CreateMap<CourseVM, Courses>();
            CreateMap<Courses, CourseVM>();
            CreateMap<Chapter, ChapterVM>();
            CreateMap<ChapterVM, Chapter>();
            CreateMap<Cart, CartVM>();
            CreateMap<CartVM, Cart>();
            CreateMap<CourseLessons, CourseLessonVM>();
            CreateMap<CourseLessonVM, CourseLessons>();
            CreateMap<Discount, DiscountVM>();
            CreateMap<DiscountVM, Discount>();
            CreateMap<LessonComments, LessonCommentVM>();
            CreateMap<LessonCommentVM, LessonComments>();
            CreateMap<Orders, OrderVM>();
            CreateMap<OrderVM, Orders>();
            CreateMap<OrderDetailVM, OrderDetails>();
            CreateMap<OrderDetails, OrderDetailVM>();
            CreateMap<Users, UserVM>();
            CreateMap<UserVM, Users>();
        }
    }
}