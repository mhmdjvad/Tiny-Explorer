using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Main_menu : MonoBehaviour
{
    public AudioClip hoverSound; // فایل صدا را در اینسپکتور اینجا قرار دهید
    public string mainSceneName = "MainScene";

    private AudioSource audioSource;
    private VisualElement root;

    void OnEnable()
    {
        // Unlock and show cursor when menu is enabled
        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;

        // 1. گرفتن ریشه UI
        var uiDocument = GetComponent<UIDocument>();
        if (uiDocument == null) return;
        root = uiDocument.rootVisualElement;

        // 2. آماده‌سازی سیستم پخش صدا
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        // 3. پیدا کردن تمام دکمه‌ها و ثبت رویدادها
        List<Button> allButtons = root.Query<Button>().ToList();

        foreach (var btn in allButtons)
        {
            // وقتی موس وارد محدوده دکمه می‌شود
            btn.RegisterCallback<MouseEnterEvent>(evt => OnButtonHover(btn));

            // وقتی موس از محدوده دکمه خارج می‌شود
            btn.RegisterCallback<MouseLeaveEvent>(evt => OnButtonLeave(btn));

            btn.RegisterCallback<ClickEvent>(evt => OnButtonClick(btn));
        }
    }

    private void OnButtonHover(Button btn)
    {
        // پخش صدا
        if (hoverSound != null)
        {
            audioSource.PlayOneShot(hoverSound);
        }

        // غیب کردن دکمه (شفافیت صفر)
        btn.style.opacity = 0;
    }

    private void OnButtonLeave(Button btn)
    {
        // ظاهر کردن مجدد دکمه (شفافیت یک)
        btn.style.opacity = 1;
    }

    private void OnButtonClick(Button btn)
    {
        // چک کردن نام دکمه (دقت کنید که در UI Builder نام دکمه را Start و Quit گذاشته باشید)
        if (btn.name == "Start")
        {
            // مخفی کردن و قفل کردن موس
            UnityEngine.Cursor.visible = false;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;

            SceneManager.LoadScene("MainScene");
        }
        else if (btn.name == "Quit")
        {
            Debug.Log("Quitting Application...");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // برای تست در ادیتور
#else
                Application.Quit(); // برای نسخه نهایی (Build)
#endif
        }
    }
}
