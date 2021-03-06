﻿using Rocket.Core.Plugins;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Rocket.Unturned.Chat;
using Rocket.API;
using Rocket.Unturned;
using UnityEngine;

namespace BlackListedDrivers
{
    public class Main : RocketPlugin<BlackListedDriversConfiguration>
    {
        public static Main Instance;
        public static List<CSteamID> Blacklisted = new List<CSteamID>();

        protected override void Load()
        {
            Instance = this;
            Rocket.Core.Logging.Logger.Log("Blacklisted Drivers loaded! ~~ Alexr03!");

            Rocket.Unturned.Events.UnturnedPlayerEvents.OnPlayerUpdateStance += UpdatedStance;

            U.Events.OnShutdown += OnShutdown;
        }

        private void OnShutdown()
        {
            throw new NotImplementedException();
        }

        private void UpdatedStance(UnturnedPlayer player, byte stance)
        {
            if(stance == 6)
            {
                if (Blacklisted.Contains(player.CSteamID))
                {
                    player.CurrentVehicle.forceRemovePlayer(out byte seat, player.CSteamID, out Vector3 point, out byte angle);
                    UnturnedChat.Say(player.CSteamID, "You have been blacklisted from driving.");
                }

                if (Configuration.Instance.GroupBlacklisting)
                {
                    IRocketPlayer player2 = (IRocketPlayer)player;

                    if (player2.HasPermission("driver.group"))
                    {
                        player.CurrentVehicle.forceRemovePlayer(out byte seat, player.CSteamID, out Vector3 point, out byte angle);
                        UnturnedChat.Say(player.CSteamID, "You have been blacklisted from driving.");

                        Provider.map = "Washington";
                    }
                }
            }
        }

        protected override void Unload()
        {
            Rocket.Core.Logging.Logger.Log("Blacklisted Drivers unloaded! ~~ Alexr03!");
        }

        private void FixedUpdate()
        {
        }
    }
}
