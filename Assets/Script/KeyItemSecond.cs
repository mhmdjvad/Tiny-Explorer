using UnityEngine;

public class KeyItemSecond : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 100f; 
    public AudioSource KeySound;
    
    void Update()
    {
        // Continuous rotation
        transform.Rotate(new Vector3(0, 0, 90) * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Find KeyInventoryUISecond specifically
            KeyInventoryUISecond uiManager = FindObjectOfType<KeyInventoryUISecond>();

            if (uiManager != null)
            {
                uiManager.ShowKeyOnUI(gameObject.tag);
            }
            else
            {
                Debug.LogWarning("KeyInventoryUISecond not found in the scene!");
            }

            if(KeySound != null)
            {
                KeySound.Play();
            }

            // Destroy key object
            Destroy(gameObject);
        }
    }
}
