using UnityEngine;

public class KeyItemSecond : MonoBehaviour
{
    [Header("Visual Effects")]
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private Vector3 rotationAxis = new Vector3(0, 1, 0);
    [SerializeField] private float bobSpeed = 2f;
    [SerializeField] private float bobHeight = 0.2f;

    public AudioSource KeySound;
    
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Continuous rotation
        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);

        // Bobbing (Up and Down)
        float newY = startPos.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
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
