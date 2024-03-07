namespace IT_DeskServer.Core.ResultPattern;
#nullable enable

public interface IDataResult<T> : IResult
{
    public T? Data { get; }
}