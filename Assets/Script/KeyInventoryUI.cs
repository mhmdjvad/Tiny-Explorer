using UnityEngine;
using UnityEngine.UIElements;

public class KeyInventoryUI : MonoBehaviour
{
    private VisualElement bedKeyUI, bathKeyUI, exitKeyUI;

    //For Doors
    private Label pressELabel, alertLabel;

    // متغیرهایی برای ذخیره داشتن یا نداشتن کلید
    public bool hasBedKey = false;
    public bool hasBathKey = false;
    public bool hasExitKey = false;

    void OnEnable()
    {
        var doc = GetComponent<UIDocument>();
        if (doc == null) { Debug.LogError("UIDocument پیدا نشد!"); return; }

        var root = doc.rootVisualElement;

        // پیدا کردن المنت‌ها (دقیقاً با نام‌هایی که در UXML گذاشتی)
        bedKeyUI = root.Q<VisualElement>("BedKey");
        bathKeyUI = root.Q<VisualElement>("BathKey");
        exitKeyUI = root.Q<VisualElement>("ExitKey");

        pressELabel = root.Q<Label>("PressE");
        alertLabel = root.Q<Label>("Alert");

        if (pressELabel == null) Debug.LogError("لیبل PressE در فایل UI پیدا نشد! نام را چک کن.");
        else Debug.Log("لیبل PressE با موفقیت پیدا شد.");

        if (alertLabel == null) Debug.LogError("لیبل Alert در فایل UI پیدا نشد! نام را چک کن.");

        // در ابتدا همه را مخفی می‌کنیم
        bedKeyUI.style.display = DisplayStyle.None;
        bathKeyUI.style.display = DisplayStyle.None;
        exitKeyUI.style.display = DisplayStyle.None;

        pressELabel.style.display = DisplayStyle.None;
        alertLabel.style.display = DisplayStyle.None;
    }

    // این متد توسط کلیدها صدا زده می‌شود
    public void ShowKeyOnUI(string keyTag)
    {
        if (keyTag == "BedRoom_key") { 
            bedKeyUI.style.display = DisplayStyle.Flex;
            hasBedKey = true;
        }
        else if (keyTag == "BathRoom_key") { 
            bathKeyUI.style.display = DisplayStyle.Flex;
            hasBathKey = true;
        }
        else if (keyTag == "ExitDoor_key") {
            exitKeyUI.style.display = DisplayStyle.Flex;
            hasExitKey = true;
        }
    }

    public void ShowPressE(bool show) { 
        pressELabel.style.display = show ? DisplayStyle.Flex : DisplayStyle.None;
        //Debug.Log("Press E");
    }

    public void ShowAlert(bool show, string message = "")
    {
        Debug.Log(message);
        if (show) alertLabel.text = message;
        alertLabel.style.display = show ? DisplayStyle.Flex : DisplayStyle.None;
    }
}