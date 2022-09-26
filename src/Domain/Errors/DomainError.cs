namespace UnicornValley.Domain.Errors;
public record DomainError : IError
{
    public string Code { get; }
    public string Title { get; }
    public string Message { get; }
    public string MessageTemplate { get; }
    public object?[] Args { get; }
    public Dictionary<string, object> Metadata { get; } = new();
    public List<IError> Reasons { get; } = new();

    internal DomainError(string code, string title, string messageTemplate, object?[] args, Dictionary<string, object> metadata)
    {
        Code = code;
        Title = title;
        MessageTemplate = messageTemplate;
        Args = args;
        Metadata = metadata;

        Message = string.Format(messageTemplate, args);
    }
}

internal interface INeedCode
{
    INeedTitle WithCode(string code);
}

internal interface INeedTitle
{
    INeedMessage WithTitle(string title);
}

internal interface INeedMessage
{
    IAmReady WithMessage(string messageTemplate, params object?[] args);
}

internal interface IAmReady
{
    IAmReady WithMetadata(string key, object value);
    DomainError Create();
}

internal class DomainErrorBuilder : INeedCode, INeedTitle, INeedMessage, IAmReady
{
    private string _code;
    private string _title;
    private string _messageTemplate;
    private object?[] _messageArgs;
    private Dictionary<string, object> _metadata = new();

    public INeedTitle WithCode(string code)
    {
        _code = code;
        return this;
    }

    public INeedMessage WithTitle(string title)
    {
        _title = title;
        return this;
    }

    public IAmReady WithMessage(string messageTemplate, params object?[] args)
    {
        _messageTemplate = messageTemplate;
        _messageArgs = args;
        return this;
    }

    public IAmReady WithMetadata(string key, object value)
    {
        _metadata.Add(key, value);
        return this;
    }

    public DomainError Create()
    {
        return new DomainError(_code, _title, _messageTemplate, _messageArgs, _metadata);
    }
}