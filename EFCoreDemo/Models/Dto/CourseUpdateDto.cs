namespace EFCoreDemo.Models.Dto
{
    public class CourseUpdateDto
    {
        public int CourseId { get; set; }

        public string Title { get; set; } = null!;

        public int Credits { get; set; }

        public DateTime ModifiedOn { get; } = DateTime.Now;

    }
}