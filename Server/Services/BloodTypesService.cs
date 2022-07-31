namespace ApiMedicalClinicEx.Server.Services;

public interface IBloodTypesService
{
    bool IsValidBloodType(string bloodType, out string outBloodType);
    IEnumerable<string> GetBloodTypes();
}

internal class BloodTypesService : IBloodTypesService
{
    private readonly static IEnumerable<string> _bloodTypes = 
        new List<string> { "A+", "A-", "AB+", "AB-", "B+", "B-", "O+", "O-" };

    public IEnumerable<string> GetBloodTypes()
    {
        return _bloodTypes;
    }

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