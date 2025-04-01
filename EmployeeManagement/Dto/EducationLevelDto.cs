namespace EmployeeManagement.Dto
{
    public class EducationLevelDto : BaseEntityDto
    {
        public string? EducationLevelName { get; set; }  // Tên trình độ (e.g., Skill)
        public string? Description { get; set; }  // Mô tả
        public string? Abbreviation { get; set; }  // Viết tắt
    }
}