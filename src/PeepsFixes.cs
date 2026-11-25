using System;
using System.Text;
using Newtonsoft.Json.Linq;
using Vintagestory.API.Common;

namespace PeepsFixes
{
    // Fixes that must run after every mod's JSON patches have been applied (the game applies those at 0.05,
    // and turns the assets into items and blocks at 0.2). Each fix only runs if the mod it repairs is installed,
    // and only changes anything if the problem is actually there.
    public class PeepsFixesSystem : ModSystem
    {
        private const StringComparison IC = StringComparison.OrdinalIgnoreCase;

        public override double ExecuteOrder() => 0.11;

        public override void AssetsLoaded(ICoreAPI api)
        {
            // PetAI replaces the vegetable's whole behaviour list on the server, which removes ground storage.
            if (api.ModLoader.IsModEnabled("petai"))
                TryFix(api, "game:itemtypes/food/vegetable.json", RestoreGroundStorable);

            // Expanded Foods keeps a shadow overlay that is a different size from its new texture, so the game skips it and logs a warning.
            if (api.ModLoader.IsModEnabled("expandedfoods"))
            {
                TryFix(api, "game:blocktypes/wood/trough-large.json", t => RemoveMismatchedOverlay(t, "contents-egg", "trough/small/egg", "trough/large/shadow"));
                TryFix(api, "game:blocktypes/wood/trough-small.json", t => RemoveMismatchedOverlay(t, "contents-peanut", "trough/small/peanut", "trough/small/shadow"));
            }
        }

        private void TryFix(ICoreAPI api, string path, System.Func<JToken, bool> fix)
        {
            try
            {
                IAsset asset = api.Assets.TryGet(new AssetLocation(path));
                if (asset == null || asset.Data == null) return;
                string text = Encoding.UTF8.GetString(asset.Data).TrimStart('﻿');
                JToken token = JToken.Parse(text);
                if (fix(token))
                {
                    asset.Data = Encoding.UTF8.GetBytes(token.ToString());
                    Mod.Logger.Notification("Applied fix to {0}", path);
                }
            }
            catch (Exception e)
            {
                Mod.Logger.Warning("Could not apply fix to {0}: {1}", path, e.Message);
            }
        }

        private static bool RestoreGroundStorable(JToken token)
        {
            JObject root = token as JObject;
            if (root == null) return false;

            JArray behaviors = root.GetValue("behaviors", IC) as JArray;
            if (behaviors == null)
            {
                JProperty old = FindProp(root, "behaviors");
                if (old != null) old.Remove();
                behaviors = new JArray();
                root["behaviors"] = behaviors;
            }

            foreach (JToken b in behaviors)
            {
                JObject o = b as JObject;
                if (o == null) continue;
                JToken name = o.GetValue("name", IC);
                if (name != null && string.Equals(name.ToString(), "GroundStorable", IC)) return false;
            }

            behaviors.Add(JObject.Parse("{\"name\":\"GroundStorable\",\"properties\":{\"layout\":\"Quadrants\",\"collisionBox\":{\"x1\":0,\"y1\":0,\"z1\":0,\"x2\":1,\"y2\":0.125,\"z2\":1}}}"));
            return true;
        }

        private static bool RemoveMismatchedOverlay(JToken token, string textureCode, string baseContains, string overlayContains)
        {
            JObject root = token as JObject;
            JObject textures = root?.GetValue("textures", IC) as JObject;
            JObject entry = textures?.GetValue(textureCode, IC) as JObject;
            if (entry == null) return false;

            JToken baseTex = entry.GetValue("base", IC);
            if (baseTex == null || baseTex.ToString().IndexOf(baseContains, IC) < 0) return false;

            JProperty overlays = FindProp(entry, "overlays");
            if (overlays == null || overlays.Value.ToString().IndexOf(overlayContains, IC) < 0) return false;

            overlays.Remove();
            return true;
        }

        private static JProperty FindProp(JObject obj, string name)
        {
            foreach (JProperty p in obj.Properties()) if (string.Equals(p.Name, name, IC)) return p;
            return null;
        }
    }
}
