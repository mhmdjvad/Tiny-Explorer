using UnityEngine;

public class KeyController : MonoBehaviour
{
    public GameObject BedDoor;
    public GameObject BathDoor;
    public GameObject ExitDoor;
    private void OnTriggerEnter(Collider other)
    {
        string keyTag = gameObject.tag as string;
        Debug.Log($"Cache key {keyTag}");
        // چک می‌کنیم که آیا آبجکتی که وارد تریگر شده، پلیر است یا نه
        if (other.CompareTag("Player"))
        {
            if (keyTag == "BedRoom_key")
            {
                Debug.Log($"Open door{BedDoor}");
                BedDoor.SetActive(false);
            }
            if (keyTag == "BathRoom_key")
            {
                Debug.Log($"Open door{BathDoor}");
                BathDoor.SetActive(false);
            }
            if (keyTag == "ExitDoor_key")
            {
                Debug.Log($"Open door{ExitDoor}");
                ExitDoor.SetActive(false);
            }

            // 5. کلید را از صحنه حذف می‌کنیم
            gameObject.SetActive(false);
        }
    }
}