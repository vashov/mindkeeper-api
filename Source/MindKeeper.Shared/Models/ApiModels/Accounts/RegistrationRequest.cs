using System.ComponentModel.DataAnnotations;

namespace MindKeeper.Shared.Models.ApiModels.Accounts
{
    public class RegistrationRequest
    {
        [Required(AllowEmptyStrings = false)]
        public string Mail { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Password { get; set; }
    }
}
