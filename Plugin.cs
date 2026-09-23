using BepInEx;
using HarmonyLib;

namespace GK2QueueCount
{
    [BepInPlugin(
        "de.w00dst0ckOo.gk2.queuecount",
        "GK2 Queue Count",
        "0.3.0"
    )]
    public class Plugin : BaseUnityPlugin
    {
        private Harmony _harmony;

        private void Awake()
        {
            Logger.LogInfo("GK2 Queue Count 0.3.0 loading...");

            _harmony = new Harmony("de.w00dst0ckOo.gk2.queuecount");
            _harmony.PatchAll();

            Logger.LogInfo("GK2 Queue Count loaded.");
        }

        private void OnDestroy()
        {
            _harmony?.UnpatchSelf();
        }
    }
}