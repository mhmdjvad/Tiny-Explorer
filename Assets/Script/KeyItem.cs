using UnityEngine;

public class KeyItem : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 100f; // سرعت چرخش
    public AudioSource KeySound;
    void Update()
    {
        // خط کد برای چرخش همیشگی کلید به دور خودش (محور Y)
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // چک می‌کنیم که آیا پلیر با کلید برخورد کرده یا نه
        if (other.CompareTag("Player"))
        {
            // پیدا کردن مدیریت کننده UI در صحنه
            KeyInventoryUI uiManager = FindObjectOfType<KeyInventoryUI>();

            if (uiManager != null)
            {
                // فرستادن تگ همین کلید به اسکریپت UI
                uiManager.ShowKeyOnUI(gameObject.tag);
            }

            // play sound track
            //KeySound = GetComponent<AudioSource>();
            KeySound.Play();

            // حذف کلید از صحنه
            Destroy(gameObject);
        }
    }
}