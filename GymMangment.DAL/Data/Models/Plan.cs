namespace GymMangment.DAL.Data.Models
{
    public class Plan : BaseEntity
    {
        public string Name { get; set; } = default!;
        public bool IsActive { get; set; }
        public string Description { get; set; } = default!;
        public int DuirationDays { get; set; }
        public decimal Price { get; set; }
        public ICollection<MemberShip> PlanMembers { get; set; } = default!;
    }
}
