using BepInEx;
using BepInEx.Configuration;
using BoplFixedMath;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlipperyPlatforms
{
    [BepInPlugin("com.000diggity000.SlipperyPlatforms", "Slippery Platforms", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        internal static ConfigFile config = new ConfigFile(Path.Combine(Paths.ConfigPath, "SlipperyPlatformsConfig.cfg"), true);
        internal static ConfigEntry<float> IntensityConfig;
        private void Awake()
        {
            Logger.LogInfo("Slippery Platforms Loaded");
            Harmony harmony = new Harmony("com.000diggity000.SlipperyPlatforms");
            harmony.PatchAll(typeof(Patches));
            IntensityConfig = config.Bind("SlipperyPlatforms", "Intensity", 8f, "Minimum is 0 and the maximum is 10");
            if(IntensityConfig.Value < 0)
            {
                IntensityConfig.Value = 0;
            }
            if(IntensityConfig.Value > 10)
            {
                IntensityConfig.Value = 10;
            }
        }
    }
    public class Patches
    {
        
        [HarmonyPatch(typeof(PlayerPhysics), "Awake")]
        [HarmonyPrefix]
        public static void PatchMove(PlayerPhysics __instance, ref Fix ___PlatformSlipperyness01, ref Fix ___IcePlatformSlipperyness01)
        {
            float intensity = Plugin.IntensityConfig.Value;
            intensity *= 0.05f;
            intensity += 0.5f;
            if (Plugin.IntensityConfig.Value == 0)
            {
                ___PlatformSlipperyness01 = (Fix)0.5f;
                ___IcePlatformSlipperyness01 = (Fix)0.5f;
            }
            else if(Plugin.IntensityConfig.Value == 10)
            {
                ___PlatformSlipperyness01 = (Fix)1f;
                ___IcePlatformSlipperyness01 = (Fix)1f;
            }
            else
            {
                ___PlatformSlipperyness01 = (Fix)intensity;
                ___IcePlatformSlipperyness01 = (Fix)intensity;
            }
            
        }
    }
}
