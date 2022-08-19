using ApiMedicalClinicEx.Server.Attributes;
using ApiMedicalClinicEx.Server.Context.Model;
using ApiMedicalClinicEx.Server.Model;
using ApiMedicalClinicEx.Server.Services;
using ApiMedicalClinicEx.Server.Services.Exceptions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace ApiMedicalClinicEx.Server.Controllers;

[Authorize, ApiController, Route("api/[controller]")]
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

    [EnableQuery]
    [HttpGet()]
    public async Task<ActionResult<IQueryable<PatientModel>>> GetPatients()
    {
        IEnumerable<PatientModel> response;

        try
        {
            response = _mapper.Map<IEnumerable<PatientModel>>(
                await _patientService.GetPatientsAsync()
            ).AsQueryable();
        }
        catch
        {
            return BadRequest("Erro ao coletar infração");
        }

        if (!response.Any())
            return NoContent();

        return Ok(response);
    }

    [HttpGet("{cpf}")]
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

    [HttpPost()]
    public async Task<ActionResult> PostPacient([FromBody] PatientModel patientModel)
    {
        try
        {
            await _patientService.AddPatientAsync(_mapper.Map<Patient>(patientModel));
        }
        catch (ServicesException e)
        {
            return StatusCode(e.StatusCode, e.Message);
        }
        catch
        {
            return BadRequest("Falha ao criar paciente.");
        }

        return Created($"/Patients/{patientModel.Cpf}", patientModel);
    }

    [HttpPut("{cpf}")]
    public async Task<ActionResult> PutPatient(string cpf, [FromBody] PatientModel patientModel)
    {
        try
        {
            if (!cpf.Equals(patientModel.Cpf))
                return BadRequest($"Cpf de {nameof(patientModel)} é diferente da url.");

            await _patientService.UpdatePatientAsync(cpf, _mapper.Map<Patient>(patientModel));
        }
        catch (ServicesException e)
        {
            return StatusCode(e.StatusCode, e.Message);
        }
        catch
        {
            return BadRequest("Falha de atualização do paciente.");
        }

        return Ok();
    }

    [HttpDelete("{cpf}")]
    [Authorize(Roles = ClaimTypeService.Claim.Admin)]
    public async Task<ActionResult> DeletePatient(string cpf)
    {
        try
        {
            await _patientService.RemovePatientAsync(cpf);
        }
        catch (ServicesException e)
        {
            return StatusCode(e.StatusCode, e.Message);
        }
        catch
        {
            return BadRequest("Falha de exclusão do paciente.");
        }

        return Ok();
    }

    #endregion

    #region  Allergy

    [EnableQuery]
    [HttpGet("Allergy/{cpf}")]
    public async Task<ActionResult<IQueryable<PatientAllergyModel>>> GetAllergys(string cpf)
    {
        try
        {
            var patient = await _patientService.GetPatientsAsync(cpf);

            if (patient is null)
                return NotFound($"Paciente com CPF {cpf} não encontrado.");

            return Ok(
                    _mapper.Map<IEnumerable<PatientAllergyModel>>(await _patientService.GetAllergysAsync(cpf)).AsQueryable()
                );
        }
        catch (ServicesException e)
        {
            return StatusCode(e.StatusCode, e.Message);
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
        catch (ServicesException e)
        {
            return StatusCode(e.StatusCode, e.Message);
        }
        catch
        {
            return BadRequest("Falha ao inserir nova alergia.");
        }

        return Created($"Allergy/{model.Cpf!}", model);
    }

    [HttpDelete("Allergy/{cpf}/{desc}")]
    public async Task<ActionResult> DeleteAllergy(string cpf, string desc)
    {
        try
        {
            if (string.IsNullOrEmpty(cpf) || string.IsNullOrEmpty(desc))
                return BadRequest("Insira corretamente os valores do cpf e descrição.");

            await _patientService.RemoveAllergyPatientAsync(cpf, desc);
        }
        catch (ServicesException e)
        {
            return StatusCode(e.StatusCode, e.Message);
        }
        catch
        {
            return BadRequest("Falha em exlusão da alergia.");
        }

        return Ok();
    }

    #endregion
}