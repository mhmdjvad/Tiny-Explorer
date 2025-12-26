using UnityEngine;

public class CabinetTriggerSecond : MonoBehaviour
{
    public enum MotionType { Rotate, Slide }
    public MotionType motionType = MotionType.Rotate;

    public GameObject doorModel;

    [Header("Rotation Settings")]
    public Vector3 openRotationAngle = new Vector3(0, 90, 0);

    [Header("Sliding Settings")]
    public Vector3 slideOffset = new Vector3(0, 0, 0.5f); // Amount to move for drawers

    public float openSpeed = 2f;

    private KeyInventoryUISecond uiManager;
    private bool isPlayerInside = false;
    private bool isOpening = false;
    
    private Quaternion targetRotation;
    private Quaternion initialRotation;

    private Vector3 targetPosition;
    private Vector3 initialPosition;

    public AudioClip openSound;

    void Start()
    {
        uiManager = FindObjectOfType<KeyInventoryUISecond>();

        if (doorModel != null)
        {
            // Initialize Rotation Data
            initialRotation = doorModel.transform.localRotation;
            targetRotation = initialRotation * Quaternion.Euler(openRotationAngle);

            // Initialize Position Data
            initialPosition = doorModel.transform.localPosition;
            targetPosition = initialPosition + slideOffset;
            
            Debug.Log($"[CabinetDebug] Initialized. MotionType: {motionType}. StartPos: {initialPosition}, TargetPos: {targetPosition}");

            // Check for Animator
            if (doorModel.GetComponent<Animator>() != null)
            {
                Debug.LogWarning($"[CabinetDebug] WARNING: {doorModel.name} has an Animator! This will stop the script from moving it. Please disable the Animator or remove it.");
            }
        }
        else
        {
            Debug.LogError("[CabinetDebug] DoorModel is NOT assigned!");
        }
    }

    void Update()
    {
        // Simple Interaction: Press E to open (No key required)
        if (isPlayerInside && !isOpening && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("[CabinetDebug] E pressed inside trigger.");
            StartOpeningSequence();
        }

        // Execute animation
        if (isOpening && doorModel != null)
        {
            if (motionType == MotionType.Rotate)
            {
                // Rotation Logic
                doorModel.transform.localRotation = Quaternion.Slerp(doorModel.transform.localRotation, targetRotation, Time.deltaTime * openSpeed);
                float angleLeft = Quaternion.Angle(doorModel.transform.localRotation, targetRotation);
                Debug.Log($"[CabinetDebug] Rotating... Angle remaining: {angleLeft}");

                if (angleLeft < 1f)
                {
                    doorModel.transform.localRotation = targetRotation;
                    FinalizeOpening();
                }
            }
            else // MotionType.Slide
            {
                // Sliding Logic
                doorModel.transform.localPosition = Vector3.Lerp(doorModel.transform.localPosition, targetPosition, Time.deltaTime * openSpeed);
                float dist = Vector3.Distance(doorModel.transform.localPosition, targetPosition);
                Debug.Log($"[CabinetDebug] Sliding... Distance remaining: {dist}");

                if (dist < 0.01f)
                {
                    doorModel.transform.localPosition = targetPosition;
                    FinalizeOpening();
                }
            }
        }
    }

    private void FinalizeOpening()
    {
        Debug.Log("[CabinetDebug] Opening Finished.");
        isOpening = false;
        
        // Safer approach: Disable only the trigger and interaction, not the entire GameObject
        // This prevents the mesh from disappearing if the script is on the door itself.
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;
        
        this.enabled = false; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("[CabinetDebug] Player Entered Trigger.");
            isPlayerInside = true;
            if (uiManager != null)
            {
                uiManager.ShowPressE(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("[CabinetDebug] Player Exited Trigger.");
            isPlayerInside = false;
            if (uiManager != null)
            {
                uiManager.ShowPressE(false);
            }
        }
    }

    private void StartOpeningSequence()
    {
        Debug.Log("[CabinetDebug] Starting Opening Sequence...");
        isOpening = true;

        if (openSound != null)
        {
            AudioSource.PlayClipAtPoint(openSound, doorModel.transform.position);
        }

        if (uiManager != null)
        {
            uiManager.ShowPressE(false);
        }
    }
}
