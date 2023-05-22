using System.ComponentModel.DataAnnotations;

namespace EFCoreDemo.Models.Dto
{
    public class CourseCreateDto
    {
        [Required(ErrorMessage = "請務必填寫課程標題")]
        [MinLength(3, ErrorMessage = "課程標題至少需要 3 個字元以上")]
        public string Title { get; set; } = null!;

        [Range(1, 5, ErrorMessage = "課程評價必須介於 1 到 5 之間")]
        public int Credits { get; set; } = 1;

        public int DepartmentId { get; set; }
    }
}