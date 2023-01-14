Solution template for REST WebApi.

- Net 7.0
- Asp.Net 7.0
    - [FastEndpoints](https://fast-endpoints.com/) (instead of Controllers)
- Serilog
  - Debug/Console sinks (in dev)
  - ApplicationInsights sink
- Salix.AspNetUtilities
  - JSON error handling
  - Front page
  - HealthCheck transformer and page
  - Configuration validation

# Solution projects

Solution is split into multiple projects, 
each responsible for own doings with necessary/allowed refences between them.

**WebApi** is responsible for handling HTTP communication with clients - mainly using asp.net and fast-endpoints 
handle HTTP calls and data serialization.

**Urls** is separate small project containing WebApi endpoint addresses.
It is separated in case these are shared with come C# client as strongly typed addresses.

**CoreLogic** is project where all business logic and data sources are accessed.

**Domain** contains WebApi data contracts (DTOs).

**Crosscut** contains classes, shareable in any of other projects (Exceptions, Types, Extensions (to .Net types/classes, NOT Business!)).

