
![Zilla Logo](logo.png)
Jira clone for Web Application Development course

## Notes

### General
 - ASP.NET MVC 5 
 - Form validation 
 - UI + Front-end framework 
 - UX 
 - Admin full CRUD access

### User types
	Guest
		Access limited to homepage and authentication forms
	User
		Organizer
			Task management
			Task assignment
			Can edit task statuses
		Member
			Access limited to tasks from his team's projects
			Comment on existing tasks
			Manage own comments
			Can edit task statuses
	Administrator
		Access to entire application
		Task/team/comment/etc. management
		Can change user's privileges
	
### Functionalities
	Project author becomes organizer automatically
	Task statuses: Not started, In progress, Completed
	Tasks listed on team page
	User privilege management
	
### Models (+Controllers)
	User
		Mail                                 : String
		Password                             : String
		First name |  Username/Display name  : String
		Last name  |
		Description                          : String
		Privileges/Type (Normal / Admin)     : Integer
		Registration date                    : Date
		? Teams                              : Team[]
		? Comments                           : Comment[]
		
	Team
		Title                                : String
		Description                          : String
		Members                              : User[]
		Projects                             : Project[]
	
	Project
		Title                                : String
		Description                          : String
		Organizers                           : User[]
		Members                              : User[]
		Tasks                                : Task[]
		? Team                               : Team
	
	Task
		Title                                : String
		Description                          : String
		Assigner                             : User[]
		Assignee                             : User[]
		Status                               : Integer
		Comments                             : Comment[]
		Start date                           : Date
		End date                             : Date
		? Project                            : Project
		
	Comment
		Author                               : User
		? Title                              : String
		Content                              : String
		Creation date/Timestamp              : Date
