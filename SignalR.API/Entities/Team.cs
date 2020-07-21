using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.API.Entities
{
    public class Team
    {
        public Team()
        {
            Users = new List<User>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public IList<User> Users { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }

    public class Manager
    {
        public static List<Team> Teams { get; set; } = new List<Team>();
        public static List<User> Users { get; set; } = new List<User>();

        public Team AddToTeam(Team team)
        {
            Teams.Add(team);
            return team;
        }
    }
}
