
using Microsoft.AspNetCore.SignalR;

namespace ticketsRequerimientosBackend
{
    public class TicketResolucionHUB : Hub
    {
        public async Task SendTicketRequerimiento(string estado)
        {
            await Clients.All.SendAsync("SendTicketRequerimiento", estado);
        }
    }
}

