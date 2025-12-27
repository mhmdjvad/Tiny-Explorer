using UnityEngine.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class KeyInventoryUI : MonoBehaviour
{
    private VisualElement bedKeyUI, bathKeyUI, exitKeyUI;

    //For Doors
    private Label pressELabel, alertLabel;
    private VisualElement fadeOverlay;

    // متغیرهایی برای ذخیره داشتن یا نداشتن کلید
    public bool hasBedKey = false;
    public bool hasBathKey = false;
    public bool hasExitKey = false;
    
    [Header("Transition Settings")]
    public AudioClip transitionSound;

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

        // Create Fade Overlay Programmatically
        fadeOverlay = new VisualElement();
        fadeOverlay.style.position = Position.Absolute;
        fadeOverlay.style.width = Length.Percent(100);
        fadeOverlay.style.height = Length.Percent(100);
        fadeOverlay.style.backgroundColor = Color.white;
        fadeOverlay.style.opacity = 0f;
        fadeOverlay.pickingMode = PickingMode.Ignore; // Don't block clicks while invisible
        root.Add(fadeOverlay);
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

    public void UseKey(string keyTag)
    {
        if (keyTag == "BedRoom_key" && bedKeyUI != null) bedKeyUI.style.opacity = 0.3f;
        else if (keyTag == "BathRoom_key" && bathKeyUI != null) bathKeyUI.style.opacity = 0.3f;
        else if (keyTag == "ExitDoor_key" && exitKeyUI != null) exitKeyUI.style.opacity = 0.3f;
    }

    public void StartSceneTransition()
    {
        if (transitionSound != null)
        {
            AudioSource.PlayClipAtPoint(transitionSound, Camera.main.transform.position);
        }
        
        fadeOverlay.pickingMode = PickingMode.Position; // Block interaction
        StartCoroutine(FadeToWhiteAndLoad());
    }

    private IEnumerator FadeToWhiteAndLoad()
    {
        float duration = 2.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            fadeOverlay.style.opacity = Mathf.Lerp(0f, 1f, elapsed / duration);
            yield return null;
        }

        fadeOverlay.style.opacity = 1f;
        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene("secendscene");
    }
}