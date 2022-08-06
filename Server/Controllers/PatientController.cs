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

    #region Patient

    [HttpGet("Patient")]
    public async Task<ActionResult<IEnumerable<PatientModel>>> GetPatients()
    {
        IEnumerable<PatientModel> response;

        try
        {
            response = _mapper.Map<IEnumerable<PatientModel>>(
                await _patientService.GetPatientsAsync()
            );
        }
        catch
        {
            return BadRequest("Erro ao coletar infração");
        }

        if (!response.Any())
            return NoContent();

        return Ok(response);
    }

    [HttpGet("Patient/{cpf}")]
    public async Task<ActionResult<PatientModel>> GetPatients(string cpf)
    {
        try
        {
            return Ok(
                _mapper.Map<PatientModel>(
                    await _patientService.GetPatientsAsync(cpf)
                )
            );
        }
        catch
        {
            return BadRequest("Falha ao coletar pacientes.");
        }
    }

    [HttpPost("Patient")]
    public async Task<ActionResult> PostPacient([FromBody] PatientModel patientModel)
    {
        try
        {
            await _patientService.AddPatientAsync(_mapper.Map<Patient>(patientModel));
        }
        catch
        {
            return BadRequest("Falha ao criar paciente.");
        }

        return Created($"/Patients/{patientModel.Cpf}", patientModel);
    }

    [HttpPut("Patient/{cpf}")]
    public async Task<ActionResult> PutPatient(string cpf, [FromBody] PatientModel patientModel)
    {
        try
        {
            if (!cpf.Equals(patientModel.Cpf))
                return BadRequest($"Cpf de {nameof(patientModel)} é diferente da url.");

            await _patientService.UpdatePatientAsync(cpf, _mapper.Map<Patient>(patientModel));
        }
        catch
        {
            return BadRequest("Falha de atualização do paciente.");
        }

        return Ok();
    }

    [HttpDelete("Patient/{cpf}")]
    public async Task<ActionResult> DeletePatient(string cpf)
    {
        try
        {
            await _patientService.RemovePatientAsync(cpf);
        }
        catch
        {
            return BadRequest("Falha de exclusão do paciente.");
        }

        return Ok();
    }

    #endregion

    #region  Allergy

    [HttpGet("Allergy/{cpf}")]
    public async Task<ActionResult<IEnumerable<PatientAllergyModel>>> GetAllergys(string cpf)
    {
        try
        {
            var patient = await _patientService.GetPatientsAsync(cpf);

            if (patient is null)
                return NotFound($"Paciente com CPF {cpf} não encontrado.");

            return Ok(
                    _mapper.Map<IEnumerable<PatientAllergyModel>>(await _patientService.GetAllergysAsync(cpf))
                );
        }
        catch
        {
            return BadRequest("Falha na coleta de alergias.");
        }
    }

    [HttpPost("Allergy")]
    public async Task<ActionResult> PostAllergy(PatientAllergyModel model)
    {
        try
        {
            var patient = await _patientService.GetPatientsAsync(model.Cpf!);

            if (patient is null)
                return NotFound($"Paciente com CPF {model.Cpf!} não encontrado.");

            await _patientService.AddAllergyPatientAsync(model.Cpf!, _mapper.Map<PatientAllergy>(model));
        }
        catch
        {
            return BadRequest("Falha ao inserir nova alergia.");
        }

        return Created($"Allergy/{model.Cpf!}", model);
    }

    [HttpPost("Allergy/{cpf}/{desc}")]
    public async Task<ActionResult> DeleteAllergy(string cpf, string desc)
    {
        try
        {
            if (string.IsNullOrEmpty(cpf) || string.IsNullOrEmpty(desc))
                return BadRequest("Insira corretamente os valores do cpf e descrição.");

            await _patientService.RemoveAllergyPatientAsync(cpf, desc);
        }
        catch
        {
            return BadRequest("Falha em exlusão da alergia.");
        }

        return Ok();
    }

    #endregion
}