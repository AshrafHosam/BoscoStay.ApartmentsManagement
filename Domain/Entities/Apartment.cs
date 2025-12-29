using Domain.Common;
using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Apartment : BaseEntity
    {
        [MaxLength(60)]
        public string Name { get; set; }

        [MaxLength(150)]
        public string Address { get; set; }

        [MaxLength(350)]
        public string Description { get; set; }

        public int Floor { get; set; }
        public NoiseLevelEnum NoiseLevel { get; set; }
        public double DistanceToCenterInKm { get; set; }
        public bool IsVisible { get; set; } = true;
        public double AreaInSquareMeters { get; set; }
        public bool IsFurnished { get; set; }
        public double PricePerDay { get; set; }
    }
}
