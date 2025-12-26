using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    // انتخاب کن که این در به کدام کلید نیاز دارد
    public enum KeyRequired { Bedroom, Bathroom, Exit }
    public KeyRequired doorType;

    private KeyInventoryUI uiManager;

    void Start()
    {
        // پیدا کردن مدیریت کننده UI در صحنه
        uiManager = FindObjectOfType<KeyInventoryUI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CheckKey();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // وقتی بازیکن از در دور شد، تمام پیام‌ها غیب شوند
            uiManager.ShowPressE(false);
            uiManager.ShowAlert(false);
        }
    }

    private void CheckKey()
    {
        bool hasKey = false;
        string doorName = "";

        // بررسی اینکه آیا بازیکن کلید مربوطه را دارد یا نه
        switch (doorType)
        {
            case KeyRequired.Bedroom:
                hasKey = uiManager.hasBedKey;
                doorName = "Bed Room";
                break;
            case KeyRequired.Bathroom:
                hasKey = uiManager.hasBathKey;
                doorName = "Bath Room";
                break;
            case KeyRequired.Exit:
                hasKey = uiManager.hasExitKey;
                doorName = "Exit";
                break;
        }

        if (hasKey)
        {
            uiManager.ShowPressE(true); // نمایش Press E
            uiManager.ShowAlert(false);
        }
        else
        {
            uiManager.ShowAlert(true, "You dont have " + doorName + " key"); // نمایش Alert
            uiManager.ShowPressE(false);
        }
    }
}