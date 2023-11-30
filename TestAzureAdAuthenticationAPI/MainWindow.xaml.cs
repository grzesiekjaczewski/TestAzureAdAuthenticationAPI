using Microsoft.Identity.Client;
using System.Windows;
using System.Configuration;

namespace TestAzureAdAuthenticationAPI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var wih = new System.Windows.Interop.WindowInteropHelper(this);
            IntPtr hWnd = wih.Handle;

            AuthenticationResult result = Task.Run(() => WamTokenManager.GetToken(ConfigurationManager.AppSettings["ClientId"], hWnd))
                .GetAwaiter()
                .GetResult();

            TokenBox.Text = result.AccessToken;
            UserName.Text = result.Account.Username;
         }
    }
}