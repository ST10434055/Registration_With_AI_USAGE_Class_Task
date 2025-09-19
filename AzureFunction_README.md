# Azure Function for Profile Registration with AI Usage

This project includes an Azure Function that saves profile data to Azure Table Storage.

## Files Created/Modified

### New Files:

- `Models/ProfileTableEntity.cs` - Table entity class for Azure Table Storage
- `Functions/SaveProfileFunction.cs` - Azure Function with HTTP trigger
- `TestClient/ProfileTestClient.cs` - Test client for the Azure Function
- `host.json` - Azure Functions configuration
- `local.settings.json` - Local development settings

### Modified Files:

- `ProfileRegistratrionWithAiUsage.csproj` - Added Azure Functions and Table Storage packages
- `Program.cs` - Added Azure Functions and Table Storage configuration
- `AI_Declaration.txt` - Documented AI tools used

## Setup Instructions

1. **Install Dependencies:**

   ```bash
   dotnet restore
   ```

2. **Configure Azure Table Storage:**

   - Update the connection string in `appsettings.json` and `local.settings.json`
   - Replace `yourstorageaccount` and `yourkey` with your actual Azure Storage account details

3. **Run Locally:**

   ```bash
   dotnet run
   ```

4. **Deploy to Azure:**
   - Create an Azure Function App
   - Deploy the project to Azure
   - Configure the connection string in Azure Function App settings

## API Usage

### Endpoint

```
POST /api/profile/save
```

### Request Body

```json
{
  "name": "John",
  "surname": "Doe",
  "email": "john.doe@example.com",
  "age": "30"
}
```

### Response

```json
{
  "success": true,
  "message": "Profile saved successfully",
  "rowKey": "guid-here"
}
```

## Testing

Use the `ProfileTestClient` class to test the Azure Function:

```csharp
await ProfileTestClient.TestSaveProfile();
```

## AI Tools Used

- **Claude Sonnet 4 (Anthropic)**: Generated all Azure Function code and configuration
- **No external web searches**: Used existing knowledge of Azure services and .NET development

## Table Storage Structure

The data is stored in Azure Table Storage with the following structure:

- **Table Name**: Profiles
- **Partition Key**: "Profile" (static)
- **Row Key**: Auto-generated GUID
- **Properties**: Name, Surname, Email, Age, CreatedDate
