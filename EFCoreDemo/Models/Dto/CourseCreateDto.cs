namespace EFCoreDemo.Models.Dto
{
    public class CourseCreateDto
    {
        public string Title { get; set; } = null!;

        public int Credits { get; } = 1;

        public int DepartmentId { get; set; }
    }
}