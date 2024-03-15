using Microsoft.AspNetCore.SignalR;
using ticketsRequerimientosBackend.Models;

namespace ticketsRequerimientosBackend
{
    public class MensajeHUB : Hub
    {
        public async Task SendMessageHub(MensajeriaTicket msj, IEnumerable<object> respuesta)
        {
            await Clients.All.SendAsync("SendMessageHub", msj, respuesta);
        }
    }
}
