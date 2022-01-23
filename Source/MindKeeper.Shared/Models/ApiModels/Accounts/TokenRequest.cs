using System.ComponentModel.DataAnnotations;

namespace MindKeeper.Shared.Models.ApiModels.Accounts
{
    public class TokenRequest
    {
        [Required(AllowEmptyStrings = false)]
        public string Username { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Password { get; set; }
    }
}
