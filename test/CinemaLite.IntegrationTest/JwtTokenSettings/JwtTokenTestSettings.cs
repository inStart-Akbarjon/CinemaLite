using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace CinemaLite.IntegrationTest.JwtTokenSettings;

public class JwtTokenTestSettings
{
    public const string SecretKey = "secretKeyForAuth18efefefefefefefefefefefjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjffffffffffffffffffffffffffffffffffff18861178118918181111848wefwefwefewfwefwlibyvyeroapnpiwebgwegpweigbweigbweg";
    public const string Audience = "CinemaLiteClient";
    public const string Issuer = "CinemaLiteAPI";
    public const int ExpireTimeInSeconds = 3600;
    
    private static readonly SymmetricSecurityKey secretKey = new(
        Encoding.UTF8.GetBytes(SecretKey));
}