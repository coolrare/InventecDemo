namespace EFCoreDemo.Models.Dto
{
    public class CourseCreateResponseDto
    {
        public int CourseId { get; set; }

        public string Title { get; set; }

        public int Credits { get; set; }

        public int DepartmentId { get; set; }

        public DateTime ModifiedOn { get; set; }
    }
}