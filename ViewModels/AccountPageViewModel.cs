using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using Auth0.OidcClient;
using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
using Taskinator.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using Taskinator.Models;
using System.Text.Json;

namespace Taskinator.ViewModels
{
    public partial class AccountPageViewModel : BaseViewModel
    {
        private readonly IConnectivity _connectivity;
        private readonly IDialogService _dialogService;
        private readonly IDatabaseService _databaseService;
        private Auth0Client _client;
        private string _authDomain;
        private string _clientId;

        public AccountPageViewModel(IConnectivity connectivity, IDialogService dialogService, IDatabaseService databaseService)
        {
            _dialogService = dialogService;
            _connectivity = connectivity;
            _databaseService = databaseService;

            string jsonCredentials = SecureStorage.GetAsync("auth_credentials").Result ?? "auth_credentials";
            if (jsonCredentials != null)
            {
                var credentials = JsonSerializer.Deserialize<AuthCredentials>(jsonCredentials);
                _authDomain = credentials?.domain ?? "your_domain";
                _clientId = credentials?.clientId ?? "your_client_id";
            }
            else
            {
                _authDomain = "your_domain";
                _clientId = "your_client_id";
            }

            _client = new Auth0Client(new Auth0ClientOptions
            {
                Domain = _authDomain,
                ClientId = _clientId,
                Scope = "openid profile email",
                RedirectUri = "myapp://callback",
                PostLogoutRedirectUri = "myapp://callback",
            });

            LoginCommand = new AsyncRelayCommand(OnLoginClicked);
            LogoutCommand = new AsyncRelayCommand(OnLogoutClicked);
        }

        [ObservableProperty]
        private string helloText = "Hello, User!";

        [ObservableProperty]
        private string errorText = string.Empty;

        [ObservableProperty]
        private bool isLoginVisible = true;

        [ObservableProperty]
        private bool isLogoutVisible = false;

        public ICommand LoginCommand { get; }
        public ICommand LogoutCommand { get; }

        private async Task OnLoginClicked()
        {
            if (!IsConnectedToInternet())
            {
                await _dialogService.DisplayAlert("Warning!", "No internet connection. Please check your network settings and try again.", "Ok");
                return;
            }

            var extraParameters = new Dictionary<string, string>();
            var audience = ""; // fill with audience if needed

            if (!string.IsNullOrEmpty(audience))
                extraParameters.Add("audience", audience);

            try
            {
                var result = await _client.LoginAsync(extraParameters);

                _databaseService.SetLoggedInUser(result);

                DisplayResult(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task OnLogoutClicked()
        {
            if (!IsConnectedToInternet())
            {
                await _dialogService.DisplayAlert("Warning!", "No internet connection. Please check your network settings and try again.", "Ok");
                return;
            }

            try
            {
                var browserResult = await _client.LogoutAsync();

                if (browserResult != BrowserResultType.Success)
                {
                    ErrorText = browserResult.ToString();
                    return;
                }

                IsLoginVisible = true;
                IsLogoutVisible = false;
                HelloText = "Hello, User!";
                ErrorText = string.Empty;

                _databaseService.SetLoggedInUser(new LoginResult());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void DisplayResult(LoginResult loginResult)
        {
            if (loginResult.IsError)
            {
                ErrorText = loginResult.Error;
                return;
            }

            IsLoginVisible = false;
            IsLogoutVisible = true;
            HelloText = $"Hello, {loginResult.User?.Identity?.Name}";
            ErrorText = string.Empty;
        }

    }
}
