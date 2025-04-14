using System;
using Postgrest.Attributes;
using Postgrest.Models;

namespace CarspotLourd.Models
{
    [Table("user_collections")]
    public class UserCollection : BaseModel
    {
        [PrimaryKey("id")]
        public Guid Id { get; set; }

        [Column("user_id")]
        public Guid? UserId { get; set; }

        [Column("model_id")]
        public long? ModelId { get; set; }

        [Column("spotted")]
        public bool Spotted { get; set; }

        [Column("spotted_date")]
        public DateTime? SpottedDate { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("location")]
        public string? Location { get; set; }

        [Column("latitude")]
        public double? Latitude { get; set; }

        [Column("longitude")]
        public double? Longitude { get; set; }

        [Column("image_url")]
        public string? ImageUrl { get; set; }

        [Column("is_public")]
        public bool IsPublic { get; set; }

        [Column("image2_url")]
        public string? Image2Url { get; set; }

        [Column("image3_url")]
        public string? Image3Url { get; set; }

        [Column("superspot")]
        public bool Superspot { get; set; }

        [Column("warn")]
        public int Warn { get; set; }
    }
}