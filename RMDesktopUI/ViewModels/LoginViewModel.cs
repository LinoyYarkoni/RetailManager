using Caliburn.Micro;
using System;
using System.Threading.Tasks;
using RMDesktopUI.Library.Api;
using RMDesktopUI.EventModles;

namespace RMDesktopUI.ViewModels
{
    public class LoginViewModel : Screen
    {
		private string _userName = "Linoy@gmail.com";
        private string _password = "Lin12345.";
        private IAPIHelper _apiHelper;
        private string _errorMessage;
        private IEventAggregator _events;

        public LoginViewModel(IAPIHelper apiHelper, IEventAggregator events)
        {
            _apiHelper = apiHelper;
            _events = events;
        }

        public string UserName
		{
			get
            { 
                return _userName; 
            }
			set
			{ 
				_userName = value;
				NotifyOfPropertyChange(() => UserName);
                NotifyOfPropertyChange(() => CanLogIn);
            }
		}

        public string Password
        {
            get
            { 
                return _password; 
            }
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanLogIn);
            }
        }

        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                _errorMessage = value;
                NotifyOfPropertyChange(() => ErrorMessage);
                NotifyOfPropertyChange(() => IsErrorVisible);
            }
        }


        public bool IsErrorVisible
        {
            get
            {
                return ErrorMessage?.Length > 0 ? true : false;
            }
        }

        public bool CanLogIn
        {
            get
            {
                bool result = false;

                if (UserName?.Length > 0 && Password?.Length > 0)
                {
                    result = true;
                }

                return result;
            }
        }

        public async Task LogIn() 
        {
            try
            {
                ErrorMessage = string.Empty;
                var authenticateUser = await _apiHelper.Authenticate(UserName, Password);

                await _apiHelper.GetLoggedInUserInfo(authenticateUser.Access_Token);

                await _events.PublishOnUIThreadAsync(new LogOnEvent());

            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
    }
}
