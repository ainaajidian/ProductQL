using System;
using System.Collections.Generic;

#nullable disable

namespace ProductQL.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
