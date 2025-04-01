using EmployeeManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Context
{
    public class EmployeeManagementContext : DbContext
    {
        public EmployeeManagementContext(DbContextOptions options) : base(options)
        {

        }
        public virtual DbSet<Allowance> Allowances { get; set; }
        public virtual DbSet<Attendance> Attendances { get; set; }
        public virtual DbSet<CandidateProfile> CandidateProfiles { get; set; }
        public virtual DbSet<Contract> Contracts { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<EducationLevel> EducationLevels { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<Reward> Rewards { get; set; }
        public virtual DbSet<Insurance> Insurances { get; set; }
        public virtual DbSet<Salary> Salaries { get; set; }
        public virtual DbSet<SalaryAdvance> SalaryAdvances { get; set; }
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Penalize> Penalizes { get; set; }
        public virtual DbSet<Leave> Leaves { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SalaryAdvance>()
                .Property(sa => sa.AdvanceAmount)
                .HasPrecision(18,4);

            modelBuilder.Entity<Reward>()
                .Property(sa => sa.Amount)
                .HasColumnType("decimal(18,4)");

            modelBuilder.Entity<Penalize>()
                .Property(sa => sa.Amount)
                .HasColumnType("decimal(18,4)");

            modelBuilder.Entity<Allowance>()
                .Property(sa => sa.Amount)
                .HasColumnType("decimal(18,4)");

            modelBuilder.Entity<Employee>()
                .Property(sa => sa.BasicSalary)
                .HasColumnType("decimal(18,4)");

            modelBuilder.Entity<Salary>(entity => {
                entity.Property(sa => sa.NetSalary).HasColumnType("decimal(18,4)");
                entity.Property(sa => sa.Bonus).HasColumnType("decimal(18,4)");
                entity.Property(sa => sa.Penalty).HasColumnType("decimal(18,4)");
                entity.Property(sa => sa.NetSalary).HasColumnType("decimal(18,4)");
                entity.Property(sa => sa.BasicSalary).HasColumnType("decimal(18,4)");
                entity.Property(sa => sa.Allowance).HasColumnType("decimal(18,4)");
                entity.Property(sa => sa.SalaryAdvance).HasColumnType("decimal(18,4)");
            });
        }
    }
}
