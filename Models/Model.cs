using System;
using Postgrest.Attributes;
using Postgrest.Models;

namespace CarspotLourd.Models
{
    [Table("models")]
    public class Model : BaseModel
    {
        [PrimaryKey("id")]
        public long Id { get; set; }

        [Column("brand_id")]
        public long? BrandId { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("price")]
        public decimal? Price { get; set; }

        [Column("engine")]
        public string? Engine { get; set; }

        [Column("horsepower")]
        public int? Horsepower { get; set; }

        [Column("torque")]
        public int? Torque { get; set; }

        [Column("curb_weight")]
        public int? CurbWeight { get; set; }

        [Column("acceleration_0_100")]
        public decimal? Acceleration0To100 { get; set; }

        [Column("top_speed")]
        public int? TopSpeed { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("phase")]
        public string? Phase { get; set; }

        [Column("series")]
        public string? Series { get; set; }

        [Column("commercial_start_date")]
        public string? CommercialStartDate { get; set; }

        [Column("commercial_end_date")]
        public string? CommercialEndDate { get; set; }

        [Column("generation_name")]
        public string? GenerationName { get; set; }

        [Column("micro_history")]
        public string? MicroHistory { get; set; }

        [Column("length")]
        public decimal? Length { get; set; }

        [Column("width")]
        public decimal? Width { get; set; }

        [Column("height")]
        public decimal? Height { get; set; }

        [Column("trunk_volume_min")]
        public int? TrunkVolumeMin { get; set; }

        [Column("trunk_volume_max")]
        public int? TrunkVolumeMax { get; set; }

        [Column("doors")]
        public int? Doors { get; set; }

        [Column("seats")]
        public int? Seats { get; set; }

        [Column("engine_type")]
        public string? EngineType { get; set; }

        [Column("fiscal_power")]
        public int? FiscalPower { get; set; }

        [Column("engine_description")]
        public string? EngineDescription { get; set; }

        [Column("displacement")]
        public int? Displacement { get; set; }

        [Column("horsepower_rpm")]
        public int? HorsepowerRpm { get; set; }

        [Column("transmission")]
        public string? Transmission { get; set; }

        [Column("transmission_gears")]
        public int? TransmissionGears { get; set; }

        [Column("drivetrain")]
        public string? Drivetrain { get; set; }

        [Column("fuel_consumption_avg")]
        public decimal? FuelConsumptionAvg { get; set; }

        [Column("fuel_tank_capacity")]
        public int? FuelTankCapacity { get; set; }

        [Column("co2_emission")]
        public decimal? Co2Emission { get; set; }

        [Column("brake_type")]
        public string? BrakeType { get; set; }

        [Column("trim")]
        public string? Trim { get; set; }

        [Column("variant")]
        public string? Variant { get; set; }

        [Column("rarity")]
        public string Rarity { get; set; }

        [Column("production_count")]
        public int? ProductionCount { get; set; }
    }
}