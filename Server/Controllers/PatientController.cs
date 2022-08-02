using System.Security.Claims;
using ApiMedicalClinicEx.Server.Context;
using ApiMedicalClinicEx.Server.Context.Model;
using ApiMedicalClinicEx.Server.Model;
using ApiMedicalClinicEx.Server.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiMedicalClinicEx.Server.Controllers;

[ApiController, Route("api/[controller]")]
public class PatientController : ControllerBase
{
    private readonly IPatientService _patientService;
    private readonly IMapper _mapper;

    public PatientController(IPatientService patientService, IMapper mapper)
    {
        _patientService = patientService;
        _mapper = mapper;
    }

    #region Patients

    [HttpGet("Patients")]
    public async Task<ActionResult<IEnumerable<PatientModel>>> GetPatients()
    {
        IEnumerable<PatientModel> response;

        try
        {
            response = await _patientService.GetPatientsAsync();
        }
        catch
        {
            return BadRequest("Erro ao coletar infração");
        }

        if (!response.Any())
            return NoContent();

        return Ok(response);
    }

    [HttpPost("Patients")]
    public async Task<ActionResult> PostPacient([FromBody] PatientModel patientModel)
    {
        await Task.CompletedTask;
        return Ok();
    }

    #endregion

    #region  Allergys
    #endregion
}