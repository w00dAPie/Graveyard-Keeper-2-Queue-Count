using HarmonyLib;
using TMPro;
using UnityEngine;

namespace GK2QueueCount.Patches
{
    [HarmonyPatch(typeof(UICraftHintWidget))]
    internal static class CraftHintPatch
    {
        private const string QueueLabelName = "GK2QueueCount_QueueLabel";

        [HarmonyPostfix]
        [HarmonyPatch("RedrawCraftHint", new System.Type[] { })]
        private static void RedrawCraftHintPostfix(UICraftHintWidget __instance)
        {
            if (__instance == null)
                return;

            UICraftHintWidgetData data =
                Traverse.Create(__instance)
                    .Field("data")
                    .GetValue<UICraftHintWidgetData>();

            if (data?.CraftComponent?.CurrentCraftElement == null)
                return;

            CraftElementBase craft =
                data.CraftComponent.CurrentCraftElement;

            UIItemCell craftResultItem =
                Traverse.Create(__instance)
                    .Field("craftResultItem")
                    .GetValue<UIItemCell>();

            if (craftResultItem == null)
                return;

            TMP_Text originalCountLabel =
                Traverse.Create(craftResultItem)
                    .Field("countLabel")
                    .GetValue<TMP_Text>();

            if (originalCountLabel == null)
                return;

            TMP_Text queueLabel =
                GetOrCreateQueueLabel(
                    craftResultItem,
                    originalCountLabel
                );

            queueLabel.text = $"×{craft.Count}";
            queueLabel.gameObject.SetActive(true);
        }

        private static TMP_Text GetOrCreateQueueLabel(
            UIItemCell craftResultItem,
            TMP_Text originalCountLabel)
        {
            // Direkt am Icon verankern.
            Transform parent = craftResultItem.Icon.transform;

            Transform existing = parent.Find(QueueLabelName);

            if (existing != null)
            {
                TMP_Text existingLabel = existing.GetComponent<TMP_Text>();

                if (existingLabel != null)
                    return existingLabel;
            }

            GameObject obj = new GameObject(
                QueueLabelName,
                typeof(RectTransform),
                typeof(TextMeshProUGUI)
            );

            obj.transform.SetParent(parent, false);
            obj.transform.SetAsLastSibling();

            TextMeshProUGUI queueLabel =
                obj.GetComponent<TextMeshProUGUI>();

            // Optik vom originalen GK2-Counter übernehmen.
            queueLabel.font = originalCountLabel.font;
            queueLabel.fontSharedMaterial =
                originalCountLabel.fontSharedMaterial;

            queueLabel.fontSize =
                originalCountLabel.fontSize;

            queueLabel.fontStyle =
                originalCountLabel.fontStyle;

            queueLabel.color =
                originalCountLabel.color;

            queueLabel.raycastTarget = false;

            queueLabel.textWrappingMode =
                TextWrappingModes.NoWrap;

            queueLabel.overflowMode =
                TextOverflowModes.Overflow;

            queueLabel.alignment =
                TextAlignmentOptions.TopLeft;

            RectTransform rect = queueLabel.rectTransform;

            // Oben links innerhalb des Icons.
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(0f, 1f);
            rect.pivot = new Vector2(0f, 1f);

            rect.anchoredPosition = new Vector2(5f, -5f);
            rect.sizeDelta = new Vector2(50f, 22f);
            
            return queueLabel;
        }
    }
}