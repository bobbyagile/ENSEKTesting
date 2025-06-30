How to Run These Tests
1. Create a .NET Test Project: If you haven't already, create a new "MSTest Test Project" in Visual Studio or using dotnet new mstest from the CLI.

2.Install Packages: Ensure you have the necessary NuGet packages:
Microsoft.Playwright
Microsoft.NET.Test.Sdk
MSTest.TestAdapter
MSTest.TestFramework
Newtonsoft.Json (if you plan to use it for JSON serialization/deserialization)

3.Organize Files: Place ApiClient.cs in a root folder (or ApiClient folder), BaseApiTest.cs in a Tests folder (or Base folder), and then each individual test class (AuthTests.cs, OrderTests.cs, etc.) in the Tests folder. Adjust namespaces accordingly.

4.Run Tests: Build your project. Then, open the Test Explorer window in Visual Studio (Test > Test Explorer) or run dotnet test from your project directory in the terminal. You should see the individual test methods listed under their respective classes.

Project Structure (Conceptual)
To support individual test classes, you might organize your project like this:

YourProjectName/
├── Models/                    
│   ├── LoginRequest.cs
│   ├── OrderResponse.cs
│   └── ...
├── ApiClient/
│   └── ApiClient.cs           
├── Tests/
│   ├── BaseApiTest.cs          
│   ├── EnergyTests.cs         
│   ├── OrderTests.cs          
│   ├── AuthTests.cs            
│   └── FuelTests.cs           
└── appsettings.json            
