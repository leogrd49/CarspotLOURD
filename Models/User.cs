using System;
using Postgrest.Attributes;
using Postgrest.Models;

namespace CarspotLourd.Models
{
    [Table("users")]
    public class User : BaseModel
    {
        [PrimaryKey("id")]
        public Guid Id { get; set; }

        [Column("username")]
        public string Username { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("profile_pic_url")]
        public string ProfilePicUrl { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("bio")]
        public string Bio { get; set; }

        [Column("location")]
        public string Location { get; set; }
    }
}