using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangment.BLL.ViewModels.PlanViewModel
{
    public class PlanViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public int DuirationDays { get; set; }
        public decimal Price { get; set; }
    }
}
