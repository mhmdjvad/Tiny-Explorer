using UnityEngine;
using UnityEngine.SceneManagement; // برای جابه‌جایی بین سکانس‌ها ضروری است

public class LevelExit : MonoBehaviour
{
    [Header("Settings")]
    public string nextSceneName; // نام دقیق سکانس بعدی را اینجا بنویس
    public float delayBeforeLoad = 0.5f; // کمی تاخیر برای حس بهتر

    private void OnTriggerEnter(Collider other)
    {
        // چک کن که آیا وارد شونده پلیر است؟
        if (other.CompareTag("Player"))
        {
            // شروع عملیات رفتن به مرحله بعد
            Invoke("LoadNextLevel", delayBeforeLoad);
        }
    }

    void LoadNextLevel()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("نام مرحله بعدی را در اینسپکتور وارد نکرده‌ای!");
        }
    }
}