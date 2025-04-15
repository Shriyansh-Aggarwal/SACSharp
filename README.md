
# SACSharp
A static analyzer for C# projects. Currently an ongoing project.
## Installation
You need .NET 8.0 to run this project.

Install SACSharp with dotnet

```bash
  git clone https://github.com/Shriyansh-Aggarwal/SACSharp.git
  cd SACSharp
  dotnet build
  dotnet pack -c Release --output ./nupkg
  dotnet tool install --global --add-source ./nupkg sacsharp
```
    
## Usage

Create a C# project and use the below command (Remember to only provide the project directory as the argument, not a file!)

```bash
sacsharp scan /path/to/project-directory
```

I have added a tests folder in the repository, you can run the tests to check if the installation works by running this command in SACSharp directory:
```bash
sacsharp scan ./Tests
```