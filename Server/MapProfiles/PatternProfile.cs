using AutoMapper;
using ApiMedicalClinicEx.Server.Context.Model;
using ApiMedicalClinicEx.Server.Model;


namespace ApiMedicalClinicEx.Server.MapProfiles;

public class PatternProfile : Profile
{
    public PatternProfile() : base()
    {
        CreateMap<Patient, PatientModel>().ReverseMap();
        CreateMap<PatientAllergy, PatientAllergyModel>().ReverseMap();
        CreateMap<Appointment, AppointmentModel>().ReverseMap();
    }
}