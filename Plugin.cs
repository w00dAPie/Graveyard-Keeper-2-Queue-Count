using BepInEx;
using HarmonyLib;

namespace GK2QueueCount
{
    [BepInPlugin(PluginGuid, PluginName , PluginVersion)]
    public class Plugin : BaseUnityPlugin
    {
        private Harmony _harmony;

        public const string PluginGuid = "de.w00dst0ckOo.gk2.queuecount";
        public const string PluginName = "GK2 Queue Count";
        public const string PluginVersion = "0.3.1";

        private void Awake()
        {
            Logger.LogInfo($"{PluginName} {PluginVersion} loading...");

            _harmony = new Harmony(PluginGuid);
            _harmony.PatchAll();

            Logger.LogInfo($"{PluginName} {PluginVersion} loaded.");
        }

        private void OnDestroy()
        {
            _harmony?.UnpatchSelf();
        }
    }
}

