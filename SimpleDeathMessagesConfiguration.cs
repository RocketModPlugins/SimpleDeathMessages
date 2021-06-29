using Rocket.API;

namespace coolpuppy24.simpledeathmessages
{
    public class SimpleDeathMessagesConfiguration : IRocketPluginConfiguration
    {
        //public string DeathMessagesColor = "Red";
        public bool ShowSuicideMSG = true;

        public void LoadDefaults() { }
    }
}

