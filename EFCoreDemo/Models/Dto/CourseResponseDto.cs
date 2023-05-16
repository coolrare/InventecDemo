using EFCoreDemo.Models;

namespace EFCoreDemo.Models.Dto
{
    public class CourseResponseDto
    {
        public int CourseId { get; set; }

        public string Title { get; set; } = null!;

        public int Credits { get; set; }

        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; } = null!;
    }
}