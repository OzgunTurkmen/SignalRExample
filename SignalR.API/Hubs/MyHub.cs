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
        public static int TotalClient { get; set; } = 0;
        public static int TeamCount { get; set; } = 7;

        public async Task SendMessage(string message)
        {
            if (Messages.Count >= TeamCount)
            {
                await Clients.Caller.SendAsync("Error", "Takım en fazla " + TeamCount + " kişi kadar olabilir.");
            }
            else
            {
                Messages.Add(message);
                await Clients.All.SendAsync("ReceiveMessage", message + "test");
            }


        }

        public async Task GetMessages()
        {
            await Clients.All.SendAsync("ReceiveMessages", Messages);
        }

        //clientlar bağlandıktça çalışır.
        public async override Task OnConnectedAsync()
        {
            TotalClient++;
            await Clients.All.SendAsync("ReceiveClientCount", TotalClient);
            await base.OnConnectedAsync();
        }

        //clientlar disconnect oldukça çalışır.
        public async override Task OnDisconnectedAsync(Exception exception)
        {
            TotalClient--;
            await Clients.All.SendAsync("ReceiveClientCount", TotalClient);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
