using System.Runtime.Serialization;

namespace MokuSakura.SudokuConsole.Exception;

public class NoRequirementFoundException : ApplicationException
{
    public NoRequirementFoundException(string requirementName)
    {
        RequirementName = requirementName;
    }

    protected NoRequirementFoundException(SerializationInfo info, StreamingContext context, string requirementName) : base(info, context)
    {
        RequirementName = requirementName;
    }

    public NoRequirementFoundException(string? message, string requirementName) : base(message)
    {
        RequirementName = requirementName;
    }

    public NoRequirementFoundException(string? message, System.Exception? innerException, string requirementName) : base(message, innerException)
    {
        RequirementName = requirementName;
    }

    public string RequirementName { get; init; }
}