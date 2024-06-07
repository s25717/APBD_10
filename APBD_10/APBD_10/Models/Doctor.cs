using System.ComponentModel.DataAnnotations;

namespace APBD_10.Models
{
    public class Doctor
    {
        [Key] // Add this attribute to specify the primary key
        public int IdDoctor { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
