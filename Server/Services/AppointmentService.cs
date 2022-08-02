using ApiMedicalClinicEx.Server.Context;
using ApiMedicalClinicEx.Server.Context.Model;

namespace ApiMedicalClinicEx.Server.Services;

public interface IAppointmentService
{
    Task<IEnumerable<Appointment>> GetAppointmentsDoctorAsync(string idDoctor);
    Task<IEnumerable<Appointment>> GetAppointmentsPatientAsync(string patient);
    Task<IEnumerable<Appointment>> GetAppointmentsAfterDateAsync(DateTime date);
    Task AddAppointmentAsync(Appointment appointment);
    Task RemoveAppointmentAsync(int IdAppointment);
    Task UpdateAppointmentAsync(int IdAppointment, Appointment appointment);
}

public class AppointmentService : IAppointmentService
{
    private readonly AppClinicContext _context;

    public AppointmentService(AppClinicContext context)
    {
        _context = context;
    }

    public Task AddAppointmentAsync(Appointment appointment)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Appointment>> GetAppointmentsAfterDateAsync(DateTime date)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Appointment>> GetAppointmentsDoctorAsync(string idDoctor)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Appointment>> GetAppointmentsPatientAsync(string patient)
    {
        throw new NotImplementedException();
    }

    public Task RemoveAppointmentAsync(int IdAppointment)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAppointmentAsync(int IdAppointment, Appointment appointment)
    {
        throw new NotImplementedException();
    }
}