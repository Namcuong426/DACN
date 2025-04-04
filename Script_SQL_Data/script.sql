USE [EmployeeManagement]
GO
SET IDENTITY_INSERT [dbo].[department] ON 

INSERT [dbo].[department] ([id], [department_name], [description], [abbreviation], [created_at], [updated_at]) VALUES (6, N'Phòng hành chính', N'Mô tả phc', N'PHC', CAST(N'2024-12-01T00:00:00.0000000' AS DateTime2), CAST(N'2024-12-01T00:00:00.0000000' AS DateTime2))
SET IDENTITY_INSERT [dbo].[department] OFF
GO
SET IDENTITY_INSERT [dbo].[education_level] ON 

INSERT [dbo].[education_level] ([id], [education_name], [description], [abbreviation], [created_at], [updated_at]) VALUES (3, N'Giao duc 1', N'gdcd', N'GD1', CAST(N'2024-01-01T00:00:00.0000000' AS DateTime2), CAST(N'2024-01-01T00:00:00.0000000' AS DateTime2))
SET IDENTITY_INSERT [dbo].[education_level] OFF
GO
SET IDENTITY_INSERT [dbo].[position] ON 

INSERT [dbo].[position] ([id], [position_name], [description], [created_at], [updated_at], [abbreviation]) VALUES (2, N'Chuc vu 1', N'truong phong', CAST(N'2024-01-01T00:00:00.0000000' AS DateTime2), CAST(N'2024-01-01T00:00:00.0000000' AS DateTime2), N'sssssss')
SET IDENTITY_INSERT [dbo].[position] OFF
GO
SET IDENTITY_INSERT [dbo].[employee] ON 

INSERT [dbo].[employee] ([id], [name], [date_of_birth], [gender], [address], [phone_number], [email], [hire_date], [foreign_language_proficiency], [citizen_id], [department_id], [education_level_id], [position_id], [basic_salary], [Username], [Password], [IsAdmin], [IsActivated], [created_at], [updated_at], [employee_code], [avatar]) VALUES (23, N'TaoLaAdmin', CAST(N'2023-12-24T00:00:00.0000000' AS DateTime2), N'1', N'Hà Nội', N'0921846578', N'admin@gmail.com', CAST(N'2023-12-24T00:00:00.0000000' AS DateTime2), 1, N'001200023333', 6, 3, 2, CAST(10000.0000 AS Decimal(18, 4)), N'admin@gmail.com', N'E10ADC3949BA59ABBE56E057F20F883E', 1, 1, CAST(N'2024-01-12T00:00:00.0000000' AS DateTime2), CAST(N'2024-01-12T00:00:00.0000000' AS DateTime2), N'001', NULL)
INSERT [dbo].[employee] ([id], [name], [date_of_birth], [gender], [address], [phone_number], [email], [hire_date], [foreign_language_proficiency], [citizen_id], [department_id], [education_level_id], [position_id], [basic_salary], [Username], [Password], [IsAdmin], [IsActivated], [created_at], [updated_at], [employee_code], [avatar]) VALUES (24, N'TaoLaNhanVien', CAST(N'2023-12-24T00:00:00.0000000' AS DateTime2), N'1', N'Hà Nội', N'0923333333', N'nhanvien@gmail.com', CAST(N'2023-12-24T00:00:00.0000000' AS DateTime2), 1, N'001200026666', 6, 3, 2, CAST(10000.0000 AS Decimal(18, 4)), N'nhanvien@gmail.com', N'E10ADC3949BA59ABBE56E057F20F883E', 0, 1, CAST(N'2024-01-12T00:00:00.0000000' AS DateTime2), CAST(N'2024-01-12T00:00:00.0000000' AS DateTime2), N'001', NULL)
SET IDENTITY_INSERT [dbo].[employee] OFF
GO
SET IDENTITY_INSERT [dbo].[account] ON 

INSERT [dbo].[account] ([id], [username], [password], [role], [created_at], [updated_at]) VALUES (3, N'admin@gmail.com', N'E10ADC3949BA59ABBE56E057F20F883E', 2, CAST(N'2024-01-01T00:00:00.0000000' AS DateTime2), CAST(N'2024-01-01T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[account] ([id], [username], [password], [role], [created_at], [updated_at]) VALUES (5, N'nhanvien@gmail.com', N'E10ADC3949BA59ABBE56E057F20F883E', 0, CAST(N'2024-01-01T00:00:00.0000000' AS DateTime2), CAST(N'2024-01-01T00:00:00.0000000' AS DateTime2))
SET IDENTITY_INSERT [dbo].[account] OFF
GO
