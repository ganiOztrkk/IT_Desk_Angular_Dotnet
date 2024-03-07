namespace IT_DeskServer.Entity.Options;

public sealed class Jwt // appsettings.json dosyasındaki Jwt sectionumuzu tip güvenli yapmak için options pattern uyguluyoruz. bu sectionu eşleyeceğimiz sınıf burası.
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string SecretKey { get; set; }
}