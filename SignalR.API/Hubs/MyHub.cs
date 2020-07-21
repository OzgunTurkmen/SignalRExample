using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;
using SignalR.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.API.Hubs
{
    public class MyHub : Hub
    {
        private Manager _manager;

        public MyHub()
        {
            _manager = new Manager();
        }

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

        public async Task AddToGroup(string teamName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, teamName);
        }

        public async Task RemoveToGroup(string teamName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, teamName);
        }

        public async Task SendNameByGroup(string name, string teamName)
        {
            var team = Manager.Teams.Where(x => x.Name == teamName).FirstOrDefault();

            var lastUser = Manager.Users.LastOrDefault();


            var user = new User() { Name = name };

            if (lastUser != null)
            {
                user.Id = lastUser.Id + 1;
            }
            else
            {
                user.Id = 1;
            }

            if (team != null)
            {
                team.Users.Add(user);
            }
            else
            {
                var newTeam = new Team() { Name = teamName };
                newTeam.Users.Add(user);
            }

            await Clients.Group(teamName).SendAsync("ReceiveMessageByGroup");
        }

        public async Task GetNamesByGroup()
        {
            var teams = Manager.Teams;

            await Clients.All.SendAsync("ReceiveNamesByGroup", teams);
        }
    }
}
