using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace GulDiyet.Core.Application.ViewModels.Patients
{
    public class PatientViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string IdNumber { get; set; }
        public DateTime DateBirth { get; set; }
        public bool IsSmoker { get; set; }
        public bool HasAllergies { get; set; }
        public string ImageUrl { get; set; }
    }
}
