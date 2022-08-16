using ApiMedicalClinicEx.Server.Context;
using ApiMedicalClinicEx.Server.Context.Model;
using ApiMedicalClinicEx.Server.Services.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ApiMedicalClinicEx.Server.Services;

/// <summary>
/// Manage patients
/// </summary>
public interface IPatientService
{
    /// <summary>
    /// Get list of patients
    /// </summary>
    /// <returns>list of patients</returns>
    Task<IEnumerable<Patient>> GetPatientsAsync();

    /// <summary>
    /// Get unique patient or null
    /// </summary>
    /// <returns>patient or null value</returns>
    Task<Patient?> GetPatientsAsync(string cpf);

    /// <summary>
    /// Add new patient
    /// </summary>
    /// <param name="patient">object to add</param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ConflictException"></exception>
    Task AddPatientAsync(Patient patient);

    /// <summary>
    /// remove existing patient
    /// </summary>
    /// <param name="idPatient">identifier</param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotFoundException"></exception>
    Task RemovePatientAsync(string idPatient);

    /// <summary>
    /// update patient
    /// </summary>
    /// <param name="idPatient">patient identifier</param>
    /// <param name="patient">object to update</param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotFoundException"></exception>
    Task UpdatePatientAsync(string idPatient, Patient patient);

    /// <summary>
    /// Add new allergy to patient
    /// </summary>
    /// <param name="idPatient">patient id</param>
    /// <param name="allergy">allergy to patient</param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotFoundException"></exception>
    Task AddAllergyPatientAsync(string idPatient, PatientAllergy allergy);

    /// <summary>
    /// Remove allergy from patient
    /// </summary>
    /// <param name="idPatient">patient id</param>
    /// <param name="desc">description allergy</param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotFoundException"></exception>
    Task RemoveAllergyPatientAsync(string idPatient, string desc);

    /// <summary>
    /// Remove allergy from patient
    /// </summary>
    /// <param name="idPatient">patient id</param>
    /// <param name="idAllergy">id allergy</param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotFoundException"></exception>
    Task RemoveAllergyPatientAsync(string idPatient, int idAllergy);

    /// <summary>
    /// get allergys from patient
    /// </summary>
    /// <param name="idPatient">patient identifier</param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotFoundException"></exception>
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

    /// <inheritdoc/>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotFoundException"></exception>
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

    /// <inheritdoc/>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ConflictException"></exception>
    public async Task AddPatientAsync(Patient patient)
    {
        if (patient is null)
            throw new ArgumentException("Arguments null or emptys.");

        if ((await TryGetPatient(patient.Cpf!)).hasPatient)
            throw new ConflictException($"Paciente {patient.Cpf} já existe.");

        string? validBlood = null;
        if (patient.BloodType is not null &&
            !_bloodTypesService.IsValidBloodType(patient.BloodType, out validBlood))
            throw new BusinessRulesException($"Tipo de sangue inválido!");
        if (validBlood is not null)
            patient.BloodType = validBlood;

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

    /// <inheritdoc/>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotFoundException"></exception>
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

    /// <inheritdoc/>
    public async Task<IEnumerable<Patient>> GetPatientsAsync()
    {
        return await _context.Patients.AsNoTracking().ToListAsync();

    }

    /// <inheritdoc/>
    /// <param name="cpf"></param>
    /// <returns></returns>
    public async Task<Patient?> GetPatientsAsync(string cpf)
    {
        var response = await TryGetPatient(cpf);

        return response.patient;
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotFoundException"></exception>
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

    /// <inheritdoc/>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotFoundException"></exception>
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

    /// <inheritdoc/>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotFoundException"></exception>
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

    /// <inheritdoc/>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotFoundException"></exception>
    public async Task UpdatePatientAsync(string idPatient, Patient patient)
    {
        if (string.IsNullOrEmpty(idPatient) || string.IsNullOrEmpty(patient.Cpf) || idPatient != patient.Cpf)
            throw new ArgumentException("Arguments null, emptys or invalids.");

        var patientDb = (await TryGetPatient(idPatient)).patient;

        if (patientDb is null)
            throw new NotFoundException($"Paciente com Id {idPatient} não encontrado.");

        string? validBlood = null;
        if (patient.BloodType is not null &&
            !_bloodTypesService.IsValidBloodType(patient.BloodType, out validBlood))
            throw new BusinessRulesException($"Tipo de sangue inválido!");
        if (validBlood is not null)
            patient.BloodType = validBlood;

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