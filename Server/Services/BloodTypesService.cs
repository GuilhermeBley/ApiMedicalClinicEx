namespace ApiMedicalClinicEx.Server.Services;

/// <summary>
/// blood types
/// </summary>
public interface IBloodTypesService
{
    /// <summary>
    /// Validate blood type
    /// </summary>
    /// <remarks>
    ///     <para>if is valid, <paramref name="outBloodType"/> return a non null value</para>
    /// </remarks>
    /// <param name="bloodType">string to validate</param>
    /// <param name="outBloodType">type formatted</param>
    /// <returns>true : is valid, false : invalid</returns>
    bool IsValidBloodType(string bloodType, out string outBloodType);

    /// <summary>
    /// List of avaliable blood types
    /// </summary>
    /// <returns>list string types</returns>
    IEnumerable<string> GetBloodTypes();
}

internal class BloodTypesService : IBloodTypesService
{
    private readonly static IEnumerable<string> _bloodTypes = 
        new List<string> { "A+", "A-", "AB+", "AB-", "B+", "B-", "O+", "O-" };

    /// <inheritdoc/>
    public IEnumerable<string> GetBloodTypes()
    {
        return _bloodTypes;
    }

    /// <inheritdoc/>
    public bool IsValidBloodType(string bloodType, out string outBloodType)
    {
        outBloodType = null!;

        bloodType = bloodType.ToUpper().Replace(" ", "");

        if (!_bloodTypes.Contains(bloodType))
            return false;

        outBloodType = bloodType;

        return true;
    }
}