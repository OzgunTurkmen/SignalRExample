using ChatApp.API.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.API.DataAccess
{
    public class ChatAppContext:DbContext
    {

        public DbSet<User> User { get; set; }
        public ChatAppContext()
        {


        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=ChatApp;integrated security=True;MultipleActiveResultSets=True");
            base.OnConfiguring(optionsBuilder);
        }

       
    }
}
