using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Pulpa.TrainerSafety.Data.Entities
{
    public class UserTrainerSafety : IdentityUser
    {
        [MaxLength(255)]
        public string FirstName { get; set; }
        [MaxLength(255)]
        public string LastName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
    }



}
