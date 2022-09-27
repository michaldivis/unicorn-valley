namespace UnicornValley.Domain.Errors;

internal class DomainSuccessBuilder
{
    private string _code = null!;
    private string _title = null!;
    private string _messageTemplate = null!;
    private object?[] _messageArgs = Array.Empty<object?>();
    private readonly Dictionary<string, object> _metadata = new();

    public DomainSuccessBuilder WithCode(string code)
    {
        _code = code;
        return this;
    }

    public DomainSuccessBuilder WithTitle(string title)
    {
        _title = title;
        return this;
    }

    public DomainSuccessBuilder WithMessage(string messageTemplate, params object?[] args)
    {
        _messageTemplate = messageTemplate;
        _messageArgs = args;
        return this;
    }

    public DomainSuccessBuilder WithMetadata(string key, object value)
    {
        _metadata.Add(key, value);
        return this;
    }

    public DomainSuccess Create()
    {
        return new DomainSuccess(_code, _title, _messageTemplate, _messageArgs, _metadata);
    }
}