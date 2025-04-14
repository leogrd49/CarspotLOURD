﻿using System;
using Postgrest.Attributes;
using Postgrest.Models;

namespace CarspotLourd.Models
{
    [Table("spot_shares")]
    public class SpotShare : BaseModel
    {
        [PrimaryKey("id")]
        public Guid Id { get; set; }

        [Column("user_id")]
        public Guid? UserId { get; set; }

        [Column("spot_id")]
        public Guid? SpotId { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}