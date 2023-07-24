using System.Collections.Generic;

namespace DataManager.Models
{
    public class ApplicationUserModel
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public Dictionary<string, string> Roles { get; set; } = new Dictionary<string, string>();
    }
}