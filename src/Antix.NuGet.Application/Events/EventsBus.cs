using Antix.NuGet.Events;
using Microsoft.AspNet.SignalR;

namespace Antix.NuGet.Application.Events
{
    public class EventsBus : IEventsBus
    {
        readonly IHubContext _hubContext;

        public EventsBus(IHubContext hubContext)
        {
            _hubContext = hubContext;
        }

        public void Raise(Event e)
        {
            _hubContext.Clients.All.raise(e);
        }
    }
}