using ApiMedicalClinicEx.Server.Context.Model;
using ApiMedicalClinicEx.Server.Model;
using ApiMedicalClinicEx.Server.Services.Exceptions;
using ApiMedicalClinicEx.Server.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OData.Query;

namespace ApiMedicalClinicEx.Server.Controllers;

[Authorize, ApiController, Route("api/[controller]")]
public class AppointmentController : ControllerBase
{
    private readonly IAppointmentService _appointService;
    private readonly IMapper _mapper;

    public AppointmentController(IAppointmentService appointService, IMapper mapper)
    {
        _appointService = appointService;
        _mapper = mapper;
    }

    [EnableQuery]
    [HttpGet("Doctor/{idDoctor:int}")]
    public async Task<ActionResult<IQueryable<AppointmentResponseModel>>> GetAppointmentsDoctor(int idDoctor)
    {
        try
        {
            return Ok(
                _mapper.Map<IEnumerable<AppointmentResponseModel>>(await _appointService.GetAppointmentsDoctorAsync(idDoctor)).AsQueryable()
            );
        }
        catch
        {
            return BadRequest("Falha ao coletar agendamentos.");
        }
    }

    [EnableQuery]
    [HttpGet("Patient/{cpfPatient}")]
    public async Task<ActionResult<IQueryable<AppointmentResponseModel>>> GetAppointmentsPatient(string cpfPatient)
    {
        try
        {
            return Ok(
                _mapper.Map<IEnumerable<AppointmentResponseModel>>(await _appointService.GetAppointmentsPatientAsync(cpfPatient)).AsQueryable()
            );
        }
        catch
        {
            return BadRequest("Falha ao coletar agendamentos.");
        }
    }

    [EnableQuery]
    [HttpGet("Date/{cpfPatient:datetime}")]
    public async Task<ActionResult<IQueryable<AppointmentResponseModel>>> GetAppointmentsAfterDate(DateTime date)
    {
        try
        {
            return Ok(
                _mapper.Map<IEnumerable<AppointmentResponseModel>>(await _appointService.GetAppointmentsAfterDateAsync(date)).AsQueryable()
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
            return BadRequest("Falha ao inserir nova consulta médica.");
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
            return BadRequest("Falha ao inserir nova consulta médica.");
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
            return BadRequest("Falha ao inserir nova consulta médica.");
        }

        return Ok();
    }
}