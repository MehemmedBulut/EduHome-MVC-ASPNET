using System.ComponentModel.DataAnnotations;

namespace Eduprob.ViewModels
{
    public class ResetPasswordVM
    {
        [Required(ErrorMessage = "Bu xana boş ola bilməz!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Bu xana boş ola bilməz!")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Please add same password")]
        public string CheckPassword { get; set; }
    }
}
