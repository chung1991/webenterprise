use master
go
if exists (select name from master.sys.databases where name = N'CMRDB')
drop database CMRDB
create database CMRDB
go
use CMRDB

create table [Role] (
	roleId int primary key,
	roleName varchar(100),
	roleDescription varchar(255)
)
insert into [Role] values('0', 'Admin', 'Admin')
insert into [Role] values('1', 'CL', 'Course Leader')
insert into [Role] values('2', 'CM', 'Course Moderator')
insert into [Role] values('3', 'DLT', 'Director of Learning and Quality')
insert into [Role] values('4', 'PVC', 'Pro-Vice Chancellor')

create table [Profile] (
	profileId int primary key identity(1, 1),
	name varchar(50),
	[address] varchar(255),
	telephone varchar(12),
	dateOfBirth datetime
)
insert into [Profile] values('Admin', '', '', '1/1/1970')

create table Account (
	accountId int primary key identity(1, 1),
	userName varchar(20) unique,
	userPassword varchar(100),
	profileId int foreign key (profileId) references [Profile] (profileId),
	roleId int foreign key(roleId) references Role(roleId)
)
insert into Account values('admin', 'admin', '1', '0')

create table Faculty (
	facultyId int primary key identity(1, 1),
	name varchar(100)
)
insert into Faculty values('Sience')

create table Course (
	courseId int primary key identity(1, 1),
	name varchar(100),
	facultyId int foreign key(facultyId) references Faculty(facultyId)
)
insert into Course values('Sience in use', '1')

create table AnnualCourse (
	annualCourseId int primary key identity(1, 1),
	academicYear int,
	courseId int foreign key(courseId) references Course(courseId)
)

create table ApproveStatus (
	approveStatusId int primary key identity(1, 1),
	name varchar(50)
)
insert into ApproveStatus values('Pending')

--fail/pass/credit/distinction is 4 levels of certificate, sum of them is student count
create table AnnualCourseRecord (
	annualCourseRecordId int primary key identity (1, 1),
	annualCourseId int foreign key(annualCourseId) references AnnualCourse(annualCourseId),
	failCount int,
	passCount int,
	creditCount int,
	distinctionCount int,
	evaluation varchar(500),
	approveStatusId int foreign key(approveStatusId) references ApproveStatus(approveStatusId),
)

create table Comment (
	commentId int primary key identity(1, 1),
	content varchar(500),
)
create table CLAnnualCourse(
	Id int primary key identity (1, 1),
	CLId int foreign key(CLId) references Account(accountId),
	annualCourseId int foreign key(annualCourseId) references AnnualCourse(annualCourseId)
)