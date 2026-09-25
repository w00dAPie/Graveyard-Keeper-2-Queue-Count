using System.Runtime.CompilerServices;
using HarmonyLib;
using TMPro;
using UnityEngine;

namespace GK2QueueCount.Patches
{
    [HarmonyPatch(typeof(UICraftHintWidget))]
    internal static class CraftHintPatch
    {
        private const string QueueLabelName = "GK2QueueCount_QueueLabel";
        private static readonly AccessTools.FieldRef<
            UICraftHintWidget,
            UICraftHintWidgetData
        > ReadData = AccessTools.FieldRefAccess<UICraftHintWidget, UICraftHintWidgetData>("data");
        private static readonly AccessTools.FieldRef<UICraftHintWidget, UIItemCell> ReadResultItem =
            AccessTools.FieldRefAccess<UICraftHintWidget, UIItemCell>("craftResultItem");
        private static readonly AccessTools.FieldRef<UIItemCell, TextMeshProUGUI> ReadCountLabel =
            AccessTools.FieldRefAccess<UIItemCell, TextMeshProUGUI>("countLabel");
        private static readonly ConditionalWeakTable<UICraftHintWidget, WidgetState> States = new();

        private sealed class WidgetState
        {
            public UIItemCell ResultItem;
            public Transform Icon;
            public TMP_Text Label;
            public int LastCount;
            public bool HasCount;
        }

        [HarmonyPostfix]
        [HarmonyPatch("RedrawCraftHint", new System.Type[] { })]
        private static void RedrawCraftHintPostfix(UICraftHintWidget __instance)
        {
            if (__instance == null)
                return;

            // The accessor is cached, but pooled widgets must always use their current data.
            UICraftHintWidgetData data = ReadData(__instance);

            if (data?.CraftComponent?.CurrentCraftElement == null)
                return;

            UIItemCell craftResultItem = ReadResultItem(__instance);

            if (craftResultItem == null)
                return;

            TMP_Text originalCountLabel = ReadCountLabel(craftResultItem);

            if (originalCountLabel == null)
                return;

            if (craftResultItem.Icon == null)
                return;

            Transform icon = craftResultItem.Icon.transform;
            if (!States.TryGetValue(__instance, out WidgetState state))
            {
                state = new WidgetState();
                States.Add(__instance, state);
            }

            // Unity's null checks also detect destroyed native objects. Re-resolve after
            // replacement/reparenting, without retaining widgets through instance IDs.
            if (
                state.ResultItem != craftResultItem
                || state.Icon != icon
                || state.Label == null
                || state.Label.transform.parent != icon
            )
            {
                state.ResultItem = craftResultItem;
                state.Icon = icon;
                state.Label = GetOrCreateQueueLabel(craftResultItem, originalCountLabel);
                state.HasCount = false;
            }
            TMP_Text queueLabel = state.Label;

            int remainingCrafts = 0;

            CraftElementBase currentCraft = data.CraftComponent.CurrentCraftElement;

            foreach (CraftElementBase queueElement in data.CraftComponent.CraftElementsQueue)
            {
                if (queueElement != null && queueElement.CraftId == currentCraft.CraftId)
                {
                    remainingCrafts += queueElement.Count;
                }
            }

            if (!state.HasCount || state.LastCount != remainingCrafts)
            {
                queueLabel.text = $"×{remainingCrafts}";
                state.LastCount = remainingCrafts;
                state.HasCount = true;
            }

            // Read actual state so pooling or another lifecycle callback cannot stale a cache.
            bool visible = remainingCrafts > 0;
            if (queueLabel.gameObject.activeSelf != visible)
                queueLabel.gameObject.SetActive(visible);
        }

        private static TMP_Text GetOrCreateQueueLabel(
            UIItemCell craftResultItem,
            TMP_Text originalCountLabel
        )
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

            TextMeshProUGUI queueLabel = obj.GetComponent<TextMeshProUGUI>();

            // Optik vom originalen GK2-Counter übernehmen.
            queueLabel.font = originalCountLabel.font;

            queueLabel.fontSharedMaterial = originalCountLabel.fontSharedMaterial;

            queueLabel.fontSize = originalCountLabel.fontSize;

            queueLabel.fontStyle = originalCountLabel.fontStyle;

            queueLabel.color = originalCountLabel.color;

            queueLabel.raycastTarget = false;

            queueLabel.textWrappingMode = TextWrappingModes.NoWrap;

            queueLabel.overflowMode = TextOverflowModes.Overflow;

            queueLabel.alignment = TextAlignmentOptions.TopLeft;

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
