namespace EFCoreDemo.Models.Dto
{
    public class CourseUpdateResquestDto
    {
        public int CourseId { get; set; }

        public string Title { get; set; } = null!;

        public int Credits { get; set; }

        public DateTime ModifiedOn { get; } = DateTime.Now;

    }
}