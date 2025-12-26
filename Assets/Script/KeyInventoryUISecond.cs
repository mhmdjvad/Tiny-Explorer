using UnityEngine;
using UnityEngine.UIElements;

public class KeyInventoryUISecond : MonoBehaviour
{
    private VisualElement bedKeyUI, bathKeyUI, exitKeyUI;

    //For Doors
    private Label pressELabel, alertLabel;

    // Variables to store key status
    public bool hasBedKey = false;
    public bool hasBathKey = false;
    public bool hasExitKey = false;

    void OnEnable()
    {
        var doc = GetComponent<UIDocument>();
        if (doc == null) { Debug.LogError("UIDocument not found!"); return; }

        var root = doc.rootVisualElement;

        // Finding Elements (Assuming same UXML names as MainScene)
        bedKeyUI = root.Q<VisualElement>("BedKey");
        bathKeyUI = root.Q<VisualElement>("BathKey");
        exitKeyUI = root.Q<VisualElement>("ExitKey");

        pressELabel = root.Q<Label>("PressE");
        alertLabel = root.Q<Label>("Alert");

        if (pressELabel == null) Debug.LogError("PressE label not found in UI!");
        
        if (alertLabel == null) Debug.LogError("Alert label not found in UI!");

        // Initially hide all
        if(bedKeyUI != null) bedKeyUI.style.display = DisplayStyle.None;
        if(bathKeyUI != null) bathKeyUI.style.display = DisplayStyle.None;
        if(exitKeyUI != null) exitKeyUI.style.display = DisplayStyle.None;

        if(pressELabel != null) pressELabel.style.display = DisplayStyle.None;
        if(alertLabel != null) alertLabel.style.display = DisplayStyle.None;
    }

    public void ShowKeyOnUI(string keyTag)
    {
        if (keyTag == "BedRoom_key") { 
            if(bedKeyUI != null) bedKeyUI.style.display = DisplayStyle.Flex;
            hasBedKey = true;
        }
        else if (keyTag == "BathRoom_key") { 
            if(bathKeyUI != null) bathKeyUI.style.display = DisplayStyle.Flex;
            hasBathKey = true;
        }
        else if (keyTag == "ExitDoor_key") {
            if(exitKeyUI != null) exitKeyUI.style.display = DisplayStyle.Flex;
            hasExitKey = true;
        }
    }

    public void UseKey(string keyTag)
    {
        if (keyTag == "BedRoom_key" && bedKeyUI != null) bedKeyUI.style.opacity = 0.3f;
        else if (keyTag == "BathRoom_key" && bathKeyUI != null) bathKeyUI.style.opacity = 0.3f;
        else if (keyTag == "ExitDoor_key" && exitKeyUI != null) exitKeyUI.style.opacity = 0.3f;
    }

    public void ShowPressE(bool show) { 
        if(pressELabel != null) pressELabel.style.display = show ? DisplayStyle.Flex : DisplayStyle.None;
    }

    public void ShowAlert(bool show, string message = "")
    {
        Debug.Log(message);
        if (show && alertLabel != null) alertLabel.text = message;
        if(alertLabel != null) alertLabel.style.display = show ? DisplayStyle.Flex : DisplayStyle.None;
    }
}
