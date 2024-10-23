namespace _3.Infrastructure.Persistence.Interfaces.Token
{
    public interface ITokenRepository
    {
        public string GenerateTokenJwt(string username, string cuenta);
    }
}
