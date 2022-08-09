using ApiMedicalClinicEx.Server.Context.Model;
using ApiMedicalClinicEx.Server.Model;
using ApiMedicalClinicEx.Server.Services.Exceptions;
using ApiMedicalClinicEx.Server.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;


namespace ApiMedicalClinicEx.Server.Controllers;

[ApiController, Route("api/[controller]")]
public class AppointmentController : ControllerBase
{
    private readonly IAppointmentService _appointService;
    private readonly IMapper _mapper;

    public AppointmentController(IAppointmentService appointService, IMapper mapper)
    {
        _appointService = appointService;
        _mapper = mapper;
    }

    [HttpGet("Doctor/{idDoctor}")]
    public async Task<ActionResult<AppointmentModel>> GetAppointmentsDoctor(string idDoctor)
    {
        try
        {
            return Ok(
                _mapper.Map<IEnumerable<AppointmentModel>>(await _appointService.GetAppointmentsDoctorAsync(idDoctor))
            );
        }
        catch
        {
            return BadRequest("Falha ao coletar agendamentos.");
        }
    }

    [HttpGet("Patient/{cpfPatient}")]
    public async Task<ActionResult<AppointmentModel>> GetAppointmentsPatient(string cpfPatient)
    {
        try
        {
            return Ok(
                _mapper.Map<IEnumerable<AppointmentModel>>(await _appointService.GetAppointmentsPatientAsync(cpfPatient))
            );
        }
        catch
        {
            return BadRequest("Falha ao coletar agendamentos.");
        }
    }

    [HttpGet("Date/{cpfPatient:datetime}")]
    public async Task<ActionResult<AppointmentModel>> GetAppointmentsAfterDate(DateTime date)
    {
        try
        {
            return Ok(
                _mapper.Map<IEnumerable<AppointmentModel>>(await _appointService.GetAppointmentsAfterDateAsync(date))
            );
        }
        catch
        {
            return BadRequest("Falha ao coletar agendamentos.");
        }
    }

    [HttpPost]
    public async Task<ActionResult> PostAppointment([FromBody] AppointmentModel model)
    {
        try
        {
            await _appointService.AddAppointmentAsync(_mapper.Map<Appointment>(model));
        }
        catch (ServicesException e)
        {
            return StatusCode(e.StatusCode, e.Message);
        }
        catch
        {
            return BadRequest("Falha ao inserir nova consulta média.");
        }

        return Created($"Patient/{model.Patient}", model);
    }

    [HttpPut("{idAppointment:int}")]
    public async Task<ActionResult> PutAppointment(int idAppointment, [FromBody] AppointmentModel model)
    {
        try
        {
            await _appointService.UpdateAppointmentAsync(idAppointment, _mapper.Map<Appointment>(model));
        }
        catch (ServicesException e)
        {
            return StatusCode(e.StatusCode, e.Message);
        }
        catch
        {
            return BadRequest("Falha ao inserir nova consulta média.");
        }

        return Ok();
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteAppointment(int idAppointment)
    {
        try
        {
            await _appointService.RemoveAppointmentAsync(idAppointment);
        }
        catch (ServicesException e)
        {
            return StatusCode(e.StatusCode, e.Message);
        }
        catch
        {
            return BadRequest("Falha ao inserir nova consulta média.");
        }

        return Ok();
    }
}