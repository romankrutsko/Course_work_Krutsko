using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportWebAplication.Models
{
    public class PlayersContext: DbContext
    {
        public DbSet<TopScorers> Players { get; set; }
        public PlayersContext(DbContextOptions<PlayersContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
