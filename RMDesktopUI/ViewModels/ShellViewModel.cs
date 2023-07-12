using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using RMDesktopUI.EventModles;

namespace RMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        private SalesViewModel _salesViewModel;
        private IEventAggregator _events;

        public ShellViewModel(IEventAggregator events, SalesViewModel salesViewModel)
        {
            _events = events;
            _events.Subscribe(this);

            _salesViewModel = salesViewModel;

            ActivateItemAsync(IoC.Get<LoginViewModel>());
        }

        public async Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
        {
            await ActivateItemAsync(_salesViewModel);
        }
    }
}
