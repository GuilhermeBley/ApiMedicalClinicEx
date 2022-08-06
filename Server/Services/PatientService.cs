using ApiMedicalClinicEx.Server.Context;
using ApiMedicalClinicEx.Server.Context.Model;
using ApiMedicalClinicEx.Server.Services.Exceptions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApiMedicalClinicEx.Server.Services;

public interface IPatientService
{
    Task<IEnumerable<Patient>> GetPatientsAsync();
    Task<Patient?> GetPatientsAsync(string cpf);
    Task AddPatientAsync(Patient patient);
    Task RemovePatientAsync(string idPatient);
    Task UpdatePatientAsync(string idPatient, Patient patient);

    Task AddAllergyPatientAsync(string idPatient, PatientAllergy allergy);
    Task RemoveAllergyPatientAsync(string idPatient, string desc);
    Task RemoveAllergyPatientAsync(string idPatient, int idAllergy);
    Task<IEnumerable<PatientAllergy>> GetAllergysAsync(string idPatient);
}

public class PatientService : IPatientService
{
    private readonly AppClinicContext _context;
    private readonly IBloodTypesService _bloodTypesService;

    public PatientService(AppClinicContext context, IBloodTypesService bloodTypesService)
    {
        _context = context;
        _bloodTypesService = bloodTypesService;
    }

    public async Task AddAllergyPatientAsync(string idPatient, PatientAllergy allergy)
    {
        if (allergy is null || string.IsNullOrEmpty(idPatient) || string.IsNullOrEmpty(allergy.Cpf))
            throw new ArgumentException("Parameters null or emptys.");

        var resultPatient = await TryGetPatient(idPatient);

        if (!resultPatient.hasPatient)
            throw new NotFoundException($"Paciente {idPatient} não encontrado.");

        using var transaction = _context.Database.BeginTransaction();
        try
        {
            _context.PatientAllergys.Add(allergy);

            await _context.SaveChangesAsync();
            transaction.Commit();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }

    }

    public async Task AddPatientAsync(Patient patient)
    {
        if (patient is null)
            throw new ArgumentException("Arguments null or emptys.");

        if (!(await TryGetPatient(patient.Cpf!)).hasPatient)
            throw new ConflictException($"Paciente {patient.Cpf} já existe.");

        using var transaction = _context.Database.BeginTransaction();
        try
        {
            _context.Patients.Add(patient);

            await _context.SaveChangesAsync();
            transaction.Commit();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<IEnumerable<PatientAllergy>> GetAllergysAsync(string idPatient)
    {
        if (string.IsNullOrEmpty(idPatient))
            throw new ArgumentException("Arguments null or emptys.");

        if (!(await TryGetPatient(idPatient)).hasPatient)
        {
            throw new NotFoundException($"Paciente com Cpf {idPatient} não encontrado.");
        }

        return (await _context.PatientAllergys.AsNoTracking().ToListAsync()).Where(allergy => allergy.Cpf!.Equals(idPatient));
    }

    public async Task<IEnumerable<Patient>> GetPatientsAsync()
    {
        return await _context.Patients.AsNoTracking().ToListAsync();

    }

    public async Task<Patient?> GetPatientsAsync(string cpf)
    {
        var response = await TryGetPatient(cpf);

        return response.patient;
    }

    public async Task RemoveAllergyPatientAsync(string idPatient, string desc)
    {
        if (string.IsNullOrEmpty(idPatient) || string.IsNullOrEmpty(desc))
            throw new ArgumentException("Arguments null or emptys.");

        var allergy = await _context.PatientAllergys.AsNoTracking().FirstOrDefaultAsync(f => f.Cpf!.Equals(idPatient) && f.Desc!.Equals(desc));

        if (allergy is null)
            throw new NotFoundException($"Alegia de paciente com Id {idPatient} e descrição de {desc} não encontrada.");

        using var transaction = _context.Database.BeginTransaction();
        try
        {
            _context.Entry(allergy).State = EntityState.Deleted;

            await _context.SaveChangesAsync();
            transaction.Commit();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task RemoveAllergyPatientAsync(string idPatient, int idAllergy)
    {
        if (string.IsNullOrEmpty(idPatient) || idAllergy < 1)
            throw new ArgumentException("Arguments null or emptys.");

        var allergy = await _context.PatientAllergys.AsNoTracking().FirstOrDefaultAsync(f => f.Cpf!.Equals(idPatient) && f.Id!.Equals(idAllergy));

        if (allergy is null)
            throw new NotFoundException($"Alegia de paciente com Id de paciente {idPatient} e Id da alergia {idAllergy} não encontrado.");

        using var transaction = _context.Database.BeginTransaction();
        try
        {
            _context.Entry(allergy).State = EntityState.Deleted;

            await _context.SaveChangesAsync();
            transaction.Commit();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task RemovePatientAsync(string idPatient)
    {
        if (string.IsNullOrEmpty(idPatient))
            throw new ArgumentException("Arguments null or emptys.");

        if (string.IsNullOrEmpty(idPatient))
            throw new ArgumentException();

        var patient = (await TryGetPatient(idPatient)).patient;

        if (patient is null)
            throw new NotFoundException($"Paciente {patient} não encontrado.");

        using var transaction = _context.Database.BeginTransaction();
        try
        {
            _context.Entry(patient).State = EntityState.Deleted;

            await _context.SaveChangesAsync();
            transaction.Commit();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task UpdatePatientAsync(string idPatient, Patient patient)
    {
        if (string.IsNullOrEmpty(idPatient) || string.IsNullOrEmpty(patient.Cpf) || idPatient != patient.Cpf)
            throw new ArgumentException("Arguments null, emptys or invalids.");

        var patientDb = (await TryGetPatient(idPatient)).patient;

        if (patientDb is null)
            throw new NotFoundException($"Paciente com Id {idPatient} não encontrado.");

        using var transaction = _context.Database.BeginTransaction();
        try
        {
            _context.Entry(patientDb).CurrentValues.SetValues(patient);
            _context.Entry(patient).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            transaction.Commit();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    private async Task<(bool hasPatient, ApiMedicalClinicEx.Server.Context.Model.Patient? patient)> TryGetPatient(string idCpf)
    {
        var patient = await _context.Patients.AsNoTracking().FirstOrDefaultAsync(f => f.Cpf!.Equals(idCpf));

        if (patient is null)
            return (false, patient);

        return (true, patient);
    }
}