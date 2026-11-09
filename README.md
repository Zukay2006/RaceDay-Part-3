# RaceDay – Part 3: MVC Web Application

## Project Overview

**RaceDay** is a web-based event management platform developed using ASP.NET Core MVC. It provides an interface for participants to discover and enrol in sporting events, while organisers can manage events, categories, enrolments and results.

This is Part 3 of the RaceDay project. The MVC application communicates with the RaceDay REST API developed in Part 2.

## Main Features

### Participant Features
- Register and log in.
- Browse and view sporting events.
- View event categories.
- Enrol in events.
- View personal enrolments.
- View published results.
- View and update profile information.
- Upload a profile picture.

### Organiser Features
- Organiser registration and login.
- Organiser dashboard.
- Create, edit and delete events.
- Manage event categories.
- View participant enrolments.
- Enter and publish race results.

## Technologies Used

- C#
- ASP.NET Core MVC
- .NET 10
- ASP.NET Core Web API
- Entity Framework Core (through the API)
- Microsoft Azure Blob Storage
- Bootstrap
- HTML and CSS
- JavaScript
- Docker
- GitHub Actions

## Project Architecture

The MVC application communicates with the RaceDay API using HTTP requests.

The MVC application does not directly access the database or Azure Blob Storage. Data operations and image uploads are handled through the API.

## Requirements

- .NET 10 SDK
- Docker Desktop
- RaceDay Part 2 API
- Visual Studio 2026 or another compatible development environment

## Running the Application Locally

1. Clone this repository.
2. Open the MVC project in Visual Studio.
3. Start the RaceDay Part 2 API using its HTTP profile.
4. Ensure the API is listening on `http://localhost:5036`.
5. Run the MVC application.
6. Open the local MVC address provided by Visual Studio.

The API base URL can be configured through the `ApiSettings:BaseUrl` setting.

## Running with Docker

Build the Docker image from the MVC project directory:

```bash
docker build -t raceday-mvc .
```

Run the container:

```bash
docker run -d -p 8080:8080 --name raceday-mvc-test -e ApiSettings__BaseUrl=http://host.docker.internal:5036/ raceday-mvc
```

Open the application at:

`http://localhost:8080`

**Important:** The RaceDay API must also be running and accessible. The Docker container hosts the MVC application, while the API runs separately.

To stop the container:

```bash
docker stop raceday-mvc-test
```

To start it again:

```bash
docker start raceday-mvc-test
```

## GitHub Actions

GitHub Actions is configured to automatically restore dependencies and build the MVC application when changes are pushed to the main branch or a pull request is created.

## Cloud Storage

Azure Blob Storage is used for profile picture storage through the RaceDay API. The MVC application communicates with the API for image uploads and retrieval.

## Repository

GitHub: https://github.com/Zukay2006/RaceDay-Part-3

## Youtube demonstration

https://youtu.be/aAeZ7vBkhHk?si=6eCbKlOBl85aA_nb

## Author

**Zukhanyise Vitshima**

*RaceDay – Part 3*
