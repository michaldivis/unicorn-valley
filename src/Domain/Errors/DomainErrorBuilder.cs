namespace UnicornValley.Domain.Errors;

internal class DomainErrorBuilder
{
    private string _code = null!;
    private string _title = null!;
    private string _messageTemplate = null!;
    private object?[] _messageArgs = Array.Empty<object?>();
    private readonly Dictionary<string, object> _metadata = new();

    public DomainErrorBuilder WithCode(string code)
    {
        _code = code;
        return this;
    }

    public DomainErrorBuilder WithTitle(string title)
    {
        _title = title;
        return this;
    }

    public DomainErrorBuilder WithMessage(string messageTemplate, params object?[] args)
    {
        _messageTemplate = messageTemplate;
        _messageArgs = args;
        return this;
    }

    public DomainErrorBuilder WithMetadata(string key, object value)
    {
        _metadata.Add(key, value);
        return this;
    }

    public DomainError Create()
    {
        return new DomainError(_code, _title, _messageTemplate, _messageArgs, _metadata);
    }
}