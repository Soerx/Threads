using System.Windows;

namespace Threads
{
    public partial class App : Application
    {
        public static App CurrentInstance => (App)Current;

        public ThreadsDbContext Context { get; }

        public App() => Context = new ThreadsDbContext();
    }
}
