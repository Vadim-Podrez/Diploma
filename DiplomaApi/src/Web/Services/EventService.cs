using DiplomaApi.Application.Events;

namespace DiplomaApi.Web.Services;
using Microsoft.AspNetCore.SignalR;
using Web.Hubs;

public class EventService
{
    private readonly IHubContext<EventHub> _hubContext;

    public EventService(IHubContext<EventHub> hubContext)
    {
        _hubContext = hubContext;
    }
    
    public async Task OnEventReceived(EventDto eventData)
    {
        // 1. Зберегти eventData у БД (додаєш тут)
        // ...

        // 2. Відправити на Dashboard
        await _hubContext.Clients.All.SendAsync("ReceiveEvent", eventData);
    }
    
}
