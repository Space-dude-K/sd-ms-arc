namespace Api_authentication.Models
{
    public record AuthenticationToken(string Token, int ExpiresIn);
}