using System.ComponentModel.DataAnnotations;

namespace EFCoreDemo.Models.Dto
{
    public class CourseCreateRequestDto : IValidatableObject
    {
        [Required(ErrorMessage = "請務必填寫課程標題")]
        [MinLength(3, ErrorMessage = "課程標題至少需要 3 個字元以上")]
        //[RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$", ErrorMessage = "課程標題必須以大寫英文字母開頭")]
        public string Title { get; set; } = null!;

        [Range(1, 5, ErrorMessage = "課程評價必須介於 1 到 5 之間")]
        public int Credits { get; set; } = 1;

        public int DepartmentId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Title.Contains("Git") && Credits > 1)
            {
                yield return new ValidationResult("當建立 Git 相關課程時，課程評價必須等於 1", 
                    new[] { nameof(Credits) });
            }

            //if (Title.Contains("Git") && Credits > 1)
            //{
            //    yield return new ValidationResult("當建立 Git 相關課程時，課程評價必須等於 1",
            //        new[] { nameof(Credits) });
            //}
        }
    }
}