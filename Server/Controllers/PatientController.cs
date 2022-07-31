using System.Security.Claims;
using ApiMedicalClinicEx.Server.Context;
using ApiMedicalClinicEx.Server.Context.Model;
using ApiMedicalClinicEx.Server.Model;
using ApiMedicalClinicEx.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ApiMedicalClinicEx.Server.Controllers;

[ApiController, Route("api/[controller]")]
public class PatientController : ControllerBase
{
    private readonly AppClinicContext _context;
    private readonly IBloodTypesService _bloodTypesService;

    public PatientController(AppClinicContext context, IBloodTypesService bloodTypesService)
    {
        _context = context;
        _bloodTypesService = bloodTypesService;
    }

    #region Patients

    [HttpGet("Patients")]
    public Task<ActionResult<IEnumerable<PatientModel>>> GetPatients()
    {
        
    }

    #endregion

    #region  Allergys
    #endregion
}