namespace ScBootstrap.Logic.Services
{
    public interface IAuthenticationService
    {
        bool IsAuthenticated();
        bool Login(string userName, string password, bool persistent);
        void Logout();
    }
}
