namespace Api_authentication.Models
{
    public record User(string Username, string Password, string Role, string[] Scopes);
}