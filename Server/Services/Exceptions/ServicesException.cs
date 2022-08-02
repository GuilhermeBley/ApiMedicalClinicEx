using System.Runtime.Serialization;

namespace ApiMedicalClinicEx.Server.Services.Exceptions;

public abstract class ServicesException : Exception
{

    public abstract int StatusCode { get; }
    
    protected ServicesException()
    {
    }

    protected ServicesException(string? message) : base(message)
    {
    }

    protected ServicesException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    protected ServicesException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}