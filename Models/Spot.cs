using System;
using Postgrest.Attributes;
using Postgrest.Models;

namespace CarspotLourd.Models
{
    [Table("spots")]
    public class Spot : BaseModel
    {
        [PrimaryKey("id")]
        public Guid Id { get; set; }

        [Column("user_id")]
        public Guid UserId { get; set; }

        [Column("model_id")]
        public long ModelId { get; set; }

        [Column("image_url")]
        public string ImageUrl { get; set; }

        [Column("location")]
        public string Location { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("latitude")]
        public double? Latitude { get; set; }

        [Column("longitude")]
        public double? Longitude { get; set; }
    }
}