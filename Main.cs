using Rocket.Unturned.Player;
using SDG.Unturned;
using Rocket.Core.Plugins;
using Rocket.API.Collections;
using Rocket.Unturned.Events;
using Rocket.Unturned.Chat;
using Rocket.Core.Logging;
using Color = UnityEngine.Color;
using Steamworks;

namespace coolpuppy24.simpledeathmessages
{
    public class Main : RocketPlugin<SimpleDeathMessagesConfiguration>
    {
        protected override void Load()
        {
            UnturnedPlayerEvents.OnPlayerDeath += OnPlayerDeath;
           Logger.Log("Successfully Loaded!");
        }

        protected override void Unload()
        {
            UnturnedPlayerEvents.OnPlayerDeath -= OnPlayerDeath;
            Logger.Log("Unloaded!");
        }

        public override TranslationList DefaultTranslations =>
            new TranslationList
            {
                {"gun_headshot","{1} [GUN - {3}] {2} {0}"},
                {"gun","{1} [GUN - {2}] {0}"},
                {"food","[FOOD] {0}"},
                {"arena","[ARENA] {0}"},
                {"shred","[SHRED] {0}"},
                {"punch_headshot","{1} [PUNCH] {2} {0}"},
                {"punch","{0} [PUNCH] {1}"},
                {"bones","[BONES] {0}"},
                {"melee_headshot","{1} [MELEE - {3}] {2} {0}"},
                {"melee","{0} [MELEE- {2}] {1}"},
                {"water","[WATER] {0}"},
                {"breath","[BREATH] {0}"},
                {"zombie","[ZOMBIE] {0}"},
                {"animal","[ANIMAL] {0}"},
                {"grenade","[GRENADE] {0}"},
                {"vehicle","[VEHICLE] {0}"},
                {"suicide","[SUICIDE] {0}"},
                {"burning","[BURNING] {0}"},
                {"headshot","+ [HEADSHOT]" },
                {"landmine","[LANDMINE] {0}"},
                {"roadkill","{1} [ROADKILL] {0}"},
                {"bleeding","[BLEEDING] {0}"},
                {"freezing","[FREEZING] {0}"},
                {"sentry","[SENTRY] {0}"},
                {"charge","[CHARGE] {0}"},
                {"missile","[MISSILE] {0}"},
                {"splash","[SPLASH] {0}"},
                {"acid","[ACID] {0}"},
                {"spark", "[SPARK] {0}"},
                {"infection", "[INFECTION] {0}"},
                {"spit","[SPIT] {0}"},
                {"kill","[ADMIN KILL] {0}"},
                {"boulder","[BOULDER] {0}"}
            };

        private void OnPlayerDeath(UnturnedPlayer player, EDeathCause cause, ELimb limb, CSteamID murderer)
        {
            var deathMessageColor = Color.red;

            var killer = UnturnedPlayer.FromCSteamID(murderer);

            var headshot = Translate("headshot");
            if (cause.ToString() == "SHRED" || cause.ToString() == "ZOMBIE" || cause.ToString() == "ANIMAL" ||
                cause.ToString() == "SPARK" || cause.ToString() == "VEHICLE" || cause.ToString() == "FOOD" ||
                cause.ToString() == "WATER" || cause.ToString() == "INFECTION" || cause.ToString() == "BLEEDING" ||
                cause.ToString() == "LANDMINE" || cause.ToString() == "BREATH" || cause.ToString() == "KILL" ||
                cause.ToString() == "FREEZING" || cause.ToString() == "SENTRY" || cause.ToString() == "CHARGE" ||
                cause.ToString() == "MISSILE" || cause.ToString() == "BONES" || cause.ToString() == "SPLASH" ||
                cause.ToString() == "ACID" || cause.ToString() == "SPIT" || cause.ToString() == "BURNING" ||
                cause.ToString() == "BURNER" || cause.ToString() == "BOULDER" || cause.ToString() == "ARENA" ||
                cause.ToString() == "GRENADE" ||
                (Configuration.Instance.ShowSuicideMSG && cause.ToString() == "SUICIDE") ||
                cause.ToString() == "ROADKILL" || cause.ToString() == "MELEE" || cause.ToString() == "GUN" ||
                cause.ToString() == "PUNCH")
            {
                if (cause.ToString() != "ROADKILL" && cause.ToString() != "MELEE" && cause.ToString() != "GUN" &&
                    cause.ToString() != "PUNCH")
                {
                    UnturnedChat.Say(Translate(cause.ToString().ToLower(), player.DisplayName), deathMessageColor);
                }
                else switch (cause.ToString())
                {
                    case "ROADKILL":
                        UnturnedChat.Say(Translate("roadkill", player.DisplayName, killer.DisplayName), deathMessageColor);
                        break;
                    case "MELEE":
                    case "GUN":
                        UnturnedChat.Say(
                            limb == ELimb.SKULL
                                ? Translate(cause.ToString().ToLower() + "_headshot", player.DisplayName,
                                    killer.DisplayName, headshot, killer.Player.equipment.asset.itemName)
                                : Translate(cause.ToString().ToLower(), player.DisplayName, killer.DisplayName, headshot,
                                    killer.Player.equipment.asset.itemName), deathMessageColor);
                        break;
                    case "PUNCH":
                        UnturnedChat.Say(Translate(limb == ELimb.SKULL ? "punch_headshot" : "punch", player.DisplayName, killer.DisplayName, headshot), deathMessageColor);
                        break;
                }

                return;
            }

            if (Translate(cause.ToString().ToLower()) != null)
            {
                UnturnedChat.Say(
                    Translate(cause.ToString().ToLower()).Contains("{1}")
                        ? Translate(cause.ToString().ToLower(), player.DisplayName, killer.DisplayName, headshot)
                        : Translate(cause.ToString().ToLower(), player.DisplayName, headshot), deathMessageColor);

                return;
            }

            Logger.LogError("Please add translation for " + cause +
                            " | Parameters for custom translation: {0} = Player , {1} = Killer");
        }
    }
}
