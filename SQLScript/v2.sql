Use CMRDB
DROP TABLE dbo.AnnualCourseRecord

DROP Table dbo.ApproveStatus
create table ApproveStatus (
	approveStatusId int primary key identity(1, 1),
	name varchar(50)
)
insert into ApproveStatus values('Pending')
insert into ApproveStatus values('Waiting')
insert into ApproveStatus values('Rejected')
insert into ApproveStatus values('Completed')

create table CourseMonitoringReport (
	CourseMonitoringReportId int primary key identity (1, 1),
	annualCourseId int foreign key(annualCourseId) references AnnualCourse(annualCourseId),
	failCount int,
	passCount int,
	creditCount int,
	distinctionCount int,
	evaluation varchar(500),
	approveStatusId int foreign key(approveStatusId) references ApproveStatus(approveStatusId),
)

