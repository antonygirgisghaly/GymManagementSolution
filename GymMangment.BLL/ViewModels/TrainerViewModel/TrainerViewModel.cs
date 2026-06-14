using GymMangment.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangment.BLL.ViewModels.TrainerViewModel
{
    public class TrainerViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public string Specialization { get; set; } = default!;
        public DateOnly DateOfBirth { get; set; }

        //Trainer Details
        public string? Details { get; set; }
        public string? Edit { get; set; }
        public string? Delete { get; set; }
        public int BuildingNumber { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public Address Address { get; set; }
    }
}
