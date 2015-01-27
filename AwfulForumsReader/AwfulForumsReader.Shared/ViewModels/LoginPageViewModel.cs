using System;
using System.Threading.Tasks;
using AwfulForumsReader.Common;
using AwfulForumsLibrary.Exceptions;
using AwfulForumsLibrary.Interface;
using AwfulForumsLibrary.Manager;
using AwfulForumsReader.Tools;

namespace AwfulForumsReader.ViewModels
{
    public class LoginPageViewModel : NotifierBase
    {
        private readonly IAuthenticationManager _authManager;
        private string _password;
        private string _userName;
        private bool _isLoading;

        public LoginPageViewModel(IAuthenticationManager authManager)
        {
            ClickLoginButtonCommand = new AsyncDelegateCommand(async o => { await ClickLoginButton(); },
                o => CanClickLoginButton);
            _authManager = authManager;
        }

        public LoginPageViewModel()
            : this(new AuthenticationManager())
        {
        }

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                if (_isLoading == value) return;
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        public string UserName
        {
            get { return _userName; }
            set
            {
                if (_userName == value) return;
                _userName = value;
                OnPropertyChanged();
                ClickLoginButtonCommand.RaiseCanExecuteChanged();
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                if (_password == value) return;
                _password = value;
                OnPropertyChanged();
                ClickLoginButtonCommand.RaiseCanExecuteChanged();
            }
        }

        public bool CanClickLoginButton
        {
            get { return !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password); }
        }

        public AsyncDelegateCommand ClickLoginButtonCommand { get; private set; }
        public event EventHandler<EventArgs> LoginSuccessful;
        public event EventHandler<EventArgs> LoginFailed;

        public async Task ClickLoginButton()
        {
            bool loginResult;
            IsLoading = true;
            try
            {
                loginResult = await _authManager.Authenticate(UserName, Password);
            }
            catch (LoginFailedException ex)
            {
                AwfulDebugger.SendMessageDialogAsync(ex.Message, ex);
                loginResult = false;
            }
            IsLoading = false;
            base.RaiseEvent(loginResult ? LoginSuccessful : LoginFailed, EventArgs.Empty);
        }
    }
}
