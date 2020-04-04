using RE.Client.Core;
using System;
using System.ComponentModel.DataAnnotations;

namespace RE.Client.Entities
{
    public class Employee: BaseEntity
    {
         
        [Required]
        [StringLength(100)]
        [Display(Name ="Full Name")]
        public string FullName { get; set; }

        [Display(Name = "Date of birth")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "Salary expectation")]
        [Required]
        public decimal? SalaryExpectation { get; set; }

    }
}
