using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccessObjects.Migrations
{
    /// <inheritdoc />
    public partial class seedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Expertises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expertises", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FacilityTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacilityTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Specialties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fullname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Birthday = table.Column<DateOnly>(type: "date", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImgUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Facilities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OperationDay = table.Column<DateOnly>(type: "date", nullable: true),
                    Province = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ward = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImgUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Facilities_FacilityTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "FacilityTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImgUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Articles_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Articles_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Patients_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Professionals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    ExpertiseId = table.Column<int>(type: "int", nullable: true),
                    Province = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ward = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Degree = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Experience = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkingHours = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpertiseId1 = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Professionals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Professionals_Expertises_ExpertiseId",
                        column: x => x.ExpertiseId,
                        principalTable: "Expertises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Professionals_Expertises_ExpertiseId1",
                        column: x => x.ExpertiseId1,
                        principalTable: "Expertises",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Professionals_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FacilityDepartments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FacilityId = table.Column<int>(type: "int", nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacilityDepartments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FacilityDepartments_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FacilityDepartments_Facilities_FacilityId",
                        column: x => x.FacilityId,
                        principalTable: "Facilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PublicServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FacilityId = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PublicServices_Facilities_FacilityId",
                        column: x => x.FacilityId,
                        principalTable: "Facilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ArticleImage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImgUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticleImage_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProviderId = table.Column<int>(type: "int", nullable: true),
                    ProviderType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: true),
                    Rating = table.Column<int>(type: "int", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Review_Facility",
                        column: x => x.ProviderId,
                        principalTable: "Facilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reviews_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrivateServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfessionalId = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrivateServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrivateServices_Professionals_ProfessionalId",
                        column: x => x.ProfessionalId,
                        principalTable: "Professionals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProfessionalSpecialties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfessionalId = table.Column<int>(type: "int", nullable: true),
                    SpecialtyId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfessionalSpecialties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfessionalSpecialties_Professionals_ProfessionalId",
                        column: x => x.ProfessionalId,
                        principalTable: "Professionals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfessionalSpecialties_Specialties_SpecialtyId",
                        column: x => x.SpecialtyId,
                        principalTable: "Specialties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(type: "int", nullable: true),
                    ProviderId = table.Column<int>(type: "int", nullable: true),
                    ProviderType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: true),
                    ServiceType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appointment_Facility",
                        column: x => x.ProviderId,
                        principalTable: "Facilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Appointment_PrivateService",
                        column: x => x.ServiceId,
                        principalTable: "PrivateServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Appointment_Professional",
                        column: x => x.ProviderId,
                        principalTable: "Professionals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Appointment_PublicService",
                        column: x => x.ServiceId,
                        principalTable: "PublicServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Appointments_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Appointments_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "MedicalRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppointmentId = table.Column<int>(type: "int", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalRecords_Appointments_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Attachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicalRecordId = table.Column<int>(type: "int", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attachments_MedicalRecords_MedicalRecordId",
                        column: x => x.MedicalRecordId,
                        principalTable: "MedicalRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Các bài viết về các chủ đề y tế, chăm sóc sức khỏe, và bệnh lý.", "Y tế" },
                    { 2, "Các bài viết về dinh dưỡng, chế độ ăn uống và cách duy trì sức khỏe qua thực phẩm.", "Dinh dưỡng" },
                    { 3, "Các bài viết về các bệnh lý phổ biến, nguyên nhân và cách phòng ngừa.", "Bệnh lý" },
                    { 4, "Các bài viết về cách chăm sóc sức khỏe bản thân và gia đình.", "Chăm sóc sức khỏe" },
                    { 5, "Các bài viết về các loại phẫu thuật, quy trình và phục hồi sau phẫu thuật.", "Phẫu thuật" },
                    { 6, "Các bài viết về sức khỏe tinh thần, tâm lý học và cách duy trì tinh thần khỏe mạnh.", "Sức khỏe tâm lý" }
                });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Chuyên điều trị các bệnh lý nội khoa như tim mạch, tiêu hóa, thần kinh.", "Khoa Nội" },
                    { 2, "Chuyên phẫu thuật và điều trị các bệnh lý ngoại khoa.", "Khoa Ngoại" },
                    { 3, "Chuyên chăm sóc sức khỏe phụ nữ, mang thai, sinh nở và các vấn đề liên quan.", "Khoa Sản" },
                    { 4, "Chuyên điều trị các bệnh lý liên quan đến trẻ em và trẻ sơ sinh.", "Khoa Nhi" },
                    { 5, "Chuyên thực hiện các xét nghiệm chẩn đoán bệnh lý.", "Khoa Xét nghiệm" },
                    { 6, "Chuyên thực hiện các kỹ thuật hình ảnh như X-quang, MRI, CT scan.", "Khoa Chẩn đoán hình ảnh" },
                    { 7, "Chuyên điều trị các vấn đề về răng miệng và các bệnh lý liên quan.", "Khoa Răng Hàm Mặt" },
                    { 8, "Chuyên khám và điều trị các bệnh lý về mắt.", "Khoa Mắt" },
                    { 9, "Chuyên khám và điều trị các bệnh lý về tai, mũi, họng.", "Khoa Tai Mũi Họng" },
                    { 10, "Chuyên điều trị các bệnh lý về da và thẩm mỹ.", "Khoa Da Liễu" },
                    { 11, "Chuyên cấp cứu và điều trị các bệnh nhân trong tình trạng khẩn cấp.", "Khoa Cấp cứu" },
                    { 12, "Chuyên theo dõi và điều trị bệnh nhân trong tình trạng nguy kịch.", "Khoa Hồi sức tích cực" },
                    { 13, "Chuyên điều trị các vấn đề liên quan đến tâm lý, stress và trầm cảm.", "Khoa Tâm lý" },
                    { 14, "Chuyên phục hồi chức năng cho bệnh nhân sau tai nạn hoặc phẫu thuật.", "Khoa Phục hồi chức năng" },
                    { 15, "Chuyên điều trị các bệnh lý liên quan đến hệ tiết niệu và thận.", "Khoa Tiết niệu" },
                    { 16, "Chuyên điều trị các bệnh lý về tim và mạch máu.", "Khoa Tim mạch" },
                    { 17, "Chuyên điều trị các bệnh lý liên quan đến hệ hô hấp như phổi và khí quản.", "Khoa Hô hấp" },
                    { 18, "Chuyên điều trị các bệnh lý về nội tiết như tiểu đường, tuyến giáp.", "Khoa Nội tiết" },
                    { 19, "Chuyên điều trị các bệnh lý ung thư và các bệnh lý ác tính.", "Khoa Ung bướu" },
                    { 20, "Chuyên tư vấn và điều trị các vấn đề liên quan đến dinh dưỡng.", "Khoa Dinh dưỡng" }
                });

            migrationBuilder.InsertData(
                table: "Expertises",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Tốt nghiệp đại học Y khoa hệ chính quy (6 năm).", "Bác sĩ đa khoa" },
                    { 2, "Tốt nghiệp đại học Y học cổ truyền (6 năm).", "Bác sĩ y học cổ truyền" },
                    { 3, "Tốt nghiệp đại học chuyên khoa Răng - Hàm - Mặt (6 năm).", "Bác sĩ Răng - Hàm - Mặt" },
                    { 4, "Tốt nghiệp đại học chuyên ngành Y học dự phòng (6 năm).", "Bác sĩ Y học dự phòng" },
                    { 5, "Tốt nghiệp đại học ngành Dược (5 năm).", "Dược sĩ đại học" },
                    { 6, "Tốt nghiệp đại học ngành Điều dưỡng (4 năm).", "Cử nhân Điều dưỡng" },
                    { 7, "Đào tạo chuyên sâu 3 năm sau khi tốt nghiệp bác sĩ đa khoa.", "Bác sĩ nội trú" },
                    { 8, "Đào tạo sau đại học chuyên sâu trong lĩnh vực y khoa (2 năm).", "Bác sĩ chuyên khoa I" },
                    { 9, "Cấp cao hơn chuyên khoa I, đào tạo tiếp 2-3 năm.", "Bác sĩ chuyên khoa II" },
                    { 10, "Học vị thạc sĩ ngành y khoa (2 năm).", "Thạc sĩ Y khoa" },
                    { 11, "Học vị tiến sĩ y học, chuyên sâu nghiên cứu (3-5 năm).", "Tiến sĩ Y khoa" },
                    { 12, "Học hàm Phó Giáo sư, có nhiều nghiên cứu và đóng góp khoa học.", "Phó Giáo sư - Tiến sĩ" },
                    { 13, "Học hàm Giáo sư, chuyên gia đầu ngành y tế.", "Giáo sư - Tiến sĩ" }
                });

            migrationBuilder.InsertData(
                table: "FacilityTypes",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Bệnh viện do nhà nước sở hữu và quản lý, cung cấp dịch vụ y tế cho người dân với chi phí thấp hơn.", "Bệnh viện công" },
                    { 2, "Bệnh viện thuộc sở hữu của các cá nhân hoặc tổ chức tư nhân, cung cấp dịch vụ y tế với chất lượng cao và chi phí có thể cao hơn.", "Bệnh viện tư" },
                    { 3, "Cơ sở cung cấp dịch vụ y tế cơ bản và phòng ngừa cho cộng đồng.", "Trung tâm y tế" }
                });

            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "Id", "PaymentDate", "PaymentMethod", "PaymentStatus", "PaymentUrl", "Price", "TransactionId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 3, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), "VnPay", "Completed", "https://vnpay.vn/transaction/VNP13579246", 250000m, "VNP13579246" },
                    { 2, new DateTime(2025, 3, 18, 14, 15, 0, 0, DateTimeKind.Unspecified), "VnPay", "Completed", "https://vnpay.vn/transaction/VNP24681357", 60000m, "VNP24681357" },
                    { 3, new DateTime(2025, 3, 20, 9, 45, 0, 0, DateTimeKind.Unspecified), "VnPay", "Completed", "https://vnpay.vn/transaction/VNP98765432", 325000m, "VNP98765432" },
                    { 4, new DateTime(2025, 3, 22, 11, 0, 0, 0, DateTimeKind.Unspecified), "VnPay", "Completed", "https://vnpay.vn/transaction/VNP12345678", 200000m, "VNP12345678" },
                    { 5, new DateTime(2025, 3, 25, 16, 30, 0, 0, DateTimeKind.Unspecified), "VnPay", "Completed", "https://vnpay.vn/transaction/VNP87654321", 350000m, "VNP87654321" },
                    { 6, new DateTime(2025, 3, 29, 10, 45, 0, 0, DateTimeKind.Unspecified), "VnPay", "Pending", "https://vnpay.vn/transaction/VNP24680135", 100000m, "VNP24680135" },
                    { 7, new DateTime(2025, 3, 30, 15, 20, 0, 0, DateTimeKind.Unspecified), "VnPay", "Pending", "https://vnpay.vn/transaction/VNP13572468", 125000m, "VNP13572468" },
                    { 8, new DateTime(2025, 3, 28, 14, 10, 0, 0, DateTimeKind.Unspecified), "VnPay", "Failed", "https://vnpay.vn/transaction/VNP11223344", 250000m, "VNP11223344" },
                    { 9, new DateTime(2025, 3, 29, 9, 30, 0, 0, DateTimeKind.Unspecified), "VnPay", "Failed", "https://vnpay.vn/transaction/VNP99887766", 100000m, "VNP99887766" }
                });

            migrationBuilder.InsertData(
                table: "Specialties",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Chuyên ngành điều trị các bệnh lý nội bộ của cơ thể như bệnh tim mạch, tiêu hóa, hô hấp, thận.", "Chuyên khoa Nội" },
                    { 2, "Chuyên ngành liên quan đến phẫu thuật và điều trị các bệnh lý cần can thiệp phẫu thuật.", "Chuyên khoa Ngoại" },
                    { 3, "Chuyên ngành chuyên sâu về bệnh lý tim mạch, bao gồm các bệnh liên quan đến tim và mạch máu.", "Chuyên khoa Tim mạch" },
                    { 4, "Chuyên ngành chẩn đoán và điều trị các bệnh liên quan đến hệ thần kinh như đột quỵ, động kinh.", "Chuyên khoa Thần kinh" },
                    { 5, "Chuyên ngành chăm sóc và điều trị các bệnh lý về da như mụn, eczema, bệnh vảy nến.", "Chuyên khoa Da liễu" },
                    { 6, "Chuyên ngành điều trị các bệnh lý liên quan đến hệ sinh sản và chăm sóc sức khỏe phụ nữ.", "Chuyên khoa Sản phụ khoa" },
                    { 7, "Chuyên ngành chăm sóc sức khỏe và điều trị bệnh lý cho trẻ em.", "Chuyên khoa Nhi" },
                    { 8, "Chuyên ngành điều trị và quản lý các bệnh lý ung thư.", "Chuyên khoa Ung bướu" },
                    { 9, "Chuyên ngành điều trị và chăm sóc các bệnh lý về mắt, bao gồm đục thủy tinh thể, tật khúc xạ.", "Chuyên khoa Mắt" },
                    { 10, "Chuyên ngành liên quan đến các bệnh lý tai, mũi, họng và các cấu trúc liên quan.", "Chuyên khoa Tai Mũi Họng" },
                    { 11, "Chuyên ngành tập trung vào phục hồi sức khỏe cho bệnh nhân sau phẫu thuật, tai nạn, hoặc các bệnh lý nghiêm trọng.", "Chuyên khoa Phục hồi chức năng" },
                    { 12, "Chuyên ngành sử dụng các phương pháp y học cổ truyền như châm cứu, bấm huyệt để điều trị bệnh.", "Chuyên khoa Y học cổ truyền" },
                    { 13, "Chuyên ngành nghiên cứu và điều trị các bệnh lý về hô hấp như viêm phổi, hen suyễn.", "Chuyên khoa Hô hấp" },
                    { 14, "Chuyên ngành điều trị các bệnh lý liên quan đến nội tiết tố như tiểu đường, rối loạn tuyến giáp.", "Chuyên khoa Nội tiết" },
                    { 15, "Chuyên ngành chăm sóc sức khỏe răng miệng, bao gồm điều trị sâu răng, chỉnh hình răng miệng.", "Chuyên khoa Nha khoa" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Birthday", "Email", "Fullname", "Gender", "ImgUrl", "Password", "PhoneNumber", "Role", "Status" },
                values: new object[,]
                {
                    { 1, new DateOnly(1990, 1, 1), "admin@example.com", "Nguyễn Văn Admin", "Nam", "/images/users/admin_avatar.jpg", "ad123456", "0901234567", "Admin", "Active" },
                    { 2, new DateOnly(1995, 5, 20), "patient1@example.com", "Trần Thị Bích", "Nữ", "/images/users/patient_female_1.jpg", "pa123456", "0902345678", "Patient", "Active" },
                    { 3, new DateOnly(1988, 10, 12), "patient2@example.com", "Lê Văn Cường", "Nam", "/images/users/patient_male_1.jpg", "pa123456", "0903456789", "Patient", "Active" },
                    { 4, new DateOnly(1985, 3, 15), "professional1@example.com", "Phạm Minh Đức", "Nam", "/images/users/doctor_male_1.jpg", "pro123456", "0904567890", "Professional", "Active" },
                    { 5, new DateOnly(1987, 7, 30), "professional2@example.com", "Vũ Thị Hương", "Nữ", "/images/users/doctor_female_1.jpg", "pro123456", "0905678901", "Professional", "Active" },
                    { 6, new DateOnly(1992, 8, 25), "patient3@example.com", "Hoàng Thị Mai", "Nữ", "/images/users/patient_female_2.jpg", "pa123456", "0906789012", "Patient", "Active" },
                    { 7, new DateOnly(1998, 4, 17), "patient4@example.com", "Đỗ Quang Nam", "Nam", "/images/users/patient_male_2.jpg", "pa123456", "0907890123", "Patient", "Active" },
                    { 8, new DateOnly(1980, 11, 5), "professional3@example.com", "Ngô Thanh Tùng", "Nam", "/images/users/doctor_male_2.jpg", "pro123456", "0908901234", "Professional", "Active" },
                    { 9, new DateOnly(1983, 6, 10), "professional4@example.com", "Lý Thị Hoa", "Nữ", "/images/users/doctor_female_2.jpg", "pro123456", "0909012345", "Professional", "Active" },
                    { 10, new DateOnly(1990, 9, 15), "patient5@example.com", "Dương Văn Khải", "Nam", "/images/users/patient_male_3.jpg", "pa123456", "0910123456", "Patient", "Active" },
                    { 11, new DateOnly(1994, 2, 28), "patient6@example.com", "Trịnh Thu Phương", "Nữ", "/images/users/patient_female_3.jpg", "pa123456", "0911234567", "Patient", "Active" },
                    { 12, new DateOnly(1978, 4, 20), "professional5@example.com", "Bùi Quốc Anh", "Nam", "/images/users/doctor_male_3.jpg", "pro123456", "0912345678", "Professional", "Active" },
                    { 13, new DateOnly(1982, 5, 15), "lananh@example.com", "Nguyễn Thị Lan Anh", "Nữ", "/images/users/doctor_female_3.jpg", "pro123456", "0913456789", "Professional", "Active" },
                    { 14, new DateOnly(1970, 8, 22), "tranminh@example.com", "Trần Văn Minh", "Nam", "/images/users/doctor_male_4.jpg", "pro123456", "0912345678", "Professional", "Active" },
                    { 15, new DateOnly(1990, 3, 10), "thanhhuong@example.com", "Phan Thị Thanh Hương", "Nữ", "/images/users/doctor_female_4.jpg", "pro123456", "0923456789", "Professional", "Active" },
                    { 16, new DateOnly(1988, 11, 5), "mylinh@example.com", "Ngô Thị Mỹ Linh", "Nữ", "/images/users/doctor_female_5.jpg", "pro123456", "0934567890", "Professional", "Active" },
                    { 17, new DateOnly(1985, 4, 18), "quoctuan@example.com", "Đặng Quốc Tuấn", "Nam", "/images/users/doctor_male_5.jpg", "pro123456", "0945678901", "Professional", "Active" },
                    { 18, new DateOnly(1981, 6, 25), "kimngan@example.com", "Lê Thị Kim Ngân", "Nữ", "/images/users/doctor_female_6.jpg", "pro123456", "0956789012", "Professional", "Active" },
                    { 19, new DateOnly(1965, 10, 15), "ducthinh@example.com", "Hoàng Đức Thịnh", "Nam", "/images/users/doctor_male_6.jpg", "pro123456", "0967890123", "Professional", "Active" },
                    { 20, new DateOnly(1989, 2, 8), "minhquan@example.com", "Vũ Minh Quân", "Nam", "/images/users/doctor_male_7.jpg", "pro123456", "0978901234", "Professional", "Active" },
                    { 21, new DateOnly(1986, 7, 12), "minhhai@example.com", "Trịnh Minh Hải", "Nam", "/images/users/doctor_male_8.jpg", "pro123456", "0989012345", "Professional", "Active" },
                    { 22, new DateOnly(1982, 9, 28), "thuytrang@example.com", "Phạm Thị Thùy Trang", "Nữ", "/images/users/doctor_female_7.jpg", "pro123456", "0990123456", "Professional", "Active" },
                    { 23, new DateOnly(1979, 12, 5), "vanhung@example.com", "Bùi Văn Hưng", "Nam", "/images/users/doctor_male_9.jpg", "pro123456", "0901234567", "Professional", "Active" },
                    { 24, new DateOnly(1984, 5, 15), "bichngoc@example.com", "Nguyễn Thị Bích Ngọc", "Nữ", "/images/users/doctor_female_8.jpg", "pro123456", "0912345678", "Professional", "Active" },
                    { 25, new DateOnly(1978, 8, 20), "quangvinh@example.com", "Trương Quang Vinh", "Nam", "/images/users/doctor_male_10.jpg", "pro123456", "0923456789", "Professional", "Active" },
                    { 26, new DateOnly(1987, 3, 12), "duongha@example.com", "Dương Thị Hà", "Nữ", "/images/users/doctor_female_9.jpg", "pro123456", "0934567890", "Professional", "Active" },
                    { 27, new DateOnly(1991, 10, 8), "vanthang@example.com", "Mai Văn Thắng", "Nam", "/images/users/doctor_male_11.jpg", "pro123456", "0945678901", "Professional", "Inactive" }
                });

            migrationBuilder.InsertData(
                table: "Articles",
                columns: new[] { "Id", "CategoryId", "Content", "CreatedAt", "CreatedById", "ImgUrl", "Title" },
                values: new object[,]
                {
                    { 1, 1, "<p>Kiểm tra sức khỏe định kỳ là một trong những phương pháp quan trọng giúp phát hiện sớm các vấn đề sức khỏe tiềm ẩn. Việc kiểm tra thường xuyên không chỉ giúp bạn hiểu rõ hơn về tình trạng sức khỏe của mình mà còn giúp bác sĩ đưa ra các biện pháp điều trị kịp thời nếu phát hiện ra vấn đề.</p><p><strong>Các lợi ích chính của việc kiểm tra sức khỏe định kỳ bao gồm:</strong></p><ol><li><strong>Phát hiện sớm các bệnh lý tiềm ẩn</strong>: Việc kiểm tra sức khỏe giúp phát hiện sớm các bệnh lý như tiểu đường, huyết áp cao, hay các vấn đề tim mạch mà bạn có thể không nhận ra. Việc phát hiện sớm giúp điều trị hiệu quả hơn, giảm thiểu các biến chứng nghiêm trọng về sau.</li><li><strong>Tiết kiệm chi phí điều trị</strong>: Việc phát hiện bệnh sớm sẽ giúp bạn tiết kiệm chi phí điều trị, bởi vì bệnh sẽ dễ dàng được điều trị hơn khi phát hiện ở giai đoạn đầu. Điều này không chỉ giúp giảm chi phí cá nhân mà còn giúp hệ thống y tế giảm gánh nặng.</li><li><strong>Tăng tuổi thọ</strong>: Các kiểm tra định kỳ giúp phát hiện sớm các yếu tố nguy cơ sức khỏe và điều chỉnh kịp thời, từ đó tăng khả năng sống lâu. Ví dụ, việc kiểm soát mức huyết áp hoặc cholesterol có thể giúp giảm nguy cơ đột quỵ và các bệnh tim mạch.</li><li><strong>Cải thiện chất lượng cuộc sống</strong>: Việc kiểm tra sức khỏe sẽ giúp bạn có lối sống lành mạnh hơn, với chế độ ăn uống và tập thể dục phù hợp. Điều này sẽ giúp bạn có nhiều năng lượng hơn và cảm thấy tự tin vào sức khỏe của mình.</li><li><strong>Giảm căng thẳng và lo âu</strong>: Khi bạn biết rằng sức khỏe của mình ổn định, bạn sẽ cảm thấy an tâm và ít lo lắng hơn. Việc biết rằng bạn không mắc bệnh gì nghiêm trọng giúp bạn giảm bớt lo âu, từ đó cải thiện tâm trạng và chất lượng cuộc sống.</li></ol>", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "/images/articles/health-checkup.jpg", "5 Lợi Ích Của Việc Kiểm Tra Sức Khỏe Định Kỳ" },
                    { 2, 1, "<p>Tiêm chủng là một trong những phương pháp phòng ngừa bệnh hiệu quả nhất mà chúng ta có. Việc tiêm chủng định kỳ giúp cơ thể tạo ra miễn dịch đối với các bệnh truyền nhiễm, bảo vệ không chỉ cho bản thân mà còn cho cộng đồng. Dưới đây là những lý do tại sao tiêm chủng là quan trọng và cần thiết:</p><p><strong>1. Bảo vệ khỏi bệnh truyền nhiễm</strong>: Tiêm chủng giúp cơ thể chống lại các bệnh như sởi, thủy đậu, viêm gan B, bạch hầu và nhiều bệnh khác. Bằng cách tiêm phòng, bạn giảm nguy cơ mắc phải các bệnh này, giúp bạn bảo vệ sức khỏe của mình và những người xung quanh. Các bệnh truyền nhiễm có thể gây hậu quả nghiêm trọng, thậm chí là tử vong, nhưng có thể phòng ngừa dễ dàng nhờ tiêm chủng.</p><p><strong>2. Bảo vệ cộng đồng</strong>: Tiêm chủng không chỉ giúp bảo vệ bản thân mà còn giúp bảo vệ những người xung quanh, đặc biệt là những người không thể tiêm chủng như trẻ em, phụ nữ mang thai hoặc người có hệ miễn dịch yếu. Khi càng nhiều người trong cộng đồng tiêm phòng, khả năng lây lan của bệnh sẽ giảm thiểu, từ đó bảo vệ cả cộng đồng khỏi sự bùng phát của các dịch bệnh.</p><p><strong>3. Ngăn ngừa dịch bệnh</strong>: Khi đủ nhiều người trong cộng đồng được tiêm phòng, các dịch bệnh sẽ không có cơ hội bùng phát, giúp bảo vệ cả cộng đồng khỏi những đợt dịch nguy hiểm. Điều này đã được chứng minh qua nhiều quốc gia trên thế giới khi tiêm chủng giúp ngăn ngừa sự bùng phát của các bệnh như sởi, bại liệt, và cúm.</p><p><strong>4. Giảm chi phí chăm sóc sức khỏe</strong>: Khi tiêm chủng, bạn giảm nguy cơ mắc bệnh, từ đó giảm chi phí điều trị và chăm sóc y tế lâu dài. Các bệnh do không tiêm phòng có thể tốn kém hơn rất nhiều trong việc điều trị và chăm sóc sau này.</p><p><strong>5. Đảm bảo an toàn cho trẻ em</strong>: Việc tiêm chủng cho trẻ em giúp bảo vệ các em khỏi những bệnh tật nguy hiểm và giảm tỷ lệ tử vong do bệnh truyền nhiễm. Trẻ em có hệ miễn dịch yếu, nên việc tiêm chủng là biện pháp cần thiết để bảo vệ các em khỏi các mối đe dọa bệnh tật.</p>", new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "/images/articles/vaccination.jpg", "Tầm Quan Trọng Của Việc Tiêm Chủng Định Kỳ" },
                    { 3, 2, "<p>Một chế độ ăn uống cân bằng là nền tảng quan trọng để duy trì sức khỏe. Để có một chế độ ăn uống lành mạnh, bạn cần đảm bảo rằng cơ thể nhận được đủ các nhóm chất dinh dưỡng cần thiết. Sau đây là một số lời khuyên để duy trì chế độ ăn uống cân bằng và hợp lý:</p><p><strong>1. Ăn đủ 5 nhóm thực phẩm</strong>: Đảm bảo rằng mỗi bữa ăn của bạn bao gồm đủ các nhóm thực phẩm như tinh bột (gạo, khoai), protein (thịt, cá, đậu), chất béo lành mạnh (dầu olive, bơ), vitamin và khoáng chất (rau xanh, trái cây), và chất xơ. Việc kết hợp đa dạng các thực phẩm sẽ cung cấp đầy đủ dinh dưỡng cho cơ thể.</p><p><strong>2. Hạn chế thức ăn chế biến sẵn</strong>: Thực phẩm chế biến sẵn chứa nhiều chất bảo quản, đường và muối, có thể gây hại cho sức khỏe nếu tiêu thụ quá nhiều. Hãy tránh thức ăn nhanh, thực phẩm chiên rán, và thay vào đó là các món ăn tươi sống, chế biến tại nhà.</p><p><strong>3. Uống đủ nước</strong>: Cung cấp đủ nước cho cơ thể là một yếu tố quan trọng trong chế độ ăn uống. Nước giúp cơ thể hấp thu chất dinh dưỡng, giải độc và duy trì nhiệt độ cơ thể ổn định. Bạn nên uống ít nhất 8 cốc nước mỗi ngày và uống thêm nếu bạn tham gia các hoạt động thể thao.</p><p><strong>4. Ăn nhiều rau củ quả</strong>: Rau củ quả chứa nhiều vitamin, khoáng chất và chất xơ, giúp hỗ trợ hệ tiêu hóa, tăng cường hệ miễn dịch và giúp da khỏe mạnh. Hãy cố gắng ăn ít nhất 5 khẩu phần rau quả mỗi ngày để cung cấp các dưỡng chất thiết yếu cho cơ thể.</p><p><strong>5. Kiểm soát lượng đường và muối</strong>: Việc giảm lượng đường và muối trong chế độ ăn uống có thể giúp ngăn ngừa các bệnh lý như tiểu đường, huyết áp cao và bệnh tim. Bạn nên hạn chế các thực phẩm ngọt và thức uống có gas, thay vào đó là ăn trái cây tươi và sử dụng gia vị tự nhiên.</p>", new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "/images/articles/balanced-diet.jpg", "Chế Độ Ăn Uống Cân Bằng Cho Sức Khỏe" },
                    { 4, 2, "<p>Hệ tiêu hóa đóng một vai trò quan trọng trong việc duy trì sức khỏe. Khi hệ tiêu hóa hoạt động tốt, cơ thể sẽ hấp thụ dinh dưỡng hiệu quả, giảm nguy cơ mắc các bệnh lý và cải thiện chất lượng cuộc sống. Dưới đây là một số thực phẩm giúp cải thiện hệ tiêu hóa:</p><p><strong>1. Sữa chua</strong>: Sữa chua chứa các vi khuẩn có lợi giúp duy trì sự cân bằng của hệ vi sinh đường ruột, từ đó giúp hệ tiêu hóa hoạt động hiệu quả hơn. Các lợi khuẩn này giúp cải thiện sự hấp thu chất dinh dưỡng và tăng cường hệ miễn dịch.</p><p><strong>2. Chuối</strong>: Chuối là một nguồn cung cấp chất xơ tuyệt vời, giúp cải thiện nhu động ruột và ngăn ngừa táo bón. Chuối cũng có thể làm dịu dạ dày và giúp giảm cảm giác đầy bụng.</p><p><strong>3. Rau xanh</strong>: Các loại rau như rau cải, rau bina và bông cải xanh chứa nhiều chất xơ và vitamin, giúp tăng cường chức năng tiêu hóa và làm sạch đường ruột. Rau xanh giúp cải thiện nhu động ruột và giảm nguy cơ mắc bệnh về đường tiêu hóa.</p><p><strong>4. Hạt chia</strong>: Hạt chia giàu chất xơ, giúp cải thiện nhu động ruột và giảm táo bón. Ngoài ra, hạt chia còn cung cấp các axit béo omega-3 có lợi cho sức khỏe.</p><p><strong>5. Gừng</strong>: Gừng có tính kháng viêm và có thể giúp làm dịu dạ dày, hỗ trợ tiêu hóa và giảm đầy bụng. Uống trà gừng hoặc thêm gừng tươi vào các món ăn có thể giúp cải thiện hệ tiêu hóa.</p>", new DateTime(2025, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "/images/articles/digestive-health.jpg", "Thực Phẩm Giúp Cải Thiện Hệ Tiêu Hóa" },
                    { 5, 1, "<p>Bệnh tim mạch là một trong những nguyên nhân hàng đầu gây tử vong trên toàn cầu. Tuy nhiên, bệnh tim mạch có thể được phòng ngừa thông qua các biện pháp thay đổi lối sống lành mạnh. Dưới đây là những phương pháp phòng ngừa hiệu quả bệnh tim mạch:</p><p><strong>1. Duy trì một chế độ ăn uống lành mạnh</strong>: Chế độ ăn uống giàu trái cây, rau củ, ngũ cốc nguyên hạt, và giảm thiểu các thực phẩm giàu chất béo bão hòa và cholesterol sẽ giúp bảo vệ tim mạch. Hãy bổ sung các thực phẩm giàu omega-3 như cá hồi và các loại hạt giúp làm giảm nguy cơ bệnh tim.</p><p><strong>2. Tập thể dục thường xuyên</strong>: Các nghiên cứu đã chứng minh rằng việc tập thể dục thường xuyên, ít nhất 30 phút mỗi ngày, giúp cải thiện sức khỏe tim mạch. Việc này giúp tăng cường lưu thông máu, kiểm soát huyết áp và cholesterol.</p><p><strong>3. Kiểm soát cân nặng</strong>: Thừa cân làm tăng nguy cơ mắc bệnh tim mạch. Việc duy trì cân nặng hợp lý thông qua chế độ ăn uống và luyện tập sẽ giảm thiểu gánh nặng cho tim, giúp tim hoạt động hiệu quả hơn.</p><p><strong>4. Hạn chế căng thẳng</strong>: Căng thẳng kéo dài có thể làm tăng huyết áp và làm tổn thương mạch máu. Hãy áp dụng các phương pháp giảm stress như thiền, yoga, hoặc đi bộ để giảm mức độ căng thẳng trong cuộc sống hàng ngày.</p><p><strong>5. Kiểm tra sức khỏe định kỳ</strong>: Việc kiểm tra sức khỏe định kỳ, bao gồm kiểm tra huyết áp và mức cholesterol, sẽ giúp bạn phát hiện sớm các yếu tố nguy cơ và có biện pháp can thiệp kịp thời để bảo vệ sức khỏe tim mạch.</p>", new DateTime(2025, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "/images/articles/heart-health.jpg", "Cách Phòng Ngừa Bệnh Tim Mạch" },
                    { 6, 3, "<p>Ung thư phổi là một trong những loại ung thư nguy hiểm và có tỷ lệ tử vong cao. Việc phát hiện bệnh sớm sẽ giúp điều trị hiệu quả và cải thiện cơ hội sống sót. Dưới đây là một số dấu hiệu cảnh báo ung thư phổi mà bạn không nên bỏ qua:</p><p><strong>1. Ho kéo dài</strong>: Ho kéo dài, đặc biệt là ho có đờm hoặc ho ra máu, có thể là dấu hiệu của ung thư phổi. Nếu bạn có ho liên tục trong nhiều tuần, hãy đi kiểm tra để xác định nguyên nhân.</p><p><strong>2. Khó thở</strong>: Khó thở hoặc cảm giác hụt hơi khi làm những việc bình thường có thể là triệu chứng của bệnh ung thư phổi. Sự tắc nghẽn trong phổi do khối u có thể làm giảm khả năng hô hấp của bạn.</p><p><strong>3. Đau ngực</strong>: Đau hoặc cảm giác tức ngực, đặc biệt là khi ho hoặc thở sâu, có thể là dấu hiệu của ung thư phổi. Cơn đau có thể lan ra vai hoặc lưng, đặc biệt khi khối u chèn ép lên các cơ quan lân cận.</p><p><strong>4. Giảm cân không rõ lý do</strong>: Giảm cân đột ngột mà không thay đổi chế độ ăn uống hoặc lối sống có thể là một dấu hiệu của ung thư phổi. Đây là triệu chứng chung của nhiều loại ung thư, trong đó có ung thư phổi.</p><p><strong>5. Mệt mỏi kéo dài</strong>: Cảm giác mệt mỏi và yếu ớt kéo dài có thể là dấu hiệu của ung thư phổi. Khi các tế bào ung thư phát triển, cơ thể sẽ trở nên mệt mỏi hơn, và năng lượng của bạn sẽ giảm sút.</p><p>Việc kiểm tra y tế kịp thời sẽ giúp phát hiện ung thư phổi ở giai đoạn sớm, từ đó tăng cơ hội điều trị và cải thiện khả năng sống sót của bệnh nhân.</p>", new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "/images/articles/lung-cancer.jpg", "Những Dấu Hiệu Cảnh Báo Ung Thư Phổi" }
                });

            migrationBuilder.InsertData(
                table: "Facilities",
                columns: new[] { "Id", "Address", "Description", "District", "ImgUrl", "Name", "OperationDay", "Province", "Status", "TypeId", "Ward" },
                values: new object[,]
                {
                    { 1, "78 Đường Giải Phóng", "Một trong những bệnh viện đa khoa lớn nhất Việt Nam, cung cấp dịch vụ y tế chất lượng cao với đội ngũ y bác sĩ hàng đầu.", "Quận Đống Đa", "/images/facilities/bach-mai.jpg", "Bệnh viện Bạch Mai", new DateOnly(2020, 5, 15), "Thành phố Hà Nội", "Active", 1, "Phường Phương Mai" },
                    { 2, "201 Nguyễn Chí Thanh", "Bệnh viện hạng đặc biệt tại miền Nam, với trang thiết bị hiện đại và đội ngũ y bác sĩ giỏi chuyên môn.", "Quận 5", "/images/facilities/cho-ray.jpg", "Bệnh viện Chợ Rẫy", new DateOnly(2019, 7, 20), "Thành phố Hồ Chí Minh", "Active", 1, "Phường 12" },
                    { 3, "458 Minh Khai", "Bệnh viện tư nhân đẳng cấp quốc tế với cơ sở vật chất và trang thiết bị hiện đại hàng đầu Việt Nam.", "Quận Hai Bà Trưng", "/images/facilities/vinmec.jpg", "Bệnh viện Đa khoa Quốc tế Vinmec Times City", new DateOnly(2022, 3, 10), "Thành phố Hà Nội", "Active", 2, "Phường Vĩnh Tuy" },
                    { 4, "16 Lê Lợi", "Bệnh viện đa khoa hạng đặc biệt tại miền Trung, cung cấp dịch vụ y tế chất lượng cao cho khu vực.", "Thành phố Huế", "/images/facilities/hue-central.jpg", "Bệnh viện Trung ương Huế", new DateOnly(2021, 8, 5), "Thừa Thiên Huế", "Active", 1, "Phường Phước Vĩnh" },
                    { 5, "315 Nguyễn Văn Cừ", "Bệnh viện đa khoa hạng đặc biệt tại miền Tây Nam Bộ, cung cấp dịch vụ y tế cho khu vực Đồng bằng sông Cửu Long.", "Quận Ninh Kiều", "/images/facilities/cantho-central.jpg", "Bệnh viện Đa khoa Trung ương Cần Thơ", new DateOnly(2020, 10, 17), "Thành phố Cần Thơ", "Active", 1, "Phường Tân An" },
                    { 6, "30 Cầu Bươu", "Bệnh viện chuyên khoa ung bướu hàng đầu, cung cấp dịch vụ chẩn đoán và điều trị ung thư.", "Huyện Thanh Trì", "/images/facilities/k-hospital.jpg", "Bệnh viện K Tân Triều", new DateOnly(2022, 1, 20), "Thành phố Hà Nội", "Active", 1, "Xã Tân Triều" },
                    { 7, "85 Bà Triệu", "Bệnh viện chuyên khoa mắt hàng đầu, cung cấp dịch vụ chẩn đoán và điều trị các bệnh lý về mắt.", "Quận Đống Đa", "/images/facilities/eye-hospital.jpg", "Bệnh viện Mắt Trung ương", new DateOnly(2021, 5, 5), "Thành phố Hà Nội", "Active", 1, "Phường Trung Liệt" },
                    { 8, "6 Nguyễn Lương Bằng", "Bệnh viện quốc tế với đội ngũ bác sĩ trong nước và quốc tế, cung cấp dịch vụ y tế chất lượng cao.", "Quận 7", "/images/facilities/fv-hospital.jpg", "Bệnh viện FV", new DateOnly(2023, 2, 15), "Thành phố Hồ Chí Minh", "Active", 2, "Phường Tân Phú" },
                    { 9, "291 Nguyễn Văn Linh", "Bệnh viện tư nhân hiện đại tại miền Trung, cung cấp dịch vụ y tế chất lượng cao.", "Quận Hải Châu", "/images/facilities/hoan-my.jpg", "Bệnh viện Đa khoa Hoàn Mỹ Đà Nẵng", new DateOnly(2022, 6, 10), "Thành phố Đà Nẵng", "Active", 2, "Phường Thạch Thang" },
                    { 10, "84 Đường Vành Đai 4", "Trung tâm y tế cung cấp dịch vụ khám chữa bệnh cho người dân địa phương với chi phí hợp lý.", "Huyện Bình Chánh", "/images/facilities/binh-chanh.jpg", "Trung tâm Y tế huyện Bình Chánh", new DateOnly(2021, 9, 8), "Thành phố Hồ Chí Minh", "Active", 3, "Thị trấn Tân Túc" },
                    { 11, "30 Đường Đỗ Đức Dục", "Trung tâm y tế cung cấp dịch vụ chăm sóc sức khỏe ban đầu và phòng ngừa dịch bệnh cho cộng đồng.", "Quận Nam Từ Liêm", "/images/facilities/nam-tu-liem.jpg", "Trung tâm Y tế quận Nam Từ Liêm", new DateOnly(2022, 4, 12), "Thành phố Hà Nội", "Active", 3, "Phường Mỹ Đình" },
                    { 12, "638 Đường Trần Hưng Đạo", "Bệnh viện đa khoa hạng I cung cấp dịch vụ y tế cho người dân tỉnh Lào Cai và các tỉnh lân cận.", "Thành phố Lào Cai", "/images/facilities/laocai.jpg", "Bệnh viện Đa khoa tỉnh Lào Cai", new DateOnly(2021, 11, 20), "Tỉnh Lào Cai", "Active", 1, "Phường Nam Cường" },
                    { 13, "225 Đường Nguyễn Lương Bằng", "Bệnh viện đa khoa hạng I cung cấp dịch vụ y tế chất lượng cho người dân trong tỉnh.", "Thành phố Hải Dương", "/images/facilities/hai-duong.jpg", "Bệnh viện Đa khoa tỉnh Hải Dương", new DateOnly(2020, 12, 15), "Tỉnh Hải Dương", "Active", 1, "Phường Trần Phú" },
                    { 14, "Đường Phan Châu Trinh", "Bệnh viện đa khoa tuyến tỉnh, cung cấp dịch vụ khám chữa bệnh cho người dân trong tỉnh Quảng Nam.", "Thành phố Tam Kỳ", "/images/facilities/quang-nam.jpg", "Bệnh viện Đa khoa tỉnh Quảng Nam", new DateOnly(2022, 2, 8), "Tỉnh Quảng Nam", "Active", 1, "Phường An Xuân" },
                    { 15, "52 Trần Phú", "Bệnh viện tư nhân cung cấp dịch vụ y tế chất lượng cao cho người dân và du khách tại Nha Trang.", "Thành phố Nha Trang", "/images/facilities/nhatrang.jpg", "Bệnh viện Đa khoa Quốc tế Nha Trang", new DateOnly(2023, 1, 10), "Tỉnh Khánh Hòa", "Active", 2, "Phường Vĩnh Hải" },
                    { 16, "144 Đường Nguyễn Thị Minh Khai", "Bệnh viện đa khoa tuyến tỉnh, cung cấp dịch vụ y tế cho người dân tỉnh Đồng Tháp.", "Thành phố Cao Lãnh", "/images/facilities/dong-thap.jpg", "Bệnh viện Đa khoa tỉnh Đồng Tháp", new DateOnly(2021, 7, 8), "Tỉnh Đồng Tháp", "Active", 1, "Phường 1" },
                    { 17, "70 Đường 30/4", "Bệnh viện tư nhân chất lượng cao chuyên về sản phụ khoa và nhi khoa tại khu vực Đồng bằng sông Cửu Long.", "Quận Ninh Kiều", "/images/facilities/phuong-chau.jpg", "Bệnh viện Đa khoa Phương Châu", new DateOnly(2022, 9, 15), "Thành phố Cần Thơ", "Active", 2, "Phường An Bình" },
                    { 18, "17B Phù Đổng Thiên Vương", "Bệnh viện đa khoa tuyến tỉnh, cung cấp dịch vụ y tế cho người dân Lâm Đồng và các tỉnh lân cận.", "Thành phố Đà Lạt", "/images/facilities/lam-dong.jpg", "Bệnh viện Đa khoa tỉnh Lâm Đồng", new DateOnly(2021, 3, 25), "Tỉnh Lâm Đồng", "Active", 1, "Phường 8" },
                    { 19, "Tổ 3", "Trung tâm y tế cung cấp dịch vụ chăm sóc sức khỏe cơ bản cho đồng bào dân tộc vùng cao.", "Huyện Mèo Vạc", "/images/facilities/meovac.jpg", "Trung tâm Y tế huyện Mèo Vạc", new DateOnly(2022, 8, 5), "Tỉnh Hà Giang", "Inactive", 3, "Thị trấn Mèo Vạc" },
                    { 20, "24 Đường Số 4", "Bệnh viện tư nhân mới thành lập, trang bị hiện đại và cung cấp dịch vụ y tế chất lượng cao.", "Quận Bình Tân", "/images/facilities/city-hospital.jpg", "Bệnh viện Quốc tế City", new DateOnly(2023, 3, 1), "Thành phố Hồ Chí Minh", "Active", 2, "Phường Bình Trị Đông" }
                });

            migrationBuilder.InsertData(
                table: "Patients",
                columns: new[] { "Id", "Note", "UserId" },
                values: new object[,]
                {
                    { 1, "Tôi có tiền sử bệnh tim mạch, đã từng phẫu thuật van tim năm 2020", 2 },
                    { 2, "Tôi bị tiểu đường type 2 và huyết áp cao, đang điều trị thuốc ổn định", 3 },
                    { 3, "Tôi có tiền sử viêm gan B, đang theo dõi định kỳ", 6 },
                    { 4, "Tôi bị hen suyễn từ nhỏ, có dị ứng với bụi và phấn hoa", 7 },
                    { 5, "Bệnh nhân bị thoái hóa cột sống, đang điều trị vật lý trị liệu", 10 },
                    { 6, "Tôi có tiền sử sỏi thận, đã phẫu thuật lấy sỏi năm 2022", 11 }
                });

            migrationBuilder.InsertData(
                table: "Professionals",
                columns: new[] { "Id", "Address", "Degree", "District", "Experience", "ExpertiseId", "ExpertiseId1", "Province", "RequestStatus", "UserId", "Ward", "WorkingHours" },
                values: new object[,]
                {
                    { 1, "Số 15, Phố Hàng Bông", "Chuyên khoa I Nội khoa", "Quận Hoàn Kiếm", "Có 12 năm kinh nghiệm trong lĩnh vực khám chữa bệnh nội khoa", 8, null, "Thành phố Hà Nội", "Approved", 4, "Phường Hàng Bạc", "8:00 - 17:00" },
                    { 2, "Số 25, Đường Nguyễn Huệ", "Đại học Y học cổ truyền", "Quận 1", "Có 8 năm kinh nghiệm trong điều trị các bệnh lý bằng y học cổ truyền", 2, null, "Thành phố Hồ Chí Minh", "Approved", 5, "Phường Bến Nghé", "9:00 - 18:00" },
                    { 3, "Số 42, Đường Trần Phú", "Đại học Răng Hàm Mặt", "Quận Hải Châu", "Có 15 năm kinh nghiệm trong lĩnh vực nha khoa và phẫu thuật hàm mặt", 3, null, "Thành phố Đà Nẵng", "Approved", 8, "Phường Thạch Thang", "8:30 - 17:30" },
                    { 4, "Số 28, Đường Hòa Bình", "Chuyên khoa II Tim mạch", "Quận Ninh Kiều", "Có 9 năm kinh nghiệm trong lĩnh vực tim mạch và nội khoa", 9, null, "Thành phố Cần Thơ", "Approved", 9, "Phường Tân An", "7:00 - 16:00" },
                    { 5, "Số 55, Đường Ngô Gia Tự", "Tiến sĩ Y khoa chuyên ngành Ngoại khoa", "Thành phố Bắc Ninh", "Có 18 năm kinh nghiệm trong lĩnh vực ngoại khoa và phẫu thuật tổng quát", 11, null, "Tỉnh Bắc Ninh", "Approved", 12, "Phường Đại Phúc", "7:30 - 16:30" },
                    { 6, "Số 105, Đường Xuân Thủy", "Thạc sĩ Y khoa chuyên ngành Sản phụ khoa", "Quận Cầu Giấy", "Có 11 năm kinh nghiệm trong lĩnh vực sản khoa và phụ khoa", 10, null, "Thành phố Hà Nội", "Approved", 13, "Phường Dịch Vọng", "8:00 - 17:00" },
                    { 7, "Số 215, Đường Hồng Bàng", "Phó Giáo sư - Tiến sĩ Y khoa", "Quận 5", "Có 25 năm kinh nghiệm trong lĩnh vực thần kinh và nghiên cứu khoa học", 12, null, "Thành phố Hồ Chí Minh", "Approved", 14, "Phường 5", "9:00 - 16:00" },
                    { 8, "Số 38, Đường Trần Phú", "Đại học Y khoa", "Thành phố Nha Trang", "Có 5 năm kinh nghiệm trong lĩnh vực khám chữa bệnh tổng quát", 1, null, "Tỉnh Khánh Hòa", "Approved", 15, "Phường Lộc Thọ", "7:30 - 17:00" },
                    { 9, "Số 77, Đường Lê Hồng Phong", "Cử nhân Điều dưỡng", "Thành phố Thủ Dầu Một", "Có 7 năm kinh nghiệm trong chăm sóc và điều dưỡng bệnh nhân", 6, null, "Tỉnh Bình Dương", "Approved", 16, "Phường Phú Cường", "7:00 - 19:00" },
                    { 10, "Số 12, Đường Lạch Tray", "Dược sĩ đại học", "Quận Hồng Bàng", "Có 10 năm kinh nghiệm trong lĩnh vực dược phẩm và tư vấn thuốc", 5, null, "Thành phố Hải Phòng", "Approved", 17, "Phường Hoàng Văn Thụ", "8:00 - 18:00" },
                    { 11, "Số 65, Đường Nguyễn Huệ", "Đại học Y học dự phòng", "Thành phố Huế", "Có 14 năm kinh nghiệm trong lĩnh vực y học dự phòng và kiểm soát dịch bệnh", 4, null, "Tỉnh Thừa Thiên Huế", "Approved", 18, "Phường Phú Nhuận", "7:30 - 16:30" },
                    { 12, "Số 43, Đường Liễu Giai", "Giáo sư - Tiến sĩ Y khoa", "Quận Ba Đình", "Có 32 năm kinh nghiệm trong lĩnh vực nghiên cứu và điều trị ung thư", 13, null, "Thành phố Hà Nội", "Approved", 19, "Phường Kim Mã", "9:00 - 15:00" },
                    { 13, "Số 153, Đường Nguyễn Thị Thập", "Bác sĩ nội trú chuyên ngành Nhi", "Quận 7", "Có 6 năm kinh nghiệm trong lĩnh vực nhi khoa và hồi sức cấp cứu nhi", 7, null, "Thành phố Hồ Chí Minh", "Approved", 20, "Phường Tân Phú", "8:00 - 20:00" },
                    { 14, "Số 88, Đường Lê Lợi", "Đại học Răng Hàm Mặt", "Thành phố Vinh", "Có 9 năm kinh nghiệm trong lĩnh vực chỉnh nha và thẩm mỹ răng", 3, null, "Tỉnh Nghệ An", "Approved", 21, "Phường Hà Huy Tập", "8:30 - 17:30" },
                    { 15, "Số 27, Đường Lê Hồng Phong", "Chuyên khoa I Da liễu", "Thành phố Vũng Tàu", "Có 13 năm kinh nghiệm trong lĩnh vực da liễu và thẩm mỹ da", 8, null, "Tỉnh Bà Rịa - Vũng Tàu", "Approved", 22, "Phường Thắng Tam", "8:00 - 17:00" },
                    { 16, "Số 56, Đường Phan Đình Phùng", "Chuyên khoa II Ngoại Tiêu hóa", "Thành phố Đà Lạt", "Có 16 năm kinh nghiệm trong lĩnh vực ngoại tiêu hóa và phẫu thuật nội soi", 9, null, "Tỉnh Lâm Đồng", "Approved", 23, "Phường 1", "8:00 - 16:00" },
                    { 17, "Số 19, Đường Hạ Long", "Đại học Y học cổ truyền", "Thành phố Hạ Long", "Có 11 năm kinh nghiệm trong lĩnh vực y học cổ truyền và châm cứu", 2, null, "Tỉnh Quảng Ninh", "Approved", 24, "Phường Bãi Cháy", "8:00 - 17:30" },
                    { 18, "Số 33, Đường Hùng Vương", "Tiến sĩ Y khoa chuyên ngành Tai Mũi Họng", "Thành phố Việt Trì", "Có 17 năm kinh nghiệm trong lĩnh vực tai mũi họng và phẫu thuật đầu cổ", 11, null, "Tỉnh Phú Thọ", "Approved", 25, "Phường Nông Trang", "7:30 - 17:00" },
                    { 19, "Số 48, Đường Nguyễn Tất Thành", "Thạc sĩ Y khoa chuyên ngành Mắt", "Thành phố Cà Mau", "Có 8 năm kinh nghiệm trong lĩnh vực nhãn khoa và phẫu thuật mắt", 10, null, "Tỉnh Cà Mau", "Approved", 26, "Phường 5", "7:30 - 16:30" },
                    { 20, "Số 10, Đường Tô Hiệu", "Đại học Y khoa", "Thành phố Sơn La", "Có 4 năm kinh nghiệm trong lĩnh vực y học gia đình và chăm sóc sức khỏe cộng đồng", 1, null, "Tỉnh Sơn La", "Pending", 27, "Phường Quyết Thắng", "7:00 - 17:00" }
                });

            migrationBuilder.InsertData(
                table: "ArticleImage",
                columns: new[] { "Id", "ArticleId", "ImgUrl" },
                values: new object[,]
                {
                    { 1, 1, "/images/articles/health-checkup-1.jpg" },
                    { 2, 1, "/images/articles/health-checkup-2.jpg" },
                    { 3, 2, "/images/articles/vaccination-1.jpg" },
                    { 4, 2, "/images/articles/vaccination-2.jpg" },
                    { 5, 3, "/images/articles/balanced-diet-1.jpg" },
                    { 6, 3, "/images/articles/balanced-diet-2.jpg" },
                    { 7, 4, "/images/articles/digestive-health-1.jpg" },
                    { 8, 4, "/images/articles/digestive-health-2.jpg" },
                    { 9, 5, "/images/articles/heart-health-1.jpg" },
                    { 10, 5, "/images/articles/heart-health-2.jpg" },
                    { 11, 6, "/images/articles/lung-cancer-1.jpg" },
                    { 12, 6, "/images/articles/lung-cancer-2.jpg" }
                });

            migrationBuilder.InsertData(
                table: "FacilityDepartments",
                columns: new[] { "Id", "DepartmentId", "FacilityId" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 2, 1 },
                    { 3, 3, 1 },
                    { 4, 4, 1 },
                    { 5, 5, 1 },
                    { 6, 6, 1 },
                    { 7, 11, 1 },
                    { 8, 12, 1 },
                    { 9, 16, 1 },
                    { 10, 19, 1 },
                    { 11, 1, 2 },
                    { 12, 2, 2 },
                    { 13, 5, 2 },
                    { 14, 6, 2 },
                    { 15, 11, 2 },
                    { 16, 15, 2 },
                    { 17, 17, 2 },
                    { 18, 18, 2 },
                    { 19, 1, 3 },
                    { 20, 2, 3 },
                    { 21, 3, 3 },
                    { 22, 4, 3 },
                    { 23, 7, 3 },
                    { 24, 10, 3 },
                    { 25, 1, 4 },
                    { 26, 2, 4 },
                    { 27, 3, 4 },
                    { 28, 4, 4 },
                    { 29, 11, 4 },
                    { 30, 1, 5 },
                    { 31, 2, 5 },
                    { 32, 5, 5 },
                    { 33, 14, 5 },
                    { 34, 5, 6 },
                    { 35, 6, 6 },
                    { 36, 19, 6 },
                    { 37, 8, 7 },
                    { 38, 1, 8 },
                    { 39, 2, 8 },
                    { 40, 3, 8 },
                    { 41, 10, 8 },
                    { 42, 1, 9 },
                    { 43, 2, 9 },
                    { 44, 5, 9 },
                    { 45, 8, 9 },
                    { 46, 1, 10 },
                    { 47, 5, 10 },
                    { 48, 1, 11 },
                    { 49, 4, 11 },
                    { 50, 1, 12 },
                    { 51, 2, 12 },
                    { 52, 3, 12 },
                    { 53, 4, 12 },
                    { 54, 1, 13 },
                    { 55, 2, 13 },
                    { 56, 9, 13 },
                    { 57, 1, 14 },
                    { 58, 2, 14 },
                    { 59, 13, 14 },
                    { 60, 1, 15 },
                    { 61, 3, 15 },
                    { 62, 10, 15 },
                    { 63, 1, 16 },
                    { 64, 2, 16 },
                    { 65, 3, 17 },
                    { 66, 4, 17 },
                    { 67, 5, 17 },
                    { 68, 1, 18 },
                    { 69, 2, 18 },
                    { 70, 5, 18 },
                    { 71, 17, 18 },
                    { 72, 1, 19 },
                    { 73, 4, 19 },
                    { 74, 1, 20 },
                    { 75, 2, 20 },
                    { 76, 5, 20 },
                    { 77, 6, 20 },
                    { 78, 7, 20 },
                    { 79, 10, 20 },
                    { 80, 16, 20 }
                });

            migrationBuilder.InsertData(
                table: "PrivateServices",
                columns: new[] { "Id", "Description", "Name", "Price", "ProfessionalId" },
                values: new object[,]
                {
                    { 1, "Khám tổng quát và tư vấn các bệnh lý nội khoa như tim mạch, tiêu hóa, hô hấp.", "Khám và tư vấn bệnh lý nội khoa", 500000m, 1 },
                    { 2, "Chẩn đoán và điều trị các bệnh lý về đường tiêu hóa như viêm dạ dày, trào ngược dạ dày.", "Điều trị bệnh lý tiêu hóa", 600000m, 1 },
                    { 3, "Khám, tư vấn và quản lý các bệnh mạn tính như tăng huyết áp, tiểu đường.", "Khám và quản lý bệnh mạn tính", 450000m, 8 },
                    { 4, "Tư vấn chế độ dinh dưỡng phù hợp cho người mắc các bệnh lý nội khoa.", "Tư vấn dinh dưỡng cho bệnh nhân nội khoa", 350000m, 8 },
                    { 5, "Khám và chẩn đoán chuyên sâu các bệnh lý tiêu hóa phức tạp.", "Khám bệnh tiêu hóa chuyên sâu", 700000m, 16 },
                    { 6, "Khám và điều trị các bệnh lý bằng phương pháp y học cổ truyền.", "Khám và điều trị bằng y học cổ truyền", 450000m, 2 },
                    { 7, "Sử dụng phương pháp châm cứu để điều trị các chứng đau nhức cơ xương khớp.", "Châm cứu điều trị đau nhức", 400000m, 2 },
                    { 8, "Xoa bóp, bấm huyệt kết hợp với các bài thuốc cổ truyền để điều trị đau mỏi.", "Xoa bóp bấm huyệt", 350000m, 17 },
                    { 9, "Sử dụng phương pháp cấy chỉ kết hợp với châm cứu để điều trị đau thần kinh tọa.", "Cấy chỉ điều trị đau thần kinh tọa", 600000m, 17 },
                    { 10, "Khám tổng quát răng miệng, tư vấn chăm sóc và phòng ngừa bệnh lý răng miệng.", "Khám và tư vấn răng miệng", 300000m, 3 },
                    { 11, "Điều trị tủy răng cho các trường hợp viêm tủy, hoại tử tủy.", "Điều trị tủy răng", 1200000m, 3 },
                    { 12, "Trám răng sâu bằng các vật liệu thẩm mỹ, màu sắc tự nhiên.", "Trám răng thẩm mỹ", 500000m, 14 },
                    { 13, "Nhổ răng khôn với phương pháp ít đau, an toàn, phục hồi nhanh.", "Nhổ răng khôn", 1500000m, 14 },
                    { 14, "Khám, chẩn đoán và tư vấn các bệnh lý tim mạch như tăng huyết áp, rối loạn nhịp tim.", "Khám và tư vấn bệnh lý tim mạch", 650000m, 4 },
                    { 15, "Siêu âm tim để đánh giá cấu trúc và chức năng của tim.", "Siêu âm tim", 600000m, 4 },
                    { 16, "Đo điện tâm đồ để đánh giá nhịp tim và phát hiện các bất thường về điện tim.", "Điện tâm đồ", 250000m, 4 },
                    { 17, "Khám, chẩn đoán và tư vấn các trường hợp cần can thiệp phẫu thuật.", "Khám và tư vấn phẫu thuật", 600000m, 5 },
                    { 18, "Phẫu thuật cắt bỏ các u lành tính trên da và dưới da.", "Phẫu thuật cắt u lành tính", 3500000m, 5 },
                    { 19, "Phẫu thuật nội soi điều trị các bệnh lý tiêu hóa như viêm ruột thừa, sỏi mật.", "Phẫu thuật nội soi tiêu hóa", 5000000m, 16 },
                    { 20, "Khám phụ khoa tổng quát định kỳ, phát hiện sớm các bệnh lý phụ khoa.", "Khám phụ khoa tổng quát", 500000m, 6 },
                    { 21, "Siêu âm đánh giá tình trạng tử cung, buồng trứng và các cơ quan sinh sản nữ.", "Siêu âm phụ khoa", 400000m, 6 },
                    { 22, "Khám thai định kỳ, theo dõi sự phát triển của thai nhi và sức khỏe của mẹ.", "Khám thai định kỳ", 550000m, 6 },
                    { 23, "Khám, chẩn đoán và tư vấn các bệnh lý thần kinh như đau đầu, động kinh, đột quỵ.", "Khám và tư vấn bệnh lý thần kinh", 700000m, 7 },
                    { 24, "Đo điện não đồ để đánh giá hoạt động điện của não và phát hiện bất thường.", "Điện não đồ", 650000m, 7 },
                    { 25, "Đánh giá tình trạng bệnh nhân và lập kế hoạch phục hồi chức năng cá nhân hóa.", "Đánh giá và lập kế hoạch phục hồi chức năng", 500000m, 9 },
                    { 26, "Vật lý trị liệu chuyên sâu cho các bệnh lý về cột sống như thoát vị đĩa đệm, đau thắt lưng.", "Vật lý trị liệu cột sống", 400000m, 9 },
                    { 27, "Khám, tư vấn và quản lý bệnh tiểu đường, bao gồm kế hoạch điều trị và chế độ dinh dưỡng.", "Khám và điều trị bệnh tiểu đường", 600000m, 10 },
                    { 28, "Khám, chẩn đoán và điều trị các bệnh lý của tuyến giáp như cường giáp, suy giáp.", "Khám và điều trị rối loạn tuyến giáp", 650000m, 10 },
                    { 29, "Khám, chẩn đoán và điều trị các bệnh lý hô hấp như viêm phổi, hen suyễn, COPD.", "Khám và điều trị bệnh hô hấp", 550000m, 11 },
                    { 30, "Đo và đánh giá chức năng hô hấp để chẩn đoán các bệnh lý phổi.", "Đo chức năng hô hấp", 450000m, 11 },
                    { 31, "Đánh giá các yếu tố nguy cơ và tư vấn phòng ngừa ung thư cá nhân hóa.", "Tư vấn và đánh giá nguy cơ ung thư", 1000000m, 12 },
                    { 32, "Khám định kỳ và theo dõi cho bệnh nhân sau điều trị ung thư.", "Khám và theo dõi sau điều trị ung thư", 1200000m, 12 },
                    { 33, "Khám sức khỏe tổng quát định kỳ cho trẻ em, theo dõi sự phát triển và tầm soát bệnh lý.", "Khám sức khỏe tổng quát cho trẻ", 500000m, 13 },
                    { 34, "Tư vấn chế độ dinh dưỡng và lịch tiêm chủng phù hợp theo từng độ tuổi của trẻ.", "Tư vấn dinh dưỡng và tiêm chủng cho trẻ", 400000m, 13 },
                    { 35, "Khám, chẩn đoán và điều trị các bệnh lý về da như mụn trứng cá, viêm da, chàm.", "Khám và điều trị bệnh da liễu", 550000m, 15 },
                    { 36, "Điều trị chuyên sâu mụn trứng cá và cải thiện tình trạng sẹo sau mụn.", "Điều trị mụn và sẹo", 700000m, 15 },
                    { 37, "Khám tổng quát tai, mũi, họng và chẩn đoán các bệnh lý liên quan.", "Khám tai mũi họng tổng quát", 500000m, 18 },
                    { 38, "Nội soi để chẩn đoán chính xác các bệnh lý tai, mũi, họng khó phát hiện bằng khám thường.", "Nội soi tai mũi họng", 600000m, 18 },
                    { 39, "Khám tổng quát mắt, kiểm tra thị lực và chẩn đoán các bệnh lý về mắt.", "Khám mắt tổng quát", 450000m, 19 },
                    { 40, "Đo khúc xạ chính xác và kê đơn kính phù hợp cho các trường hợp cận, viễn, loạn thị.", "Đo khúc xạ và kê đơn kính", 350000m, 19 },
                    { 41, "Khám sức khỏe tổng quát và tầm soát các bệnh lý thường gặp.", "Khám sức khỏe tổng quát", 400000m, 20 },
                    { 42, "Tư vấn các biện pháp phòng ngừa bệnh tật và duy trì lối sống lành mạnh.", "Tư vấn phòng ngừa bệnh tật", 300000m, 20 }
                });

            migrationBuilder.InsertData(
                table: "ProfessionalSpecialties",
                columns: new[] { "Id", "ProfessionalId", "SpecialtyId" },
                values: new object[,]
                {
                    { 9, 6, 6 },
                    { 10, 7, 4 },
                    { 11, 8, 1 },
                    { 12, 9, 11 },
                    { 13, 10, 14 },
                    { 14, 11, 13 },
                    { 15, 12, 8 },
                    { 16, 13, 7 },
                    { 17, 14, 15 },
                    { 18, 15, 5 },
                    { 19, 16, 2 },
                    { 20, 17, 12 },
                    { 21, 18, 10 },
                    { 22, 19, 9 },
                    { 23, 20, 1 },
                    { 24, 12, 3 },
                    { 25, 16, 1 }
                });

            migrationBuilder.InsertData(
                table: "PublicServices",
                columns: new[] { "Id", "Description", "FacilityId", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "Khám và tư vấn các bệnh lý nội khoa tổng quát, bao gồm đánh giá sức khỏe toàn diện.", 1, "Khám nội khoa tổng quát", 350000m },
                    { 2, "Khám, chẩn đoán và điều trị các bệnh lý về đường tiêu hóa như viêm dạ dày, trào ngược dạ dày.", 1, "Khám và điều trị bệnh tiêu hóa", 450000m },
                    { 3, "Khám và tư vấn các bệnh lý nội khoa, bao gồm đánh giá chức năng các cơ quan nội tạng.", 2, "Khám nội khoa tổng quát", 400000m },
                    { 4, "Khám, chẩn đoán và tư vấn điều trị cho bệnh nhân tiểu đường, bao gồm kiểm tra đường huyết.", 2, "Khám bệnh tiểu đường", 500000m },
                    { 5, "Khám và tư vấn các vấn đề ngoại khoa, bao gồm các bệnh lý cần can thiệp phẫu thuật.", 1, "Khám ngoại khoa tổng quát", 400000m },
                    { 6, "Phẫu thuật cắt ruột thừa viêm bằng phương pháp nội soi hoặc mổ mở.", 2, "Phẫu thuật cắt ruột thừa", 5000000m },
                    { 7, "Khám thai định kỳ, theo dõi sự phát triển của thai nhi và sức khỏe của mẹ.", 1, "Khám thai định kỳ", 400000m },
                    { 8, "Dịch vụ sinh thường trọn gói, bao gồm theo dõi chuyển dạ và chăm sóc sau sinh.", 3, "Sinh thường", 6000000m },
                    { 9, "Dịch vụ sinh mổ trọn gói theo yêu cầu, bao gồm phẫu thuật, chăm sóc sau sinh và hỗ trợ nuôi con bằng sữa mẹ.", 17, "Sinh mổ theo yêu cầu", 25000000m },
                    { 10, "Khám sức khỏe tổng quát cho trẻ em, đánh giá sự phát triển và tầm soát bệnh lý.", 1, "Khám nhi tổng quát", 350000m },
                    { 11, "Gói tiêm chủng cơ bản cho trẻ theo lịch của Bộ Y tế, bao gồm vắc xin và theo dõi sau tiêm.", 4, "Tiêm chủng cho trẻ", 1500000m },
                    { 12, "Xét nghiệm máu cơ bản bao gồm công thức máu, đường huyết và chức năng gan thận.", 1, "Xét nghiệm máu cơ bản", 200000m },
                    { 13, "Xét nghiệm vi sinh để phát hiện các tác nhân gây bệnh như vi khuẩn, virus.", 5, "Xét nghiệm vi sinh", 350000m },
                    { 14, "Chụp X-quang các bộ phận cơ thể để chẩn đoán bệnh lý xương khớp và phổi.", 1, "Chụp X-quang", 180000m },
                    { 15, "Chụp cộng hưởng từ để chẩn đoán chi tiết các bệnh lý về não, cột sống và các cơ quan nội tạng.", 2, "Chụp cộng hưởng từ (MRI)", 2500000m },
                    { 16, "Chụp cắt lớp vi tính để chẩn đoán chi tiết các bệnh lý khác nhau trong cơ thể.", 6, "Chụp cắt lớp vi tính (CT)", 1500000m },
                    { 17, "Khám tổng quát răng miệng, tư vấn chăm sóc và phòng ngừa bệnh lý răng miệng.", 3, "Khám răng tổng quát", 150000m },
                    { 18, "Cạo vôi răng để loại bỏ cao răng và mảng bám, giúp răng chắc khỏe và ngăn ngừa bệnh nướu.", 20, "Cạo vôi răng", 350000m },
                    { 19, "Khám tổng quát mắt, kiểm tra thị lực và chẩn đoán các bệnh lý về mắt.", 7, "Khám mắt tổng quát", 200000m },
                    { 20, "Phẫu thuật Lasik điều trị cận thị, viễn thị và loạn thị bằng công nghệ laser hiện đại.", 7, "Phẫu thuật Lasik", 25000000m },
                    { 21, "Khám tổng quát tai, mũi, họng và chẩn đoán các bệnh lý liên quan.", 13, "Khám tai mũi họng", 250000m },
                    { 22, "Nội soi tai mũi họng để chẩn đoán chính xác các bệnh lý tai, mũi, họng.", 13, "Nội soi tai mũi họng", 450000m },
                    { 23, "Khám và điều trị các bệnh lý về da như mụn trứng cá, viêm da, chàm.", 3, "Khám da liễu", 300000m },
                    { 24, "Điều trị chuyên sâu mụn trứng cá và cải thiện tình trạng sẹo sau mụn.", 15, "Điều trị mụn chuyên sâu", 800000m },
                    { 25, "Dịch vụ cấp cứu cho các trường hợp chấn thương, tai nạn.", 1, "Cấp cứu chấn thương", 500000m },
                    { 26, "Dịch vụ cấp cứu cho các trường hợp đột quỵ, nhồi máu cơ tim và các cấp cứu nội khoa khác.", 2, "Cấp cứu nội khoa", 450000m },
                    { 27, "Chăm sóc đặc biệt cho bệnh nhân trong tình trạng nguy kịch (tính theo ngày).", 1, "Hồi sức tích cực", 3500000m },
                    { 28, "Tư vấn và hỗ trợ tâm lý cho người gặp vấn đề về stress, lo âu, trầm cảm.", 14, "Tư vấn tâm lý", 400000m },
                    { 29, "Vật lý trị liệu cho các bệnh nhân đau cột sống, chấn thương và sau phẫu thuật.", 5, "Vật lý trị liệu", 250000m },
                    { 30, "Khám và điều trị các bệnh lý về hệ tiết niệu như sỏi thận, viêm đường tiết niệu.", 2, "Khám tiết niệu", 350000m },
                    { 31, "Khám và tư vấn các bệnh lý tim mạch như tăng huyết áp, rối loạn nhịp tim.", 1, "Khám tim mạch", 400000m },
                    { 32, "Siêu âm tim để đánh giá cấu trúc và chức năng của tim.", 20, "Siêu âm tim", 500000m },
                    { 33, "Khám và điều trị các bệnh lý hô hấp như viêm phổi, hen suyễn, COPD.", 2, "Khám hô hấp", 350000m },
                    { 34, "Đo và đánh giá chức năng hô hấp để chẩn đoán các bệnh lý phổi.", 18, "Đo chức năng hô hấp", 300000m },
                    { 35, "Khám và điều trị các bệnh lý nội tiết như tiểu đường, rối loạn tuyến giáp.", 2, "Khám nội tiết", 400000m },
                    { 36, "Khám, chẩn đoán và tư vấn điều trị các bệnh lý ung thư.", 1, "Khám ung bướu", 500000m },
                    { 37, "Điều trị ung thư bằng phương pháp xạ trị với máy móc hiện đại (tính cho một liệu trình).", 6, "Xạ trị ung thư", 12000000m },
                    { 38, "Tư vấn chế độ dinh dưỡng cá nhân hóa cho các bệnh lý khác nhau.", 1, "Tư vấn dinh dưỡng", 300000m },
                    { 39, "Gói khám sức khỏe tổng quát cơ bản bao gồm khám nội khoa, xét nghiệm máu, X-quang ngực.", 3, "Gói khám sức khỏe tổng quát cơ bản", 1500000m },
                    { 40, "Gói khám sức khỏe toàn diện bao gồm khám chuyên khoa, xét nghiệm, siêu âm và chẩn đoán hình ảnh.", 8, "Gói khám sức khỏe toàn diện", 5000000m },
                    { 41, "Khám và điều trị bằng các phương pháp y học cổ truyền như châm cứu, bấm huyệt.", 12, "Khám và điều trị y học cổ truyền", 200000m },
                    { 42, "Khám sức khỏe toàn diện theo yêu cầu cho người đi xuất khẩu lao động.", 16, "Khám sức khỏe đi xuất khẩu lao động", 1800000m },
                    { 43, "Khám và điều trị các bệnh thông thường như cảm cúm, viêm họng, tiêu chảy.", 10, "Khám và điều trị bệnh thông thường", 120000m },
                    { 44, "Tiêm các loại vắc xin theo yêu cầu (giá chưa bao gồm vắc xin).", 11, "Tiêm vắc xin theo yêu cầu", 500000m },
                    { 45, "Khám sức khỏe cơ bản và tư vấn phòng bệnh cho người dân địa phương.", 19, "Khám sức khỏe cơ bản", 80000m }
                });

            migrationBuilder.InsertData(
                table: "Appointments",
                columns: new[] { "Id", "Date", "Description", "PatientId", "PaymentId", "ProviderId", "ProviderType", "ServiceId", "ServiceType", "Status" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 3, 15, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 1, 1, 1, "Professional", 1, "Private", "Completed" },
                    { 2, new DateTime(2025, 3, 18, 14, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 2, 3, "Professional", 11, "Private", "Completed" },
                    { 3, new DateTime(2025, 4, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 3, 4, "Professional", 14, "Private", "Confirmed" },
                    { 4, new DateTime(2025, 4, 10, 15, 0, 0, 0, DateTimeKind.Unspecified), null, 4, 4, 2, "Professional", 7, "Private", "Pending" },
                    { 5, new DateTime(2025, 4, 12, 9, 30, 0, 0, DateTimeKind.Unspecified), null, 5, null, 17, "Professional", 9, "Private", "AwaitingPayment" },
                    { 6, new DateTime(2025, 4, 2, 13, 0, 0, 0, DateTimeKind.Unspecified), null, 6, 5, 15, "Professional", 36, "Private", "Cancelled" },
                    { 7, new DateTime(2025, 3, 25, 8, 0, 0, 0, DateTimeKind.Unspecified), null, 1, null, 1, "Facility", 1, "Public", "Expired" },
                    { 8, new DateTime(2025, 4, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), null, 2, 6, 7, "Facility", 19, "Public", "AwaitingPayment" },
                    { 9, new DateTime(2025, 4, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, 17, "Facility", 9, "Public", "Rejected" },
                    { 10, new DateTime(2025, 4, 18, 14, 0, 0, 0, DateTimeKind.Unspecified), null, 4, null, 3, "Facility", 39, "Public", "Rescheduled" },
                    { 11, new DateTime(2025, 4, 22, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 5, 8, 6, "Facility", 36, "Public", "AwaitingPayment" },
                    { 12, new DateTime(2025, 4, 25, 15, 30, 0, 0, DateTimeKind.Unspecified), null, 6, 7, 5, "Facility", 29, "Public", "AwaitingPayment" },
                    { 13, new DateTime(2025, 4, 28, 8, 30, 0, 0, DateTimeKind.Unspecified), null, 1, 9, 12, "Facility", 41, "Public", "AwaitingPayment" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PatientId",
                table: "Appointments",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PaymentId",
                table: "Appointments",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_ProviderId",
                table: "Appointments",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_ServiceId",
                table: "Appointments",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleImage_ArticleId",
                table: "ArticleImage",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_CategoryId",
                table: "Articles",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_CreatedById",
                table: "Articles",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_MedicalRecordId",
                table: "Attachments",
                column: "MedicalRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_Facilities_TypeId",
                table: "Facilities",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FacilityDepartments_DepartmentId",
                table: "FacilityDepartments",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_FacilityDepartments_FacilityId",
                table: "FacilityDepartments",
                column: "FacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalRecords_AppointmentId",
                table: "MedicalRecords",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_UserId",
                table: "Patients",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateServices_ProfessionalId",
                table: "PrivateServices",
                column: "ProfessionalId");

            migrationBuilder.CreateIndex(
                name: "IX_Professionals_ExpertiseId",
                table: "Professionals",
                column: "ExpertiseId");

            migrationBuilder.CreateIndex(
                name: "IX_Professionals_ExpertiseId1",
                table: "Professionals",
                column: "ExpertiseId1");

            migrationBuilder.CreateIndex(
                name: "IX_Professionals_UserId",
                table: "Professionals",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ProfessionalSpecialties_ProfessionalId",
                table: "ProfessionalSpecialties",
                column: "ProfessionalId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfessionalSpecialties_SpecialtyId",
                table: "ProfessionalSpecialties",
                column: "SpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_PublicServices_FacilityId",
                table: "PublicServices",
                column: "FacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_PatientId",
                table: "Reviews",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ProviderId",
                table: "Reviews",
                column: "ProviderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleImage");

            migrationBuilder.DropTable(
                name: "Attachments");

            migrationBuilder.DropTable(
                name: "FacilityDepartments");

            migrationBuilder.DropTable(
                name: "ProfessionalSpecialties");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "MedicalRecords");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Specialties");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "PrivateServices");

            migrationBuilder.DropTable(
                name: "PublicServices");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Professionals");

            migrationBuilder.DropTable(
                name: "Facilities");

            migrationBuilder.DropTable(
                name: "Expertises");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "FacilityTypes");
        }
    }
}
