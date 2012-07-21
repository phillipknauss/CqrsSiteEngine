using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System;

namespace UserInterface.Models
{
    
    public class UserModel
    {
        [Editable(false)]
        public Guid Id { get; set; }
                
        [Display(Name = "Username")]
        [DataType(DataType.Text)]
        [MinLength(4)]
        public string Username { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "First Name")]
        [MinLength(1)]
        public string FirstName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Last Name")]
        [MinLength(1)]
        public string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Admin")]
        public bool IsAdmin { get; set; }

        public static UserModel FromReadModel(ReadModel.UserIndexItem item)
        {
            return new UserModel()
            {
                Id = item.Id,
                Username = item.Username,
                FirstName = item.FirstName,
                LastName = item.LastName,
                Email = item.Email,
                IsAdmin = item.IsAdmin
            };
        }
    }
}
