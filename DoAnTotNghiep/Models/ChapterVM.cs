namespace DoAnTotNghiep.Models
{
    public class ChapterVM
    {
        public ChapterVM()
        {
        }

        public int Id { get; set; }
        public string NameChapter { get; set; }
        public int? CourseId { get; set; }
        public int? OrderChapter { get; set; }
    }
}