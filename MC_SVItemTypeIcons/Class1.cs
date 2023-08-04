
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace MC_SVItemTypeIcons
{
    [BepInPlugin(pluginGuid, pluginName, pluginVersion)]
    public class Main : BaseUnityPlugin
    {
        public const string pluginGuid = "mc.starvalor.itemtypeicons";
        public const string pluginName = "SV Item Type Icons";
        public const string pluginVersion = "1.0.3";

        private static WaypointMasterControl wmc = null;

        private static ManualLogSource log = BepInEx.Logging.Logger.CreateLogSource(pluginName);

        public void Awake()
        {
            Harmony.CreateAndPatchAll(typeof(Main));
        }

        [HarmonyPatch(typeof(Collectible), "Start")]
        [HarmonyPostfix]
        private static void CollectibleStart_Post(Collectible __instance, ref GameObject ___minimapIcon)
        {
            if (wmc == null)
                wmc = GameObject.FindGameObjectWithTag("MainCanvas").transform.Find("MinimapTemp").GetComponent<WaypointMasterControl>();

            if (wmc == null)
                return;

            if (__instance.itemType > 4 || (__instance.itemType == 3 && __instance.itemID == 24))
                return;

            ___minimapIcon.transform.localScale = new Vector3(
                    ___minimapIcon.transform.localScale.x * 2.5f,
                    ___minimapIcon.transform.localScale.y * 2.5f,
                    ___minimapIcon.transform.localScale.z);

            // Get the icon type and set additional scaling
            if (__instance != null)
            {
                int num = __instance.itemType;
                if (__instance.itemType <= 3)
                {
                    num = 1;
                }
                if (__instance.itemType == 3)
                {
                    if (__instance.itemID >= 2 && __instance.itemID <= 4)
                    {
                        num = 2;
                    }
                }

                GameObject go = new GameObject();

                SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
                sr.sprite = wmc.arrowObject.transform.GetChild(num).gameObject.GetComponent<Image>().sprite;
                go.transform.SetParent(___minimapIcon.transform.parent);
                go.transform.position = ___minimapIcon.transform.position;
                go.transform.localScale = ___minimapIcon.transform.localScale;
                go.transform.localEulerAngles = ___minimapIcon.transform.localEulerAngles;
                go.layer = ___minimapIcon.layer;
                go.SetActive(true);
                ___minimapIcon = go;
            }
        }
    }
}
