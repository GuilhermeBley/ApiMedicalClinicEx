using ApiMedicalClinicEx.Server.Context;
using ApiMedicalClinicEx.Server.Context.Model;
using ApiMedicalClinicEx.Server.Services.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ApiMedicalClinicEx.Server.Services;

public interface IAppointmentService
{
    Task<IEnumerable<Appointment>> GetAppointmentsDoctorAsync(string idDoctor);
    Task<IEnumerable<Appointment>> GetAppointmentsPatientAsync(string patient);
    Task<IEnumerable<Appointment>> GetAppointmentsAfterDateAsync(DateTime date);
    Task AddAppointmentAsync(Appointment appointment);
    Task RemoveAppointmentAsync(int idAppointment);
    Task UpdateAppointmentAsync(int idAppointment, Appointment appointment);
}

public class AppointmentService : IAppointmentService
{
    private readonly AppClinicContext _context;

    public AppointmentService(AppClinicContext context)
    {
        _context = context;
    }

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

    public async Task<IEnumerable<Appointment>> GetAppointmentsAfterDateAsync(DateTime date)
    {
        return (await _context.Appointments.AsNoTracking().ToListAsync()).Where(appointment => appointment.DateAppo < date);
    }

    public async Task<IEnumerable<Appointment>> GetAppointmentsDoctorAsync(string idDoctor)
    {
        return (await _context.Appointments.AsNoTracking().ToListAsync()).Where(appointment => appointment.Medic.Equals(idDoctor));
    }

    public async Task<IEnumerable<Appointment>> GetAppointmentsPatientAsync(string patient)
    {
        return (await _context.Appointments.AsNoTracking().ToListAsync()).Where(appointment => appointment.Patient!.Equals(patient));
    }

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