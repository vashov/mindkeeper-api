using System.ComponentModel.DataAnnotations;

namespace MindKeeper.Shared.Models.ApiModels.Accounts
{
    public class RegistrationRequest
    {
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Password { get; set; }
    }
}
