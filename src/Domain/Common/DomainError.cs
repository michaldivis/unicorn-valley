namespace UnicornValley.Domain.Common;
public class DomainError : Error
{
	public string Code { get; }

	internal DomainError(string code, string message) : base(message)
	{
		Code = code;
	}
}
