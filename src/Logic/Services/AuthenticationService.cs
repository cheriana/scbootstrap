namespace ScBootstrap.Logic.Services
{
    using System.Security.Authentication;
    using Sitecore;
    using Sitecore.Security.Authentication;

    public class AuthenticationService : IAuthenticationService
    {
        public bool IsAuthenticated()
        {
            return AuthenticationManager.GetActiveUser().IsAuthenticated;
        }

        public bool Login(string userName, string password, bool persistent)
        {
            try
            {
                var domain = Context.Domain;
                var domainUser = domain.Name + @"\" + userName;
                return AuthenticationManager.Login(domainUser, password, persistent);
            }
            catch (AuthenticationException)
            {
                return false;
            }
        }

        public void Logout()
        {
            AuthenticationManager.Logout();
        }
    }
}
