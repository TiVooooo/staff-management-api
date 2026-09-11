# Task Management - Backend

Backend API for the Task Management application.

## Technologies

* ASP.NET Core
* Entity Framework Core
* SQL Server
* Database First

## Features

* Staff CRUD
* Task CRUD
* Search Staff by term
* Search Task by term
* Staff - Task management

## API

### Staff

* `GET /api/Staff`
* `POST /api/Staff`
* `GET /api/Staff/{id}`
* `PUT /api/Staff/{id}`
* `DELETE /api/Staff/{id}`

### Task

* `GET /api/Task`
* `POST /api/Task`
* `GET /api/Task/{id}`
* `PUT /api/Task/{id}`
* `DELETE /api/Task/{id}`

Search example:

```text
GET /api/Task?term=keyword
```

## Database

SQL Server with the following tables:

* Staff
* Task
* StaffInTask

## Run

1. Configure the SQL Server connection string.
2. Run the project.
3. Use Swagger to test the APIs.
