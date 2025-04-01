using EmployeeManagement.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagement.Models
{
    // hồ sơ ứng viên
    [Table("candidate_profile")]
    public class CandidateProfile : BaseEntity
    {
        [Column("candidate_name")]
        public string? CandidateName { get; set; }  // Tên ứng viên

        [Column("date_of_birth")]
        public DateTime DateOfBirth { get; set; }  // Ngày sinh

        [Column("gender")]
        public Gender? Gender { get; set; }  // Giới tính

        [Column("address")]
        public string? Address { get; set; }  // Địa chỉ

        [Column("phone_number")]
        public string? PhoneNumber { get; set; }  // Số điện thoại

        [Column("email")]
        public string? Email { get; set; }  // Email

        [Column("education_level")]
        public string? EducationLevel { get; set; }  // Trình độ

        [Column("experience")]
        public string? Experience { get; set; }  // Kinh nghiệm

        [Column("applied_position")]
        public string? AppliedPosition { get; set; }  // Vị trí ứng tuyển

        [Column("application_date")]
        public DateTime ApplicationDate { get; set; }  // Ngày nộp hồ sơ

        [Column("notes")]
        public string? Notes { get; set; }  // Ghi chú

        [Column("foreign_language_proficiency")]
        public bool ForeignLanguageProficiency { get; set; }  // Trình độ ngoại ngữ

        [Column("citizen_id")]
        public string? CitizenID { get; set; }  // CCCD
    }
}