using System;
using System.Collections.Generic;
namespace TheWorld.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class ContactViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
