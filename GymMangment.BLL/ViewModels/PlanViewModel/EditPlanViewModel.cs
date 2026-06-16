using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangment.BLL.ViewModels.PlanViewModel
{
    public class EditPlanViewModel
    {
        public string Name { get; set; } = default!;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(200,MinimumLength = 5 , ErrorMessage = "Description must be between 5 and 200 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Duration in days is required")]
        [Range(1, 365, ErrorMessage = "Duration must be a positive integer between 1 and 365")]
        public int DurationDays { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be a positive number")]
        public decimal Price { get; set; }
    }
}
