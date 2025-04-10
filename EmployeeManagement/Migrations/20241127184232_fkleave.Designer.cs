﻿// <auto-generated />
using System;
using EmployeeManagement.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EmployeeManagement.Migrations
{
    [DbContext(typeof(EmployeeManagementContext))]
    [Migration("20241127184232_fkleave")]
    partial class fkleave
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EmployeeManagement.Models.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("created_at");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("password");

                    b.Property<int?>("Role")
                        .HasColumnType("int")
                        .HasColumnName("role");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("updated_at");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("username");

                    b.HasKey("Id");

                    b.ToTable("account");
                });

            modelBuilder.Entity("EmployeeManagement.Models.Allowance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AllowanceName")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("allowance_name");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,4)")
                        .HasColumnName("amount");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("created_at");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("description");

                    b.Property<int>("EmployeeID")
                        .HasColumnType("int")
                        .HasColumnName("employee_id");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("end_date");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("start_date");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.ToTable("allowance");
                });

            modelBuilder.Entity("EmployeeManagement.Models.Attendance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("created_at");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2")
                        .HasColumnName("date");

                    b.Property<int>("EmployeeID")
                        .HasColumnType("int")
                        .HasColumnName("employee_id");

                    b.Property<bool?>("IsAccepted")
                        .HasColumnType("bit")
                        .HasColumnName("is_accepted");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("notes");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeID");

                    b.ToTable("attendance");
                });

            modelBuilder.Entity("EmployeeManagement.Models.CandidateProfile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("address");

                    b.Property<DateTime>("ApplicationDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("application_date");

                    b.Property<string>("AppliedPosition")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("applied_position");

                    b.Property<string>("CandidateName")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("candidate_name");

                    b.Property<string>("CitizenID")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("citizen_id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("created_at");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2")
                        .HasColumnName("date_of_birth");

                    b.Property<string>("EducationLevel")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("education_level");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("email");

                    b.Property<string>("Experience")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("experience");

                    b.Property<bool>("ForeignLanguageProficiency")
                        .HasColumnType("bit")
                        .HasColumnName("foreign_language_proficiency");

                    b.Property<int?>("Gender")
                        .HasColumnType("int")
                        .HasColumnName("gender");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("notes");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("phone_number");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.ToTable("candidate_profile");
                });

            modelBuilder.Entity("EmployeeManagement.Models.Contract", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ContractContent")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("contract_content");

                    b.Property<string>("ContractType")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("contract_type");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("created_at");

                    b.Property<int>("EmployeeID")
                        .HasColumnType("int")
                        .HasColumnName("employee_id");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("end_date");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("notes");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("start_date");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeID");

                    b.ToTable("contract");
                });

            modelBuilder.Entity("EmployeeManagement.Models.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Abbreviation")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("abbreviation");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("created_at");

                    b.Property<string>("DepartmentName")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("department_name");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("description");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.ToTable("department");
                });

            modelBuilder.Entity("EmployeeManagement.Models.EducationLevel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Abbreviation")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("abbreviation");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("created_at");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("description");

                    b.Property<string>("EducationLevelName")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("education_name");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.ToTable("education_level");
                });

            modelBuilder.Entity("EmployeeManagement.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("address");

                    b.Property<string>("Avatar")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("avatar");

                    b.Property<decimal>("BasicSalary")
                        .HasColumnType("decimal(18,4)")
                        .HasColumnName("basic_salary");

                    b.Property<string>("CitizenID")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("citizen_id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("created_at");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2")
                        .HasColumnName("date_of_birth");

                    b.Property<int>("DepartmentID")
                        .HasColumnType("int")
                        .HasColumnName("department_id");

                    b.Property<int>("EducationLevelID")
                        .HasColumnType("int")
                        .HasColumnName("education_level_id");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("email");

                    b.Property<string>("EmployeeCode")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("employee_code");

                    b.Property<bool>("ForeignLanguageProficiency")
                        .HasColumnType("bit")
                        .HasColumnName("foreign_language_proficiency");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("gender");

                    b.Property<DateTime>("HireDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("hire_date");

                    b.Property<bool?>("IsActivated")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsAdmin")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("name");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("phone_number");

                    b.Property<int>("PositionID")
                        .HasColumnType("int")
                        .HasColumnName("position_id");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("updated_at");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentID");

                    b.HasIndex("EducationLevelID");

                    b.HasIndex("PositionID");

                    b.ToTable("employee");
                });

            modelBuilder.Entity("EmployeeManagement.Models.Insurance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("created_at");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("description");

                    b.Property<int>("EmployeeID")
                        .HasColumnType("int")
                        .HasColumnName("employee_id");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("end_date");

                    b.Property<string>("InsuranceName")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("insurance_name");

                    b.Property<int>("PremiumAmount")
                        .HasColumnType("int")
                        .HasColumnName("premium_amount");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("start_date");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.ToTable("insurance");
                });

            modelBuilder.Entity("EmployeeManagement.Models.Leave", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("created_at");

                    b.Property<DateTime?>("DateLeave")
                        .HasColumnType("datetime2")
                        .HasColumnName("date_leave");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int")
                        .HasColumnName("employee_id");

                    b.Property<string>("ReasonLeave")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("reason_leave");

                    b.Property<int>("Status")
                        .HasColumnType("int")
                        .HasColumnName("status");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.ToTable("leave");
                });

            modelBuilder.Entity("EmployeeManagement.Models.Penalize", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,4)")
                        .HasColumnName("amount");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("created_at");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("description");

                    b.Property<int>("EmployeeID")
                        .HasColumnType("int")
                        .HasColumnName("employee_id");

                    b.Property<DateTime>("PenalizeDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("penalize_date");

                    b.Property<string>("PenalizeName")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("penalize_name");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeID");

                    b.ToTable("penalize");
                });

            modelBuilder.Entity("EmployeeManagement.Models.Position", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Abbreviation")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("abbreviation");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("created_at");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("description");

                    b.Property<string>("PositionName")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("position_name");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.ToTable("position");
                });

            modelBuilder.Entity("EmployeeManagement.Models.Reward", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,4)")
                        .HasColumnName("amount");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("created_at");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("description");

                    b.Property<int>("EmployeeID")
                        .HasColumnType("int")
                        .HasColumnName("employee_id");

                    b.Property<DateTime>("RewardDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("reward_date");

                    b.Property<string>("RewardName")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("reward_name");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeID");

                    b.ToTable("reward");
                });

            modelBuilder.Entity("EmployeeManagement.Models.Salary", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Allowance")
                        .HasColumnType("decimal(18,4)")
                        .HasColumnName("allowance");

                    b.Property<decimal>("BasicSalary")
                        .HasColumnType("decimal(18,4)")
                        .HasColumnName("basic_salary");

                    b.Property<decimal>("Bonus")
                        .HasColumnType("decimal(18,4)")
                        .HasColumnName("bonus");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("created_at");

                    b.Property<int>("EmployeeID")
                        .HasColumnType("int")
                        .HasColumnName("employee_id");

                    b.Property<int>("Insurance")
                        .HasColumnType("int")
                        .HasColumnName("insurance");

                    b.Property<int>("Month")
                        .HasColumnType("int")
                        .HasColumnName("month");

                    b.Property<decimal>("NetSalary")
                        .HasColumnType("decimal(18,4)")
                        .HasColumnName("net_salary");

                    b.Property<decimal>("Penalty")
                        .HasColumnType("decimal(18,4)")
                        .HasColumnName("penalty");

                    b.Property<decimal>("SalaryAdvance")
                        .HasColumnType("decimal(18,4)")
                        .HasColumnName("salary_advance");

                    b.Property<int?>("TotalAttendance")
                        .HasColumnType("int")
                        .HasColumnName("total_attendance");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("updated_at");

                    b.Property<int>("Year")
                        .HasColumnType("int")
                        .HasColumnName("year");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeID");

                    b.ToTable("salary");
                });

            modelBuilder.Entity("EmployeeManagement.Models.SalaryAdvance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("AdvanceAmount")
                        .HasPrecision(18, 4)
                        .HasColumnType("decimal(18,4)")
                        .HasColumnName("advance_amount");

                    b.Property<DateTime>("AdvanceDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("advance_date");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("created_at");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("description");

                    b.Property<int>("EmployeeID")
                        .HasColumnType("int")
                        .HasColumnName("employee_id");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeID");

                    b.ToTable("salary_advance");
                });

            modelBuilder.Entity("EmployeeManagement.Models.Attendance", b =>
                {
                    b.HasOne("EmployeeManagement.Models.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("EmployeeManagement.Models.Contract", b =>
                {
                    b.HasOne("EmployeeManagement.Models.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("EmployeeManagement.Models.Employee", b =>
                {
                    b.HasOne("EmployeeManagement.Models.Department", "Department")
                        .WithMany("Employees")
                        .HasForeignKey("DepartmentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EmployeeManagement.Models.EducationLevel", "EducationLevel")
                        .WithMany("Employees")
                        .HasForeignKey("EducationLevelID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EmployeeManagement.Models.Position", "Position")
                        .WithMany("Employees")
                        .HasForeignKey("PositionID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");

                    b.Navigation("EducationLevel");

                    b.Navigation("Position");
                });

            modelBuilder.Entity("EmployeeManagement.Models.Leave", b =>
                {
                    b.HasOne("EmployeeManagement.Models.Employee", "Employee")
                        .WithMany("Leaves")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("EmployeeManagement.Models.Penalize", b =>
                {
                    b.HasOne("EmployeeManagement.Models.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("EmployeeManagement.Models.Reward", b =>
                {
                    b.HasOne("EmployeeManagement.Models.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("EmployeeManagement.Models.Salary", b =>
                {
                    b.HasOne("EmployeeManagement.Models.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("EmployeeManagement.Models.SalaryAdvance", b =>
                {
                    b.HasOne("EmployeeManagement.Models.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("EmployeeManagement.Models.Department", b =>
                {
                    b.Navigation("Employees");
                });

            modelBuilder.Entity("EmployeeManagement.Models.EducationLevel", b =>
                {
                    b.Navigation("Employees");
                });

            modelBuilder.Entity("EmployeeManagement.Models.Employee", b =>
                {
                    b.Navigation("Leaves");
                });

            modelBuilder.Entity("EmployeeManagement.Models.Position", b =>
                {
                    b.Navigation("Employees");
                });
#pragma warning restore 612, 618
        }
    }
}
