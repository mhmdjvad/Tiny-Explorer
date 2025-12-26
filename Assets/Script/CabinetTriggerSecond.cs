using UnityEngine;

public class CabinetTriggerSecond : MonoBehaviour
{
    public enum MotionType { Rotate, Slide }
    public MotionType motionType = MotionType.Rotate;

    public GameObject doorModel;

    [Header("Rotation Settings")]
    public Vector3 openRotationAngle = new Vector3(0, 90, 0);

    [Header("Sliding Settings")]
    public Vector3 slideOffset = new Vector3(0, 0, 0.5f);

    public float openSpeed = 2f;

    private KeyInventoryUISecond uiManager;
    private bool isPlayerInside = false;
    private bool isOpening = false;
    
    private Quaternion targetRotation;
    private Quaternion initialRotation;

    private Vector3 targetPosition;
    private Vector3 initialPosition;

    public AudioClip openSound;

    private float currentT = 0f;

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
        }
    }

    void Update()
    {
        // Simple Interaction: Press E to open (No key required)
        if (isPlayerInside && !isOpening && Input.GetKeyDown(KeyCode.E))
        {
            StartOpeningSequence();
        }

        // Execute animation
        if (isOpening && doorModel != null)
        {
            currentT += Time.deltaTime * openSpeed;
            float clampedT = Mathf.Clamp01(currentT);

            if (motionType == MotionType.Rotate)
            {
                doorModel.transform.localRotation = Quaternion.Slerp(initialRotation, targetRotation, clampedT);
            }
            else // MotionType.Slide
            {
                doorModel.transform.localPosition = Vector3.Lerp(initialPosition, targetPosition, clampedT);
            }

            if (currentT >= 1f)
            {
                FinalizeOpening();
            }
        }
    }

    private void FinalizeOpening()
    {
        isOpening = false;
        
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;
        
        this.enabled = false; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
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
            isPlayerInside = false;
            if (uiManager != null)
            {
                uiManager.ShowPressE(false);
            }
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
        }
    }
}
