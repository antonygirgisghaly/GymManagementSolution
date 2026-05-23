using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangment.DAL.Models
{
    internal class Member : GymUser
    {
        public string? Photo { get; set; }
        //JoinDate = CreateAt 
    }
}
