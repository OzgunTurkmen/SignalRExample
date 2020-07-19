using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.API.Hubs
{
    public class MyHub : Hub
    {
        //statik olmazsa her istek yapıldığında yenilenir.
        public static List<string> Messages { get; set; } = new List<string>();

        public async Task SendMessage(string message)
        {
            Messages.Add(message);
            await Clients.All.SendAsync("ReceiveMessage", message + "test");
        }

        public async Task GetMessages()
        {
            await Clients.All.SendAsync("ReceiveMessages", Messages);
        }
    }
}
