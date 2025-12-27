using UnityEngine.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class KeyInventoryUISecond : MonoBehaviour
{
    private VisualElement bedKeyUI, bathKeyUI, exitKeyUI;

    //For Doors
    private Label pressELabel, alertLabel;
    private VisualElement fadeOverlay;

    // Variables to store key status
    public bool hasBedKey = false;
    public bool hasBathKey = false;
    public bool hasExitKey = false;

    [Header("Transition Settings")]
    public AudioClip transitionSound;

    void Start()
    {
        StartCoroutine(FadeInFromWhite());
    }

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

        // Create Fade Overlay for Fade-In effect
        fadeOverlay = new VisualElement();
        fadeOverlay.style.position = Position.Absolute;
        fadeOverlay.style.width = Length.Percent(100);
        fadeOverlay.style.height = Length.Percent(100);
        fadeOverlay.style.backgroundColor = Color.white;
        fadeOverlay.style.opacity = 1f; // Start fully white
        fadeOverlay.pickingMode = PickingMode.Ignore;
        root.Add(fadeOverlay);
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

    private IEnumerator FadeInFromWhite()
    {
        yield return new WaitForSeconds(0.5f); // Short pause before starting

        float duration = 2.0f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            if (fadeOverlay != null)
                fadeOverlay.style.opacity = Mathf.Lerp(1f, 0f, elapsed / duration);
            yield return null;
        }

        if (fadeOverlay != null)
        {
            fadeOverlay.style.opacity = 0f;
            fadeOverlay.style.display = DisplayStyle.None; // Remove completely
        }
    }

    public void StartSceneTransition()
    {
        if (transitionSound != null)
        {
            AudioSource.PlayClipAtPoint(transitionSound, Camera.main.transform.position);
        }

        if (fadeOverlay != null)
        {
            fadeOverlay.style.display = DisplayStyle.Flex;
            fadeOverlay.pickingMode = PickingMode.Position; // Block interaction
            StartCoroutine(FadeToWhiteAndLoad());
        }
    }

    private IEnumerator FadeToWhiteAndLoad()
    {
        float duration = 2.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            if (fadeOverlay != null)
                fadeOverlay.style.opacity = Mathf.Lerp(0f, 1f, elapsed / duration);
            yield return null;
        }

        if (fadeOverlay != null) fadeOverlay.style.opacity = 1f;
        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene("Main-Menu");
    }
}
