# AI Usage Declaration

## Xavier's Contributions

### Perplexity AI Usage

- **Tool**: Perplexity AI
- **Purpose**: Seeking simpler implementation approaches and guidance
- **Approach**: Asked for thorough explanations and verified understanding before implementation
- **Links**:
  - [ProfileEntity Table Implementation](https://www.perplexity.ai/search/public-class-profileentity-tab-..wMmh2iS_iFhMgEnnoguA)
  - [Project Guidance and UI Generation](https://www.perplexity.ai/search/hi-perplexity-im-doing-a-proje-0LNS_kLBTSSQ5dLAzTjVSA)

**Note**: Xavier ensured complete understanding of all code before implementation, not accepting solutions blindly.

---

## Pieter's Contributions

### 1. Azure Function Development

**AI Tool**: Claude Sonnet 4 (Anthropic)

**Generated Components**:

- `ProfileTableEntity` class with `ITableEntity` implementation
- `SaveProfileFunction` Azure Function with HTTP trigger
- Updated `Program.cs` with Azure Functions and Table Storage configuration
- Created `host.json` and `local.settings.json` configuration files
- Created `ProfileTestClient` for testing the Azure Function
- Updated project file with necessary NuGet packages for Azure Functions and Table Storage

**Approach**: Generated code based on understanding of Azure Functions architecture and Azure Table Storage requirements. No external web searches performed.

### 2. Profile Viewing Functionality

**AI Tool**: Claude Sonnet 4 (Anthropic)

**Generated Components**:

- `AllProfiles` action method in `ProfileController` to retrieve all profiles from Azure Table Storage
- `AllProfiles.cshtml` view with responsive Bootstrap table design
- Updated navigation in `_Layout.cshtml` to include "View Profiles" link
- Professional UI with Font Awesome icons, responsive design, and empty state handling
- Profile details display including Name, Surname, Email, Age, Created Date, and Profile ID
- Error handling and logging for profile retrieval operations

**Approach**: Generated code based on understanding of ASP.NET Core MVC, Bootstrap, and Azure Table Storage querying. No external web searches performed.

### 3. Code Refactoring

**AI Tool**: Claude Sonnet 4 (Anthropic)

**Refactored Components**:

- Updated `ProfileTableEntity.cs` to inherit from `Profile.cs` instead of duplicating properties
- Added constructors to `ProfileTableEntity` for easy initialization from `Profile` objects
- Updated `Profile.cs` to use non-nullable string properties with default empty string values
- Modified `ProfileController.cs` and `ProfileApiController.cs` to use new constructor pattern
- Updated `AllProfiles.cshtml` view to handle non-nullable string properties properly
- Improved code maintainability and reduced duplication through inheritance

**Approach**: Refactoring based on understanding of C# inheritance, object-oriented principles, and ASP.NET Core MVC. No external web searches performed.

### 4. Azure Functions Implementation

**AI Tool**: Claude Sonnet 4 (Anthropic)

**Generated Components**:

- `SaveProfileFunction.cs` - Azure Function for saving profiles to Azure Table Storage
- `GetAllProfilesFunction.cs` - Azure Function for retrieving all profiles from Azure Table Storage
- Updated `ProfileController.cs` to call Azure Functions via HTTP instead of direct Table Storage access
- Created `ProfileApiController.cs` for additional API endpoints alongside Azure Functions
- Updated `Program.cs` to configure HttpClient and Azure Functions Worker
- Updated `appsettings.json` with Azure Functions configuration and connection strings
- Implemented proper error handling and JSON serialization for Azure Function calls
- Maintained both Azure Functions and API controller approaches for flexibility

**Approach**: Azure Functions implementation based on understanding of Azure Functions architecture and HTTP client patterns. No external web searches performed.

---

## Summary

This project utilized AI tools responsibly with the following principles:

- **Understanding First**: All generated code was thoroughly reviewed and understood before implementation
- **Knowledge-Based**: AI tools were used based on existing knowledge rather than external searches
- **Transparency**: All AI usage is documented with specific tools and purposes
- **Quality Assurance**: Code was generated with proper architecture and best practices in mind

**Total AI Tools Used**:

- Claude Sonnet 4 (Anthropic) - Primary development tool
- Perplexity AI - Guidance and alternative approaches
