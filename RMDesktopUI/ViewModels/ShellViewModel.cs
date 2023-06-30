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
        private SimpleContainer _container;

        public ShellViewModel(IEventAggregator events, SalesViewModel salesViewModel,
            SimpleContainer container)
        {
            _events = events;
            _events.Subscribe(this);

            _salesViewModel = salesViewModel;

            _container = container;

            ActivateItemAsync(_container.GetInstance<LoginViewModel>());
        }

        public async Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
        {
            await ActivateItemAsync(_salesViewModel);
        }
    }
}
