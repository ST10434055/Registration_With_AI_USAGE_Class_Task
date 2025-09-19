using Microsoft.AspNetCore.Mvc;
using Azure.Data.Tables;
using ProfileRegistratrionWithAiUsage.Models;

//fully utilised Claude Sonnet 4 (Anthropic) to generate this controller
namespace ProfileRegistratrionWithAiUsage.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ILogger<ProfileController> _logger;
        private readonly TableServiceClient _tableServiceClient;

        public ProfileController(ILogger<ProfileController> logger, TableServiceClient tableServiceClient)
        {
            _logger = logger;
            _tableServiceClient = tableServiceClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Profile profile)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(profile);
                }

                _logger.LogInformation("Profile registration form submitted");

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

                // Redirect to success page or show success message
                TempData["SuccessMessage"] = "Profile registered successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while saving profile");
                ModelState.AddModelError("", "An error occurred while saving the profile. Please try again.");
                return View(profile);
            }
        }

        [HttpGet]
        public async Task<IActionResult> AllProfiles()
        {
            try
            {
                _logger.LogInformation("Retrieving all profiles from Azure Table Storage");

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
                    return View(new List<ProfileTableEntity>());
                }

                _logger.LogInformation($"Retrieved {profiles.Count} profiles from table storage");
                return View(profiles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving profiles");
                ModelState.AddModelError("", "An error occurred while retrieving profiles. Please try again.");
                return View(new List<ProfileTableEntity>());
            }
        }
    }
}