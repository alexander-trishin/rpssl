# Rock, Paper, Scissors, Lizard, Spock
 
## Overview

This project is based on [dotnet react template](https://docs.microsoft.com/en-us/visualstudio/javascript/tutorial-asp-net-core-with-react?view=vs-2022)
and implements [CQRS](https://docs.microsoft.com/en-us/azure/architecture/patterns/cqrs) pattern. Due to that, it uses the following libraries:

- _System.Text.Json_ - a primary JSON serializer;
- _MediatR_ - the Mediator pattern implementation for CQRS pattern realization;
- _FluentValidation_ - used for request validation;
- _Swashbuckle.AspNetCore_ - Swagger page generator for RestAPI.

### Local Development (with Visual Studio)
- Open solution.
- Choose `Start Debugging` or `Start without Debugging` on `Debug` panel.
- After a while browser window with main UI should open.
- UI for swagger will be available at `/swagger` endpoint.

### Local Development

* Check you have .NET 6.0 installed by typing on commandline `dotnet --version`
* Build: `dotnet build` from the terminal or your preferred IDE.
  * Visual Studio: Open solution file in root directory.
* Run: `dotnet run --project src/RPSSL.UI`
* Explore the service's UI at <https://localhost:7233>
* Explore the service's api through Swagger at <https://localhost:7233/swagger> in your browser

## Testing

### Unit tests

* Run `dotnet test`
