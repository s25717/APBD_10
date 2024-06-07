namespace APBD_10.DTO
{
    public class PrescriptionRequestDto
    {
        public int IdPatient { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        public int IdDoctor { get; set; }
        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }
        public List<PrescriptionMedicamentDto> Medicaments { get; set; }
    }
}
