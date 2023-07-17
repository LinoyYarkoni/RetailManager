﻿using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using RMDesktopUI.EventModles;
using RMDesktopUI.Library.Models;

namespace RMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        private SalesViewModel _salesViewModel;
        private IEventAggregator _events;
        private ILoggedInUserModel _loggedInUserModel;

        public ShellViewModel(IEventAggregator events, SalesViewModel salesViewModel, ILoggedInUserModel loggedInUserModel)
        {
            _events = events;
            _salesViewModel = salesViewModel;
            _loggedInUserModel = loggedInUserModel;

            _events.Subscribe(this);

            ActivateItemAsync(IoC.Get<LoginViewModel>());
        }

        public async Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
        {
            await ActivateItemAsync(_salesViewModel);

            NotifyOfPropertyChange(() => IsLoggedIn);
        }

        public void LogOut()
        {
            _loggedInUserModel.LogOffUser();
            ActivateItemAsync(IoC.Get<LoginViewModel>());
            NotifyOfPropertyChange(() => IsLoggedIn);
        }

        public async Task ExitApplication()
        {
            await TryCloseAsync();
        }

        public bool IsLoggedIn
        {
            get
            {
                return !string.IsNullOrWhiteSpace(_loggedInUserModel.Token);           
            }
        }
    }
}
