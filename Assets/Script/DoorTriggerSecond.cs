using UnityEngine;

public class DoorTriggerSecond : MonoBehaviour
{
    public enum KeyRequired { Bedroom, Bathroom, Exit }
    public KeyRequired doorType;

    public GameObject doorModel;
    public GameObject doorModel2;
    
    [Header("Rotation Settings")]
    public Vector3 openRotationAngle = new Vector3(0, 90, 0); 
    public float openSpeed = 2f;

    private KeyInventoryUISecond uiManager;
    private bool isPlayerInside = false;
    private bool isOpening = false;
    private Quaternion targetRotation;
    private Quaternion initialRotation;

    public AudioClip openSound;

    void Start()
    {
        uiManager = FindObjectOfType<KeyInventoryUISecond>();

        if (doorModel != null)
        {
            initialRotation = doorModel.transform.localRotation;
            targetRotation = initialRotation * Quaternion.Euler(openRotationAngle);
        }
    }

    void Update()
    {
        // Check for E press only if inside trigger and not already opening
        if (isPlayerInside && !isOpening && Input.GetKeyDown(KeyCode.E))
        {
            if (HasRequiredKey())
            {
                StartOpeningSequence();
            }
        }

        // Execute door rotation
        if (isOpening && doorModel != null)
        {
            doorModel.transform.localRotation = Quaternion.Slerp(doorModel.transform.localRotation, targetRotation, Time.deltaTime * openSpeed);

            if (Quaternion.Angle(doorModel.transform.localRotation, targetRotation) < 1f)
            {
                doorModel.transform.localRotation = targetRotation;
                isOpening = false;
                gameObject.SetActive(false); // Disable trigger
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            CheckKey(); 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            if (uiManager != null)
            {
                uiManager.ShowPressE(false);
                uiManager.ShowAlert(false);
            }
        }
    }

    private bool HasRequiredKey()
    {
        if (uiManager == null) return false;

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
        if (uiManager == null) return;

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
            AudioSource.PlayClipAtPoint(openSound, doorModel.transform.position);
        }

        if (uiManager != null)
        {
            uiManager.ShowPressE(false);
            uiManager.ShowAlert(false);
        }

        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        Debug.Log("Door is opening (Second Scene Rotation)...");
    }
}
