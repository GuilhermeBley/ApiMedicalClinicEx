using System.Net;
using System.Runtime.Serialization;

namespace ApiMedicalClinicEx.Server.Services.Exceptions;

public class NotFoundException : ServicesException
{

    public override int StatusCode => (int)HttpStatusCode.NotFound;

    public NotFoundException()
    {
    }

    public NotFoundException(string? message) : base(message)
    {
    }

    public NotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}