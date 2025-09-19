using Microsoft.AspNetCore.Mvc;
using Azure.Data.Tables;
using ProfileRegistratrionWithAiUsage.Models;
using System.Text.Json;

//fully utilised Claude Sonnet 4 (Anthropic) to generate this API controller
namespace ProfileRegistratrionWithAiUsage.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileApiController : ControllerBase
    {
        private readonly ILogger<ProfileApiController> _logger;
        private readonly TableServiceClient _tableServiceClient;

        public ProfileApiController(ILogger<ProfileApiController> logger, TableServiceClient tableServiceClient)
        {
            _logger = logger;
            _tableServiceClient = tableServiceClient;
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok(new { 
                success = true, 
                message = "API is working!",
                timestamp = DateTime.UtcNow
            });
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveProfile([FromBody] Profile profile)
        {
            try
            {
                _logger.LogInformation("SaveProfile API called");

                if (profile == null)
                {
                    return BadRequest(new { 
                        success = false, 
                        message = "Invalid profile data" 
                    });
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
                return Ok(new { 
                    success = true, 
                    message = "Profile saved successfully",
                    rowKey = tableEntity.RowKey
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while saving profile");
                
                return StatusCode(500, new { 
                    success = false, 
                    message = "An error occurred while saving the profile",
                    error = ex.Message
                });
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllProfiles()
        {
            try
            {
                _logger.LogInformation("GetAllProfiles API called");

                const string tableName = "Profiles";
                var tableClient = _tableServiceClient.GetTableClient(tableName);
                
                // Query all profiles (will return empty if table doesn't exist)
                var profiles = new List<ProfileTableEntity>();
                try
                {
                    await foreach (var profile in tableClient.QueryAsync<ProfileTableEntity>())
                    {
                        profiles.Add(profile);
                    }
                }
                catch (Azure.RequestFailedException ex) when (ex.Status == 404)
                {
                    _logger.LogWarning("Profiles table does not exist yet");
                    profiles = new List<ProfileTableEntity>();
                }

                _logger.LogInformation($"Retrieved {profiles.Count} profiles from table storage");

                // Return success response with profiles
                return Ok(new { 
                    success = true, 
                    message = "Profiles retrieved successfully",
                    count = profiles.Count,
                    profiles = profiles
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving profiles");
                
                return StatusCode(500, new { 
                    success = false, 
                    message = "An error occurred while retrieving profiles",
                    error = ex.Message
                });
            }
        }
    }
}
