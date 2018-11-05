using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SelfDestroyer
{
    public class CommandSelfDestroy : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "selfdestroy";

        public string Help => "";

        public string Syntax => "";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Length == 0)
            {
                UnturnedPlayer p = (UnturnedPlayer)caller;
                var all = TransformFinder.Own(p.CSteamID);
                int count = all.Count;
                if (count == 0)
                    UnturnedChat.Say(caller, Plugin.Instance.Translate("self_destroy_1"));
                else
                {
                    UnturnedChat.Say(caller, Plugin.Instance.Translate("self_destroy_0", count));
                    Plugin.Instance.Queries.Remove(p.CSteamID.m_SteamID);
                    Plugin.Instance.Queries.Add(p.CSteamID.m_SteamID, all);
                }
            }
            else if (command.Length == 1)
            {
                UnturnedPlayer p = (UnturnedPlayer)caller;
                if (float.TryParse(command[0], out float dist))
                {
                    var all = TransformFinder.Own(p.CSteamID).Where(x => Vector3.Distance(p.Position, x.position) <= dist).ToList();
                    int count = all.Count;
                    if (count == 0)
                        UnturnedChat.Say(caller, Plugin.Instance.Translate("self_destroy_1"));
                    else
                    {
                        UnturnedChat.Say(caller, Plugin.Instance.Translate("self_destroy_0", count));
                        Plugin.Instance.Queries.Remove(p.CSteamID.m_SteamID);
                        Plugin.Instance.Queries.Add(p.CSteamID.m_SteamID, all);
                    }
                }
                else if (command[0].ToLower() == "confirm")
                {
                    if (Plugin.Instance.Queries.TryGetValue(p.CSteamID.m_SteamID, out List<Transform> list))
                    {
                        UnturnedChat.Say(caller, Plugin.Instance.Translate("self_destroy_2"));
                        foreach (var v in list)
                        {
                            DamageTool.damage(v, false, 100000, 1, out EPlayerKill kill);
                            DamageTool.damage(v, false, v.position, 100000, 1, out EPlayerKill kill1);
                        }
                        UnturnedChat.Say(caller, Plugin.Instance.Translate("self_destroy_3"));
                        Plugin.Instance.Queries.Remove(p.CSteamID.m_SteamID);
                    }
                    else
                        UnturnedChat.Say(caller, Plugin.Instance.Translate("invalid_args"));
                }
                else if (command[0].ToLower() == "abort")
                {
                    Plugin.Instance.Queries.Remove(p.CSteamID.m_SteamID);
                    UnturnedChat.Say(caller, Plugin.Instance.Translate("self_destroy_4"));
                }
                else
                    UnturnedChat.Say(caller, Plugin.Instance.Translate("invalid_args"));
            }
            else
            {
                UnturnedChat.Say(caller, Plugin.Instance.Translate("invalid_args"));
            }
        }
    }
}
