using Microsoft.EntityFrameworkCore;
using Threads.MVVM.Models;

namespace Threads
{
    public class ThreadsDbContext : DbContext
    {
        public DbSet<Thread> Threads { get; set; }
        public DbSet<ThreadType> ThreadTypes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder modelBuilder)
            => modelBuilder.UseSqlite("Data Source=ThreadsDb.sqlite");
    }
}
