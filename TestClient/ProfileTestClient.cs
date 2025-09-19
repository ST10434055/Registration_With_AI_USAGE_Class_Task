using System.Text;
using System.Text.Json;

namespace ProfileRegistratrionWithAiUsage.TestClient
{
    public class ProfileTestClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _functionUrl;

        public ProfileTestClient(string functionUrl)
        {
            _httpClient = new HttpClient();
            _functionUrl = functionUrl;
        }

        public async Task<bool> SaveProfileAsync(Models.Profile profile)
        {
            try
            {
                var json = JsonSerializer.Serialize(profile);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_functionUrl}/api/profile/save", content);
                
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Profile saved successfully: {responseContent}");
                    return true;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error saving profile: {errorContent}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return false;
            }
        }

        public static async Task TestSaveProfile()
        {
            // Replace with your actual Azure Function URL
            var functionUrl = "https://your-function-app.azurewebsites.net";
            var client = new ProfileTestClient(functionUrl);

            var testProfile = new Models.Profile
            {
                Name = "John",
                Surname = "Doe",
                Email = "john.doe@example.com",
                Age = "30"
            };

            await client.SaveProfileAsync(testProfile);
        }
    }
}
