Solution template for REST WebApi.

- Net 7.0
- Asp.Net 7.0
    - [FastEndpoints](https://fast-endpoints.com/) (instead of Controllers)
      - FluentValidation as part of FastEndpoints
- MsSQL database
  - MsSql project (DACPAC DB state management)
  - Entity Framework (DB first)
- Serilog
  - Debug/Console sinks (in dev)
  - ApplicationInsights sink
- Salix.AspNetUtilities
  - JSON error handling
  - Front page
  - HealthCheck transformer and page
  - Configuration validation
- Testing
  - XUnit
  - FluentAssertions
  - Moq
  - Bogus Faker (real object instance generation)


# Solution projects

Solution is split into multiple projects, 
each responsible for own doings with necessary/allowed refences between them.

**WebApi** is responsible for handling HTTP communication with clients - mainly using asp.net and fast-endpoints 
handle HTTP calls and data serialization.\
It is refined (extensions) to easily understand setup and boot procedures.\
Some love is given to clean up for security (removal of unnecessary response headers).
Global error/exception handler with JSON error object in body.

**NOTE**\
_There is nothing done in regard to authentication and authorization, as each project may use its own way to handle auth* with its own requirements._\
FastEndpoints has built-in some of functionalities for own implementations (JWT Token, Token Refresh, Cookie auth) to use if necessary.

**Urls** is separate small project containing WebApi endpoint addresses.
It is separated in case these are shared with come C# client as strongly typed addresses.

**CoreLogic** is project where all business logic and data sources are accessed.

**Domain** contains WebApi data contracts (DTOs).

**Domain.Fakes** uses Bogus Faker to create realistic Domain objects for Swagger docs (examples) and automated testing projects.

**Crosscut** contains classes, shareable in any of other projects (Exceptions, Types, Extensions (to .Net types/classes, NOT Business!)).

**Enumerations** contains enums, shareable in any of other projects (Database, CoreLogic, Domain).

**Database/Database.MsSql** is special MsSql structure modification project with ability to change structure on target DB state (DACPAC).

**Database/Database.Orm** is is Entity Framework context to access database data (Database-First approach with _MsSql_ above).

**Database/Database.Orm.Fakes** is using Bogus Faker to create Entity Framework entitiies objects for automated testing.

# Automated testing

In folder **Tests** are automated testing (unit-tests, component tests, integration tests) 
by using these engines and libraries:
- **XUnit** as testing engine
- **FluentAssertions** as assertion library
- **Moq** as mocking library

There is base class in CoreLogic to provide in-memory SQLite engine and prepare for entity framework context usage in logic classes.

Projects are setup to use coverlet to calculate code coverage. Use Visual Studio extension **Fine Code Coverage** to see coverage reports (unless you use Visual Studio Enterprise)

# Build sample functionalities

There are few ready-made endpoints in this API and database/EF setup to demonstrate real use for template.\
These are heavily/fully set-up with Swagger OpenApi descriptions and request/response examples to showcase entire possibilities to document API endpoints.\
In reality you might not need such extensive documentation.

### Classic Weather
Weather endpoint from VS template is left in to show how it fits with FastEndpoints implementation. Remove it with actual use.

### Sandbox
There are several endpoints showcasing return of different types from API:
- Strings/Char, both ANSI and multi-language strings.
- Numbers (Integers, Decimals, FLoat, Double)
- Date/Time (all kinds of)
- Others (Enum, Array...)
- Exception thrown (showing 500 response body with JSON Error object)

### System Notifications
A reusable logic you may use in your project, which allows to set-up system notification for end-
users.
These may be 
- warnings of outages (example: we will have app maintenance on friday)
- warnings of partial app or external parties outages (example: do not use e-mail sending as we have problems with mail server)
- notifications of performed update (example: new functionality available - maps in client forms)

System notifications have timespan where they are active = shown for users, so app admins can set them up before they start to show up 
and they will be gone for end users as soon as end time for notification passes.

System Notification can have two states - normal and emphasized. This allows to show notification as simple normal for some time and 
then for last hour/minute to become more visible (emphasized/bold) to make users notice it.
You may or may not implement it in your UI as it is only a flag, controlled by date/time.

Also there is a possibility to set a flag to show countdown timer (like for last 5 minutes (before maintenance break)) of system notification.
Again - you may or may not implement such feature in your UI, but flag to set such up is there.

Notification messages themselves can be set up in multiple languages, if you app is multilingual.


# Builds
Template includes YAML file for Azure DevOps service to build and run tests in its pipeline. 
Testing also is configured to create code coverage report and show on Azure DevOps pipeline build report.

`MsSqlProj` type projects are picky when it comes to command-line builds, so it is **switched off (removed) from release build**.

Create separate pipeline for this project type, using `VSBuild` or `msbuild` tasks on windows-latest image.

