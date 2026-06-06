using GymMangment.DAL.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangment.DAL.Data.Models
{
    public class Trainer : GymUser
    {
        public Specialty Specialty { get; set; }
        //HireDate = CreatedAt at GymUser
        public ICollection<Session> Sessions { get; set; } = default!;
    }
}
