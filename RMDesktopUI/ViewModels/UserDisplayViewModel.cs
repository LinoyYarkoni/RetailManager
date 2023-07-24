using Caliburn.Micro;
using RMDesktopUI.Library.Api;
using RMDesktopUI.Library.Models;
using System;
using System.ComponentModel;
using System.Dynamic;
using System.Threading.Tasks;
using System.Windows;

namespace RMDesktopUI.ViewModels
{
    public class UserDisplayViewModel : Screen
    {
        private StatusInfoViewModel _status;
        private IWindowManager _window;
        private IUserEndpoint _userEndpoint;
        private BindingList<UserModel> _users;

        public UserDisplayViewModel(
            StatusInfoViewModel status,
            IWindowManager window,
            IUserEndpoint userEndpoint)
        {
            _status = status;
            _window = window;
            _userEndpoint = userEndpoint;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            try
            {
                await loadUsers();
            }
            catch (Exception ex)
            {
                dynamic settings = new ExpandoObject();
                settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                settings.ResizeMode = ResizeMode.NoResize;
                settings.Title = "System Error";

                if (ex.Message == "Unauthorized")
                {
                    _status.UpdateMessage("Unauthorize Access",
                        "You do not have a permission to interact with the sale view");
                    await _window.ShowDialogAsync(_status, null, settings);
                }
                else
                {
                    _status.UpdateMessage("Fatal Exception", ex.Message);
                    await _window.ShowDialogAsync(_status, null, settings);
                }


                await TryCloseAsync();
            }
        }

        public BindingList<UserModel> Users
        {
            get
            {
                return _users;
            }
            set
            {
                _users = value;
                NotifyOfPropertyChange(() =>  Users);
            }
        }

        private async Task loadUsers()
        {
            var userList = await _userEndpoint.GetAll();

            Users = new BindingList<UserModel>(userList);
        }
    }
}
