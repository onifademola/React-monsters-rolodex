using EBZ.Mobile.Services;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace EBZ.Mobile.ViewModels
{
    /// <summary>
    /// ViewModel for login page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class LoginPageViewModel : ViewModelBase
    {
        #region Fields
        private string email;
        private bool isInvalidEmail;
        private string password;
        AuthenticationService _authenticationService = new AuthenticationService();
        SettingsService _settingsService = new SettingsService();
        DialogService _dialogService = new DialogService();
        ConnectionService _connectionService = new ConnectionService();

        #endregion

        #region Constructor
        
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
        public string Email
        {
            get
            {
                return this.email;
            }

            set
            {
                if (this.email == value)
                {
                    return;
                }

                this.email = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the entered email is valid or invalid.
        /// </summary>
        public bool IsInvalidEmail
        {
            get
            {
                return this.isInvalidEmail;
            }

            set
            {
                if (this.isInvalidEmail == value)
                {
                    return;
                }

                this.isInvalidEmail = value;
                this.OnPropertyChanged();
            }
        }
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
            // Do something
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