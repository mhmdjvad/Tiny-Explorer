using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public enum KeyRequired { Bedroom, Bathroom, Exit }
    public KeyRequired doorType;

    public GameObject doorModel;
    public Vector3 openOffset = new Vector3(0, 0, 0f);
    public float openSpeed = 1f;

    private KeyInventoryUI uiManager;
    private bool isPlayerInside = false;
    private bool isOpening = false;
    private Vector3 targetPosition;

    public AudioClip openSound;

    void Start()
    {
        uiManager = FindObjectOfType<KeyInventoryUI>();

        if (doorModel != null)
        {
            // موقعیت نهایی در بر اساس مختصات محلی (Local)
            targetPosition = doorModel.transform.localPosition + openOffset;
        }
    }

    void Update()
    {
        // چک کردن فشردن کلید E فقط وقتی پلیر داخل محدوده است و در هنوز باز نشده
        if (isPlayerInside && !isOpening && Input.GetKeyDown(KeyCode.E))
        {
            if (HasRequiredKey())
            {
                StartOpeningSequence();
            }
        }

        // اجرای انیمیشن حرکت در
        if (isOpening && doorModel != null)
        {
            doorModel.transform.localPosition = Vector3.Lerp(doorModel.transform.localPosition, targetPosition, Time.deltaTime * openSpeed);

            // وقتی در به نزدیکی مقصد رسید، حرکت را متوقف کن
            if (Vector3.Distance(doorModel.transform.localPosition, targetPosition) < 0.01f)
            {
                doorModel.transform.localPosition = targetPosition;
                isOpening = false;
                // غیرفعال کردن کل آبجکت تریگر برای بهینه‌سازی
                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            CheckKey(); // نمایش پیام‌های مناسب هنگام ورود
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            uiManager.ShowPressE(false);
            uiManager.ShowAlert(false);
        }
    }

    // این متد فقط برای چک کردن منطقی داشتن کلید است
    private bool HasRequiredKey()
    {
        switch (doorType)
        {
            case KeyRequired.Bedroom: return uiManager.hasBedKey;
            case KeyRequired.Bathroom: return uiManager.hasBathKey;
            case KeyRequired.Exit: return uiManager.hasExitKey;
            default: return false;
        }
    }

    private void CheckKey()
    {
        bool hasKey = HasRequiredKey();
        string doorName = "";

        switch (doorType)
        {
            case KeyRequired.Bedroom: doorName = "Bed Room"; break;
            case KeyRequired.Bathroom: doorName = "Bath Room"; break;
            case KeyRequired.Exit: doorName = "Exit"; break;
        }

        if (hasKey)
        {
            uiManager.ShowPressE(true);
            uiManager.ShowAlert(false);
        }
        else
        {
            uiManager.ShowAlert(true, "You dont have " + doorName + " key");
            uiManager.ShowPressE(false);
        }
    }

    private void StartOpeningSequence()
    {
        isOpening = true;

        if (openSound != null)
        {
            // پخش صدا در محل در
            AudioSource.PlayClipAtPoint(openSound, doorModel.transform.position);
        }

        // low capacity key
        if (uiManager != null)
        {
            uiManager.ShowPressE(false);
            uiManager.ShowAlert(false);

            // Fade key icon on HUD
            string keyTag = "";
            if (doorType == KeyRequired.Bedroom) keyTag = "BedRoom_key";
            else if (doorType == KeyRequired.Bathroom) keyTag = "BathRoom_key";
            else if (doorType == KeyRequired.Exit) keyTag = "ExitDoor_key";

            uiManager.UseKey(keyTag);
        }
        
        // مخفی کردن پیام‌ها بلافاصله بعد از زدن دکمه
        uiManager.ShowPressE(false);
        uiManager.ShowAlert(false);

        // غیرفعال کردن کولایدر تریگر تا دیگر رویدادهای Enter/Exit اجرا نشوند
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        // Trigger Scene Transition if this is the Exit door
        if (doorType == KeyRequired.Exit && uiManager != null)
        {
            uiManager.StartSceneTransition();
        }

        Debug.Log("Door is opening...");
    }
}