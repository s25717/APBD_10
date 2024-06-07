using APBD_10.DTO;
using APBD_10.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APBD_10.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddPrescription([FromBody] PrescriptionRequestDto request)
        {
            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.IdPatient == request.IdPatient);

            if (patient == null)
            {
                patient = new Patient
                {
                    FirstName = request.PatientFirstName,
                    LastName = request.PatientLastName
                };
                _context.Patients.Add(patient);
                await _context.SaveChangesAsync();
            }

            if (request.Medicaments.Count > 10)
            {
                return BadRequest("A prescription can include a maximum of 10 medications.");
            }

            foreach (var med in request.Medicaments)
            {
                var medicament = await _context.Medicaments.FindAsync(med.IdMedicament);
                if (medicament == null)
                {
                    return BadRequest($"Medicament with ID {med.IdMedicament} does not exist.");
                }
            }

            var prescription = new Prescription
            {
                Date = request.Date,
                DueDate = request.DueDate,
                IdPatient = patient.IdPatient,
                IdDoctor = request.IdDoctor,
                PrescriptionMedicaments = request.Medicaments.Select(m => new PrescriptionMedicament
                {
                    IdMedicament = m.IdMedicament,
                    Dose = m.Dose,
                    Description = m.Description
                }).ToList()
            };

            _context.Prescriptions.Add(prescription);
            await _context.SaveChangesAsync();

            return Ok("Prescription added successfully.");
        }

        [HttpGet("{idPatient}")]
        public async Task<IActionResult> GetPatientData(int idPatient)
        {
            var patient = await _context.Patients
                .Include(p => p.Prescriptions)
                    .ThenInclude(pr => pr.PrescriptionMedicaments)
                        .ThenInclude(pm => pm.Medicament)
                .Include(p => p.Prescriptions)
                    .ThenInclude(pr => pr.Doctor)
                .FirstOrDefaultAsync(p => p.IdPatient == idPatient);

            if (patient == null)
            {
                return NotFound("Patient not found.");
            }

            var response = new PatientResponseDto
            {
                IdPatient = patient.IdPatient,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Prescriptions = patient.Prescriptions
                    .OrderBy(pr => pr.DueDate)
                    .Select(pr => new PrescriptionDto
                    {
                        IdPrescription = pr.IdPrescription,
                        Date = pr.Date,
                        DueDate = pr.DueDate,
                        Medicaments = pr.PrescriptionMedicaments
                            .Select(pm => new MedicamentDto
                            {
                                IdMedicament = pm.Medicament.IdMedicament,
                                Name = pm.Medicament.Name,
                                Description = pm.Medicament.Description
                            }).ToList(),
                        Doctor = new DoctorDto
                        {
                            IdDoctor = pr.Doctor.IdDoctor,
                            FirstName = pr.Doctor.FirstName,
                            LastName = pr.Doctor.LastName
                        }
                    }).ToList()
            };

            return Ok(response);
        }
    }
}
