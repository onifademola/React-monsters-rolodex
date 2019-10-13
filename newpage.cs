using EBZ.Mobile.Services;
using EBZ.Mobile.ServicesInterface;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace EBZ.Mobile.ViewModels.Login
{
    /// <summary>
    /// ViewModel for login page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class LoginPageViewModel : LoginViewModel
    {
        #region Fields
        private string email;
        private bool isInvalidEmail;
        private string password;
        private readonly IAuthenticationService _authenticationService;
        private readonly ISettingsService _settingsService;
        private readonly IDialogService _dialogService;
        private readonly IConnectionService _connectionService;
        #endregion

        #region Constructor
        public LoginPageViewModel(IAuthenticationService authenticationService, ISettingsService settingsService, IDialogService dialogService, IConnectionService connectionService)
        {
            _authenticationService = authenticationService;
            _settingsService = settingsService;
            _dialogService = dialogService;
            _connectionService = connectionService;
        }
        /// <summary>
        /// Initializes a new instance for the <see cref="LoginPageViewModel" /> class.
        /// </summary>
        public LoginPageViewModel()
        {
            this.LoginCommand = new Command(this.LoginClicked);
            this.SignUpCommand = new Command(this.SignUpClicked);
            this.ForgotPasswordCommand = new Command(this.ForgotPasswordClicked);
            this.SocialMediaLoginCommand = new Command(this.SocialLoggedIn);
        }

        #endregion

        #region property

        /// <summary>
        /// Gets or sets the property that is bound with an entry that gets the password from user in the login page.
        /// </summary>
        public string Password
        {
            get
            {
                return this.password;
            }

            set
            {
                if (this.password == value)
                {
                    return;
                }

                this.password = value;
                this.OnPropertyChanged();
            }
        }

        #endregion

        #region Command

        /// <summary>
        /// Gets or sets the command that is executed when the Log In button is clicked.
        /// </summary>
        public Command LoginCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that is executed when the Sign Up button is clicked.
        /// </summary>
        public Command SignUpCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that is executed when the Forgot Password button is clicked.
        /// </summary>
        public Command ForgotPasswordCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that is executed when the social media login button is clicked.
        /// </summary>
        public Command SocialMediaLoginCommand { get; set; }

        #endregion

        #region methods

        /// <summary>
        /// Invoked when the Log In button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void LoginClicked(object obj)
        {
            _dialogService.ShowDialog("Authenticating...");

            if (_connectionService.IsConnected)
            {
                try
                {
                    var authenticationResponse = await _authenticationService.Authenticate(Email, Password);

                    if (authenticationResponse.IsAuthenticated)
                    {
                        // we store the Id to know if the user is already logged in to the application
                        _settingsService.UserNameSetting = authenticationResponse.Username;
                        _settingsService.TokenSetting = authenticationResponse.Token;
                        _settingsService.ValidToSetting = authenticationResponse.ValidTo.ToShortDateString();
                        _settingsService.RolesSetting = authenticationResponse.Role;

                        var viewNAvServ = App.ViewNavigationService;
                        var mainPage = ((NavigationService)viewNAvServ).SetRootPage("MainPage");

                        _dialogService.HideDialog();
                        //App.Current.MainPage = new Navigation(Views.Shop.CartPage());
                    }
                    else
                    {
                        _dialogService.HideDialog();
                        await _dialogService.ShowDialog(
                        "This username/password combination is not valid",
                        "Error logging you in",
                        "OK");
                    }
                }
                catch (System.Exception)
                {
                    _dialogService.HideDialog();
                    await _dialogService.ShowDialog(
                    "This username/password combination is not valid",
                    "Error logging you in",
                    "OK");
                }
            }
            else
            {
                await _dialogService.ShowDialog(
                    "This username/password combination isn't known",
                    "Error logging you in",
                    "OK");
            }

        }

        /// <summary>
        /// Invoked when the Sign Up button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void SignUpClicked(object obj)
        {
            // Do something
        }

        /// <summary>
        /// Invoked when the Forgot Password button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void ForgotPasswordClicked(object obj)
        {
            var label = obj as Label;
            label.BackgroundColor = Color.FromHex("#70FFFFFF");
            await Task.Delay(100);
            label.BackgroundColor = Color.Transparent;
        }

        /// <summary>
        /// Invoked when social media login button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void SocialLoggedIn(object obj)
        {
            // Do something
        }

        #endregion
    }
}