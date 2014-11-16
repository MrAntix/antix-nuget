using Antix.NuGet.Events;
using Microsoft.AspNet.SignalR;

namespace Antix.NuGet.Application.Events
{
    public class EventsHub : Hub
    {
        readonly IEventsBus _eventsBus;

        public EventsHub(IEventsBus eventsBus)
        {
            _eventsBus = eventsBus;
        }
    }
}