using System.Net;
using System.Runtime.Serialization;

namespace ApiMedicalClinicEx.Server.Services.Exceptions;

public class BusinessRulesException : ServicesException
{
    public override int StatusCode => (int)HttpStatusCode.BadRequest;

    public BusinessRulesException()
    {
    }

    public BusinessRulesException(string? message) : base(message)
    {
    }

    public BusinessRulesException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public BusinessRulesException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}