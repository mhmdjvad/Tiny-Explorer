using UnityEngine;
using UnityEngine.UIElements;

public class KeyInventoryUI : MonoBehaviour
{
    private VisualElement bedKeyUI, bathKeyUI, exitKeyUI;

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        // پیدا کردن المنت‌ها (دقیقاً با نام‌هایی که در UXML گذاشتی)
        bedKeyUI = root.Q<VisualElement>("BedKey");
        bathKeyUI = root.Q<VisualElement>("BathKey");
        exitKeyUI = root.Q<VisualElement>("ExitKey");

        // در ابتدا همه را مخفی می‌کنیم
        bedKeyUI.style.display = DisplayStyle.None;
        bathKeyUI.style.display = DisplayStyle.None;
        exitKeyUI.style.display = DisplayStyle.None;
    }

    // این متد توسط کلیدها صدا زده می‌شود
    public void ShowKeyOnUI(string keyTag)
    {
        if (keyTag == "BedRoom_key") bedKeyUI.style.display = DisplayStyle.Flex;
        else if (keyTag == "BathRoom_key") bathKeyUI.style.display = DisplayStyle.Flex;
        else if (keyTag == "ExitDoor_key") exitKeyUI.style.display = DisplayStyle.Flex;
    }
}