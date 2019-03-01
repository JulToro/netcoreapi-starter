using System;
using System.ComponentModel.DataAnnotations;

namespace netcore.api.Models
{
    public class UserDto
    {
        public UserDto()
        {

        }
        public Guid Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }
        [Required]
        [MaxLength(30)]
        public string Nit { get; set; }
        public DateTime BirthDay { get; set; }
        [MaxLength(100)]
        public string Email { get; set; }
    }
}
