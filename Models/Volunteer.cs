using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace GiftOfGivers_WebApplication.Models
{
    public class Volunteer
    {
        [Key]
        public int VolunteerID { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required")]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; } = string.Empty;

        [Display(Name = "Address")]
        public string? Address { get; set; }

        [Display(Name = "City")]
        public string? City { get; set; }

        [Display(Name = "Skills/Qualifications")]
        [DataType(DataType.MultilineText)]
        public string? Skills { get; set; }

        [Display(Name = "Available Days")]
        public string? AvailableDays { get; set; }

        [Display(Name = "Availability Status")]
        public string Status { get; set; } = "Available";

        [Display(Name = "Registration Date")]
        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        [Display(Name = "Notes")]
        [DataType(DataType.MultilineText)]
        public string? Notes { get; set; }

        // Computed property
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
    }

    public class VolunteerTask
    {
        [Key]
        public int TaskID { get; set; }

        [Required(ErrorMessage = "Task title is required")]
        [Display(Name = "Task Title")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Required Skills")]
        public string? RequiredSkills { get; set; }

        [Display(Name = "Priority")]
        public string Priority { get; set; } = "Medium";

        [Display(Name = "Status")]
        public string Status { get; set; } = "Open";

        [Display(Name = "Location")]
        public string? Location { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.DateTime)]
        public DateTime? StartDate { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.DateTime)]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Volunteers Needed")]
        public int VolunteersNeeded { get; set; } = 1;
    }

    public class VolunteerAssignment
    {
        [Key]
        public int AssignmentID { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; } = "Assigned";

        [Display(Name = "Hours Contributed")]
        public decimal? HoursContributed { get; set; }

        [Display(Name = "Assigned Date")]
        public DateTime AssignedDate { get; set; } = DateTime.Now;

        [Display(Name = "Completed Date")]
        public DateTime? CompletedDate { get; set; }

        [Display(Name = "Notes")]
        [DataType(DataType.MultilineText)]
        public string? Notes { get; set; }
    }
}

