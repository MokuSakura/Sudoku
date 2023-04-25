using System.Runtime.Serialization;

namespace MokuSakura.SudokuConsole.Exception;

public class NoRequirementFoundException : ApplicationException
{
    public NoRequirementFoundException(String requirementName)
    {
        RequirementName = requirementName;
    }

    protected NoRequirementFoundException(SerializationInfo info, StreamingContext context, String requirementName) : base(info, context)
    {
        RequirementName = requirementName;
    }

    public NoRequirementFoundException(String? message, String requirementName) : base(message)
    {
        RequirementName = requirementName;
    }

    public NoRequirementFoundException(String? message, System.Exception? innerException, String requirementName) : base(message, innerException)
    {
        RequirementName = requirementName;
    }

    public String RequirementName { get; init; }
}