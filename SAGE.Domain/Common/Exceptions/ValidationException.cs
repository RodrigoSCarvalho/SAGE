namespace SAGE.Domain.Common.Exceptions;

public class ValidationException : Exception
{
    public Dictionary<string, string[]> Errors { get; }

    public ValidationException(string message) : base(message)
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(Dictionary<string, string[]> errors)
        : base("Um ou mais erros de validação ocorreram.")
    {
        Errors = errors;
    }
}
