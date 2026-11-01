using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RaceDay.APIx.Models
{
    public class AppUser
    {
        public int UserID { get; set; }
        public string Role {  get; set; } //"Organiser" or "Participant"
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public string PasswordHash { get; set; }
    }
}