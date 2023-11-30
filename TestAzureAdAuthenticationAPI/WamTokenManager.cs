using Microsoft.Identity.Client;

namespace TestAzureAdAuthenticationAPI
{
    internal class WamTokenManager
    {
        public static async Task<AuthenticationResult> GetToken(string clientId, IntPtr hWnd)
        {
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
                var authResult = await pca.AcquireTokenSilent(new[] { "user.read" }, accountToLogin)
                                            .ExecuteAsync();
                return authResult;
            }
            catch (MsalUiRequiredException)
            {
                var authResult = await pca.AcquireTokenInteractive(new[] { "user.read" })
                                            .WithAccount(accountToLogin)
                                            .WithParentActivityOrWindow(hWnd)
                                            .ExecuteAsync();
                return authResult;
            }
        }
    }
}
