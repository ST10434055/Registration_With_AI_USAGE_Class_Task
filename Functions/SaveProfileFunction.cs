using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using Azure.Data.Tables;
using ProfileRegistratrionWithAiUsage.Models;
using System.Text.Json;

//fully utilised Claude Sonnet 4 (Anthropic) to generate this Azure Function
namespace ProfileRegistratrionWithAiUsage.Functions
{
    public class SaveProfileFunction
    {
        private readonly ILogger<SaveProfileFunction> _logger;
        private readonly TableServiceClient _tableServiceClient;

        public SaveProfileFunction(ILogger<SaveProfileFunction> logger, TableServiceClient tableServiceClient)
        {
            _logger = logger;
            _tableServiceClient = tableServiceClient;
        }

        [Function("SaveProfile")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "profile/save")] HttpRequestData req)
        {
            try
            {
                _logger.LogInformation("SaveProfile Azure Function triggered");

                // Read the request body
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                
                if (string.IsNullOrEmpty(requestBody))
                {
                    var errorResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                    await errorResponse.WriteStringAsync(JsonSerializer.Serialize(new { 
                        success = false, 
                        message = "Request body is empty" 
                    }));
                    return errorResponse;
                }

                // Deserialize the profile data
                var profile = JsonSerializer.Deserialize<Profile>(requestBody, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (profile == null)
                {
                    var errorResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                    await errorResponse.WriteStringAsync(JsonSerializer.Serialize(new { 
                        success = false, 
                        message = "Invalid profile data" 
                    }));
                    return errorResponse;
                }

                // Create table entity from Profile
                var tableEntity = new ProfileTableEntity(profile)
                {
                    RowKey = Guid.NewGuid().ToString()
                };

                // Get or create the table
                const string tableName = "Profiles";
                var tableClient = _tableServiceClient.GetTableClient(tableName);
                await tableClient.CreateIfNotExistsAsync();

                // Insert the entity
                await tableClient.AddEntityAsync(tableEntity);

                _logger.LogInformation($"Profile saved successfully with RowKey: {tableEntity.RowKey}");

                // Return success response
                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteStringAsync(JsonSerializer.Serialize(new { 
                    success = true, 
                    message = "Profile saved successfully",
                    rowKey = tableEntity.RowKey
                }));

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while saving profile");
                
                var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
                await errorResponse.WriteStringAsync(JsonSerializer.Serialize(new { 
                    success = false, 
                    message = "An error occurred while saving the profile",
                    error = ex.Message
                }));

                return errorResponse;
            }
        }
    }
}