using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangment.DAL.Models
{
    public class Member : GymUser
    {
        public string? Photo { get; set; }
        //JoinDate = CreateAt 
        #region RelationShips
        public HealthRecord HealthRecord { get; set; } = default!;
        public MemberShip MemberShip { get; set; }
        #endregion
    }
}
