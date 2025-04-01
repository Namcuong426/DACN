namespace EmployeeManagement.Dto
{
    public class BaseEntityDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? Email { get; set; } = null;
        public string? UserName { get; set; } = null;
    }
}