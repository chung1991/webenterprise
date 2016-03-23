use master
go
if exists (select name from master.sys.databases where name = N'CMRDB')
drop database CMRDB
create database CMRDB
go
use CMRDB

create table [Role] (
	roleId int primary key identity(1, 1),
	roleName varchar(100),
	roleDescription varchar(255)
)
insert into [Role] values('Admin', 'Admin')
insert into [Role] values('CL', 'Course Leader')
insert into [Role] values('CM', 'Course Moderator')
insert into [Role] values('DLT', 'Director of Learning and Quality')
insert into [Role] values('PVC', 'Pro-Vice Chancellor')

create table [Profile] (
	profileId int primary key identity(1, 1),
	name varchar(50),
	[address] varchar(255),
	email varchar(255),
	telephone varchar(12),
	dateOfBirth date
)
insert into [Profile] values('Admin', 'Ha Noi','cmr@gmail.com','123456789', '1/1/1970')
insert into [Profile] values('CLAccount', 'Ha Noi','cmr@gmail.com','123456789', '1/1/1970')
insert into [Profile] values('CMAccount','Ha Noi','cmr@gmail.com','123456789', '1/1/1970')

create table Account (
	accountId int primary key identity(1, 1),
	userName varchar(20) unique,
	userPassword varchar(100),
	profileId int foreign key (profileId) references [Profile] (profileId),
	roleId int foreign key(roleId) references Role(roleId)
)
insert into Account values('admin', 'admin', 1, 1)
insert into Account values('CLAccount', '123456', 2, 2)
insert into Account values('CMAccount', '123456', 3, 3)

create table Faculty (
	facultyId int primary key identity(1, 1),
	name varchar(100),
	pvcAccount int foreign key (pvcAccount) references [Account] (accountId),
	dltAccount int foreign key (dltAccount) references [Account] (accountId),
	cmAccount int foreign key (cmAccount) references [Account] (accountId)
)
insert into Faculty values('Sience',null,null,3)

create table Course (
	courseId int primary key identity(1, 1),
	name varchar(100),
	facultyId int foreign key(facultyId) references Faculty(facultyId)
)
insert into Course values('Sience in use', 1)

create table AnnualCourse (
	annualCourseId int primary key identity(1, 1),
	academicYear int,
	courseId int foreign key(courseId) references Course(courseId),
	clAccount int foreign key(clAccount) references Account(accountId)
)

create table ApproveStatus (
	approveStatusId int primary key identity(1, 1),
	name varchar(50)
)
insert into ApproveStatus values('Pending')
insert into ApproveStatus values('Waiting')
insert into ApproveStatus values('Rejected')
insert into ApproveStatus values('Approved')

create table CourseMonitoringReport (
	CourseMonitoringReportId int primary key identity (1, 1),
	annualCourseId int foreign key(annualCourseId) references AnnualCourse(annualCourseId),
	studentTotal int,
	markA int,
	markB int,
	markC int,
	markD int,
	clEvaluation varchar(500),
	approveStatusId int foreign key(approveStatusId) references ApproveStatus(approveStatusId),
	approve_desc varchar(500),
	dltComment varchar(255),
	dateCreated date
)

create table Comment (
	commentId int primary key identity(1, 1),
	content varchar(500),
	accountId int foreign key (accountId) references Account (accountId),
	monitoringReportId int foreign key (monitoringReportId) references CourseMonitoringReport (CourseMonitoringReportId)
)