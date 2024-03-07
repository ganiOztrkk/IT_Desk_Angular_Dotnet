namespace IT_DeskServer.Core.ResultPattern;

public class SuccessDataResult<T> : DataResult<T> 
{
    public SuccessDataResult(T data) : base(data, true)
    {
    }

    public SuccessDataResult(T data, string message) : base(data, true, message)
    {
    }
}