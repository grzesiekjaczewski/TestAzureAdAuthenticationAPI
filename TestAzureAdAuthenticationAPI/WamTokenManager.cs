using Microsoft.Identity.Client;
using System.Configuration;

namespace TestAzureAdAuthenticationAPI
{
    internal class WamTokenManager
    {
        public static async Task<AuthenticationResult> GetToken(IntPtr hWnd)
        {
            string clientId = ConfigurationManager.AppSettings["ClientId"];
            string scope = ConfigurationManager.AppSettings["Scope"];

            var pca = PublicClientApplicationBuilder.Create(clientId)
                .WithBroker(new BrokerOptions(BrokerOptions.OperatingSystems.Windows))
                .Build();

            IAccount accountToLogin = (await pca.GetAccountsAsync()).FirstOrDefault();
            if (accountToLogin == null)
            {
                accountToLogin = PublicClientApplication.OperatingSystemAccount;
            }

            try
            {
                var authResult = await pca.AcquireTokenSilent(new[] { scope }, accountToLogin)
                                            .ExecuteAsync();
                return authResult;
            }
            catch (MsalUiRequiredException)
            {
                var authResult = await pca.AcquireTokenInteractive(new[] { scope })
                                            .WithAccount(accountToLogin)
                                            .WithParentActivityOrWindow(hWnd)
                                            .ExecuteAsync();
                return authResult;
            }
        }
    }
}
