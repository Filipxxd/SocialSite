namespace SocialSite.Domain.Utilities;

public class Result
{
    public virtual bool IsFailed => Errors.Count != 0;
    public virtual bool IsSuccess => Errors.Count == 0;
    public virtual Dictionary<string, IEnumerable<string>> Errors { get; protected set; } = [];

    public static Result Fail(Dictionary<string, IEnumerable<string>> errors) => new() { Errors = errors ?? [] };

    public static Result Fail(string key, string error) => new()
    {
        Errors = new Dictionary<string, IEnumerable<string>>
        {
            { key, [ error ] }
        }
    };

    public static Result Fail(string key, IEnumerable<string> errors) => new()
    {
        Errors = new Dictionary<string, IEnumerable<string>>
        {
            { key, errors ?? [] }
        }
    };

    public static Result Success() => new();
}

public class Result<T> : Result
{
    public virtual T Entity { get; protected set; }

    public static new Result<T> Fail(Dictionary<string, IEnumerable<string>> errors) => new()
    {
        Errors = errors ?? []
    };

    public static new Result<T> Fail(string key, string error) => new()
    {
        Errors = new Dictionary<string, IEnumerable<string>>
        {
            { key,[ error ] }
        }
    };

    public static new Result<T> Fail(string key, IEnumerable<string> errors) => new()
    {
        Errors = new Dictionary<string, IEnumerable<string>>
        {
            { key, errors ?? [] }
        }
    };

    public static Result<T> Success(T entity) => new() { Entity = entity };
}
