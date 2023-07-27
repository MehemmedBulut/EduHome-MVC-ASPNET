using System.ComponentModel.DataAnnotations;

namespace Eduprob.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Bu xana boş ola bilməz!")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Bu xana boş ola bilməz!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool IsRemember { get; set; }

    }
}
