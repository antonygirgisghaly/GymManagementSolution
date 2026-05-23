using GymMangment.DAL.Models;

namespace GymMangement.Models
{
    public class Plan : BaseEntity
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public int DuirationDays { get; set; }
        public decimal Price { get; set; }
    }
}
