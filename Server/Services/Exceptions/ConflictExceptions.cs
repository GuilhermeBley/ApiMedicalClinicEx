using System.Net;
using System.Runtime.Serialization;

namespace ApiMedicalClinicEx.Server.Services.Exceptions;

public class ConflictException : ServicesException
{
    public override int StatusCode => (int)HttpStatusCode.Conflict;

    public ConflictException()
    {
    }

    public ConflictException(string? message) : base(message)
    {
    }

    public ConflictException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected ConflictException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}