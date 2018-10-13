using System;
using System.Collections.Generic;

namespace IRunes.Models
{
    public class User : BaseModel<string>
    {
        public User()
        {
            this.Id = new Guid().ToString();
            this.Albums = new List<Album>();
        }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public virtual ICollection<Album> Albums { get; set; }
    }
}
