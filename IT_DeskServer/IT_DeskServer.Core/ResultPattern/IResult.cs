namespace IT_DeskServer.Core.ResultPattern;
#nullable enable

public interface IResult //void donen metotlar için dönüş değeri
{
    public bool Success { get; }
    public string? Message { get; }
}