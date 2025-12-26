using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    private VisualElement pauseRoot;
    private bool isPaused = false;

    void OnEnable()
    {
        // 1. دسترسی به ریشه UI
        var uiDocument = GetComponent<UIDocument>();
        pauseRoot = uiDocument.rootVisualElement.Q<VisualElement>("PauseContainer"); // نام پنل اصلی شما در UXML

        // مخفی کردن منو در شروع کار
        pauseRoot.style.display = DisplayStyle.None;

        // 2. تنظیم دکمه‌ها
        Button resumeBtn = pauseRoot.Q<Button>("Resume");
        Button quitBtn = pauseRoot.Q<Button>("MainMenu");

        resumeBtn.clicked += ResumeGame;
        quitBtn.clicked += QuitToMenu;
    }

    void Update()
    {
        // 3. چک کردن دکمه Escape برای باز و بسته کردن منو
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        pauseRoot.style.display = DisplayStyle.Flex; // نمایش منو
        Time.timeScale = 0f; // متوقف کردن زمان بازی

        // باز کردن قفل موس برای کار با منو
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseRoot.style.display = DisplayStyle.None; // مخفی کردن منو
        Time.timeScale = 1f; // بازگشت زمان به حالت عادی

        // قفل کردن دوباره موس (اگر بازی شما اول شخص یا اکشن است)
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
    }

    void QuitToMenu()
    {
        Time.timeScale = 1f; // حتماً قبل از تغییر سکانس زمان را نرمال کنید
        SceneManager.LoadScene("Main-Menu"); // نام سکانس منوی اصلی شما
    }
}