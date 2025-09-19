using Azure;
using Azure.Data.Tables;

//modified AI generated code to inherit from Profile class initial was duplicating code
namespace ProfileRegistratrionWithAiUsage.Models
{
    public class ProfileTableEntity : Profile, ITableEntity
    {
        public string PartitionKey { get; set; } = "Profile";
        public string RowKey { get; set; } = Guid.NewGuid().ToString();
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Constructor to initialize from base Profile
        public ProfileTableEntity() : base()
        {
        }

        // Constructor to initialize from Profile object
        public ProfileTableEntity(Profile profile) : base()
        {
            Name = profile.Name;
            Surname = profile.Surname;
            Email = profile.Email;
            Age = profile.Age;
        }
    }
}
