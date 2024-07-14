using HLTV.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace HLTV.Api.Contexts
{
    public class HltvContext : DbContext
    {

        public HltvContext(DbContextOptions<HltvContext> options)
            : base(options)
        {
        }

        public DbSet<Ranking> Rankings { get; set; }



    }
}
