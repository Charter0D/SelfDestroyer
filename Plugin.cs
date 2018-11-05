using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Steamworks;
using System.Collections.Generic;
using UnityEngine;

namespace SelfDestroyer
{
    public class Plugin : RocketPlugin
    {
        public static Plugin Instance;

        public Dictionary<ulong, List<Transform>> Queries;

        protected override void Load()
        {
            Instance = this;
            Queries = new Dictionary<ulong, List<Transform>>();
        }

        protected override void Unload()
        {
            base.Unload();
        }

        public override TranslationList DefaultTranslations => new TranslationList()
        {
            {"invalid_args", "Invalid arguments." },
            {"self_destroy_0", "Found {0} objects. Type /selfdestroy confirm to destroy it, or /selfdestroy abort to abort." },
            {"self_destroy_1", "Could not find any objects." },
            {"self_destroy_2", "Confirmed. Destroying..." },
            {"self_destroy_3", "Destroying successful." },
            {"self_destroy_4", "Aborted." },
        };
    }
}
