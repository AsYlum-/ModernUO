using System;

namespace Server.Tests
{
    public class ServerFixture : IDisposable
    {
        // Global setup
        static ServerFixture()
        {
            Core.Expansion = Expansion.EJ;

            // Load Configurations
            ServerConfiguration.Load(true);

            // Configure / Initialize
            TestMapDefinitions.ConfigureTestMapDefinitions();

            // Configure the world
            World.Configure();

            Timer.Initialize(0);

            // Load the world
            World.Load();
        }

        public void Dispose()
        {
            Timer.Initialize(0);
        }
    }
}
