using GymMangment.DAL.Data.Models.Enums;
using Microsoft.Build.Tasks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangment.BLL.ViewModels.TrainerViewModel
{
    public class CreateTrainerViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name can only contain letters and spaces")]
        public string Name { get; set; } = default!;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = default!;

        [Required(ErrorMessage = "Phone is required")]
        [Phone(ErrorMessage = "Invalid phone format")]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "Phone number must be valid in Egyptian mobile number")]
        public string Phone { get; set; } = default!;

        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public Gender Gender { get; set; }
        //Address Information
        [Required(ErrorMessage = "Building Number is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Building Number must be a positive integer")]
        public int BuildingNumber { get; set; } = default!;

        [Required(ErrorMessage = "Street is required")]
        [StringLength(100,MinimumLength = 2, ErrorMessage = "Street cannot be longer than 100 characters")]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Street can only contain letters, numbers, and spaces")]
        public string Street { get; set; } = default!;

        [Required(ErrorMessage = "City is required")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "City cannot be longer than 150 characters")]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "City can only contain letters, numbers, and spaces")]
        public string City { get; set; } = default!;
        //profional information

        [Required(ErrorMessage = "Specialty is required")]
        [EnumDataType(typeof(Specialty), ErrorMessage = "Invalid specialty value")]
        public Specialty Specialties { get; set; }
    }
}
