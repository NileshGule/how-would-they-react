# Celebrity Impersonation using AI

# How Would They React - .NET Console (LLM)

This project is a .NET console application that generates AI-powered celebrity reactions using LLMs and Microsoft.AI.Foundry.Local.

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/)
- [Microsoft.AI.Foundry.Local](https://github.com/microsoft/foundry-local) (local project reference, see below)

## Setup Instructions

### 1. Clone and Build Microsoft.AI.Foundry.Local

This project references `Microsoft.AI.Foundry.Local` as a local project reference. You must clone and build it first:

```bash
git clone https://github.com/microsoft/foundry-local.git
cd foundry-local/sdk/cs/src
# Build the project (ensure you have .NET 9 SDK)
dotnet build
```

### 2. Reference Microsoft.AI.Foundry.Local Locally

The `.csproj` already includes a local project reference:

```xml
<ItemGroup>
  <ProjectReference Include="/Users/nilesh/projects/foundry-local/sdk/cs/src/Microsoft.AI.Foundry.Local.csproj" />
</ItemGroup>
```

If you clone to a different path, update the `Include` path accordingly.

### 3. Build and Run

From the project root:

```bash
dotnet build
# Run the console app
dotnet run --project how-would-they-react-dotnet/how-would-they-react-dotnet.csproj
```

## Note
Once the official Microsoft.AI.Foundry.Local SDK is released and available on NuGet, this project will reference it as a normal NuGet package instead of a local project reference.

Refer to the [Foundry-Local](https://github.com/microsoft/Foundry-Local/) GitHub project for more updates. 

## Related YouTube Content

[![Local AI Models with Foundry Local](../images/Local%20AI%20Models%20with%20Foundry%20Local.png)](https://youtu.be/UYHZY6AbQ-4)

Watch the YouTube about setting up Foundry Local which references this demo:
[Local AI Models with Foundry Local - YouTube](https://youtu.be/UYHZY6AbQ-4)


[![Foundry Local Celebrity Impersonator C#](../images/foundry%20local%20celebrity%20impersonator.png)](https://youtu.be/w5yfetTzIgk)

Watch the new YouTube video about the Foundry Local Celebrity Impersonator C# demo:
[Foundry Local Celebrity Impersonator C# - YouTube](https://youtu.be/w5yfetTzIgk)

