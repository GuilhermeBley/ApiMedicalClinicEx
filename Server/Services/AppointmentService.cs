using ApiMedicalClinicEx.Server.Context;
using ApiMedicalClinicEx.Server.Context.Model;
using ApiMedicalClinicEx.Server.Services.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ApiMedicalClinicEx.Server.Services;

/// <summary>
/// Manage appointments
/// </summary>
public interface IAppointmentService
{
    /// <summary>
    /// Get doctor Appointments
    /// </summary>
    /// <param name="idDoctor">identifier doctor</param>
    /// <returns>list of appointment</returns>
    Task<IEnumerable<Appointment>> GetAppointmentsDoctorAsync(string idDoctor);

    /// <summary>
    /// Get patient Appointments
    /// </summary>
    /// <param name="patient">identifier patient</param>
    /// <returns>list of appointment</returns>
    Task<IEnumerable<Appointment>> GetAppointmentsPatientAsync(string patient);

    /// <summary>
    /// Get Appointments per date
    /// </summary>
    /// <param name="date">after date</param>
    /// <returns>list of appointment</returns>
    Task<IEnumerable<Appointment>> GetAppointmentsAfterDateAsync(DateTime date);

    /// <summary>
    /// add new appointment
    /// </summary>
    /// <param name="appointment">object context to add</param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="BusinessRulesException"></exception>
    /// <exception cref="NotFoundException"></exception>
    Task AddAppointmentAsync(Appointment appointment);

    /// <summary>
    /// remove appointment
    /// </summary>
    /// <param name="idAppointment">identifier</param>
    Task RemoveAppointmentAsync(int idAppointment);

    /// <summary>
    /// update appointment
    /// </summary>
    /// <param name="idAppointment">id</param>
    /// <param name="appointment">object</param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="BusinessRulesException"></exception>
    /// <exception cref="NotFoundException"></exception>
    Task UpdateAppointmentAsync(int idAppointment, Appointment appointment);
}

public class AppointmentService : IAppointmentService
{
    private readonly AppClinicContext _context;

    public AppointmentService(AppClinicContext context)
    {
        _context = context;
    }

    /// <inheritdoc/>
    public async Task AddAppointmentAsync(Appointment appointment)
    {
        await ValidAppointment(appointment);

        using var transaction = _context.Database.BeginTransaction();
        try
        {
            _context.Appointments.Add(appointment);

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
    public async Task<IEnumerable<Appointment>> GetAppointmentsAfterDateAsync(DateTime date)
    {
        return (await _context.Appointments.AsNoTracking().ToListAsync()).Where(appointment => appointment.DateAppo < date);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Appointment>> GetAppointmentsDoctorAsync(string idDoctor)
    {
        return (await _context.Appointments.AsNoTracking().ToListAsync()).Where(appointment => appointment.Medic.Equals(idDoctor));
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Appointment>> GetAppointmentsPatientAsync(string patient)
    {
        return (await _context.Appointments.AsNoTracking().ToListAsync()).Where(appointment => appointment.Patient!.Equals(patient));
    }

    /// <inheritdoc/>
    /// <exception cref="NotFoundException"></exception>
    public async Task RemoveAppointmentAsync(int idAppointment)
    {
        var appointment = await _context.Appointments.FirstOrDefaultAsync(f => f.Id == idAppointment);

        if (appointment is null)
            throw new NotFoundException($"Id {idAppointment}");

        using var transaction = _context.Database.BeginTransaction();
        try
        {
            _context.Entry(appointment).State = EntityState.Deleted;

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
    public async Task UpdateAppointmentAsync(int idAppointment, Appointment appointment)
    {
        if (appointment is null || appointment.Id != idAppointment)
            throw new ArgumentException("Id's passados são inválidos.");
        
        await ValidAppointment(appointment);

        var appointmentDb = await _context.Appointments.FirstOrDefaultAsync(f => f.Id == idAppointment);

        if (appointmentDb is null)
            throw new NotFoundException($"Id {idAppointment}");

        using var transaction = _context.Database.BeginTransaction();
        try
        {
            _context.Entry(appointmentDb).CurrentValues.SetValues(appointment);
            _context.Entry(appointment).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            transaction.Commit();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    /// <summary>
    /// async validation appointments
    /// </summary>
    /// <param name="appointment">appointment to validation</param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="BusinessRulesException"></exception>
    /// <exception cref="NotFoundException"></exception>
    private async Task ValidAppointment(Appointment appointment)
    {
        if (appointment is null)
            throw new ArgumentException();

        if (appointment.DateAppo < DateTime.Now)
        {
            throw new BusinessRulesException($"Data {appointment.DateAppo.ToString("dd/MM/yyyy hh:mm")} inválida. Data deve ser menor que o dia e horário atual.");
        }

        if (await _context.Users.FirstOrDefaultAsync(f => f.Id.Equals(appointment.Medic)) is null)
        {
            throw new NotFoundException($"Médico com identificador {appointment.Medic} inexistente.");
        }

        if (await _context.Patients.FirstOrDefaultAsync(f => f.Cpf!.Equals(appointment.Patient)) is null)
        {
            throw new NotFoundException($"Paciente com identificador {appointment.Medic} inexistente.");
        }

        if (appointment.LastAppo is not null &&
            (await _context.Appointments.FirstOrDefaultAsync(f => f.Id.Equals(appointment.LastAppo)) is null))
        {
            throw new NotFoundException($"Última consulta com identificador {appointment.LastAppo} inexistente.");
        }
    }
}