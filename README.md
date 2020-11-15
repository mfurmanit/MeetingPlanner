<img  width="150" height="150" href="https://fvm.ukim.edu.mk/vetnest-summerschool/wp-content/uploads/2018/11/calendars-icon.png" src="readme/icon.png" alt="MeetingPlanner icon">

# MeetingPlanner

Project carried out for university subject. Web application written in ASP.NET Core 3.1 and Angular 10.

Main purpose of the project was to implement simple application using features and capabilities of ASP.NET such as Dependency Injection, Entity Framework Core, Localization, SPA and others.

## Description

MeetingPlanner is a simple meeting scheduling application. Unauthorized users can create and edit global meetings that are seen by everybody. Logged in users have access to the "Private meetings" tab, where their own events are located. Private meetings allow you to add e-mail notifications that are sent at a specified time before the start of the meeting (to the user's e-mail address provided during registration / logging in).
All appointments appear as blue dots in the calendar view. On hover you can see title and (if specified) start / end time of the event. After clicking on the calendar tile, we can go to the details (edit) of the meeting or delete it (icons respectively - pencil and trash).

## What can be implemented in the future

- notifications for global events
- notifications by SMS
- easier way to update meeting date using drag and drop on calendar view
- event color selection
- translation of backend errors depending on the language chosen by user

## What I've learned:

- how to build, run and structure simple ASP.NET Core project with Angular
- how to deploy created project to Azure
- how to configure key settings and components using Azure App Service
- how to create CI/CD with integrations tests, build and deploy to Azure (using GitHub Actions)
- how to connect with MSSQL / PostgreSQL database from the code
- how to create and execute migrations based on built model
- how to enable logging to file on development and production environment
- how to implement translations using couple languages - both on frontend (ngx-translate) and backend (scaffold Razor Pages)
- how to implement scheduled jobs in ASP.NET (using Quartz.NET library)
- how to create integration tests with available technology using HttpClient and InMemory database
- how to handle exceptions using custom middleware on .NET side and Angular error handler
- how to manage authentication / authorization with Identity Server 4
- how to make better commits, tasks, pull requests and manage project on GitHub
- I've developed my previous skills in Angular (components, pipes, modules and everything else)
- I've developed my previous skills in English a little bit

## Images
<details>
<summary>
  Expand to show images!
</summary>
  <img src="readme/1%20-%20global calendar.png" width="1000px" /> <br />
  <img src="readme/2%20-%20global meeting details.png" width="1000px" /> <br />
  <img src="readme/3%20-%20languages.png" width="1000px" /> <br />
  <img src="readme/4%20-%20time input.png" width="1000px" /> <br />
  <img src="readme/5%20-%20private meeting details.png" width="1000px" /> <br />
  <img src="readme/6%20-%20private calendar.png" width="1000px" />
</details>
