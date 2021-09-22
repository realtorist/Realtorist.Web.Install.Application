using System.ComponentModel.DataAnnotations;

namespace Realtorist.Web.Install.Application.Models
{
    /// <summary>
    /// Describes model for the basic installation of the Realtorist
    /// </summary>
    public class InstallModel
    {
        [Required]
        public string WebsiteName { get; set; }

        [Required]
        public string WebsiteAddress { get; set; }
        
        [Required]
        public string WebsiteTitle { get; set; }

        [Required]
        public string Timezone { get; set; }

        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
        
        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}