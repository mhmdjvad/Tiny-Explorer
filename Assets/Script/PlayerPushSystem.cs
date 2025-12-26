using UnityEngine;
using StarterAssets;

public class PlayerPushSystem : MonoBehaviour
{
    [Header("Settings")]
    public float pushForce = 2.0f; // قدرت هل دادن

    private ThirdPersonController _controller;
    private float _pushTimeThreshold = 0.2f;
    private float _currentPushTimer;

    void Start()
    {
        Debug.Log("[PushDebug] PlayerPushSystem Started on: " + gameObject.name);
        _controller = GetComponent<ThirdPersonController>();
        if (_controller == null) Debug.LogError("[PushDebug] ThirdPersonController NOT found on this object!");
    }

    // این متد جادویی یونیتی برای تشخیص برخورد فیزیکی است
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // Debug hit info
        if (hit.gameObject.name != "Floor") // Ignore floor to avoid spam
        {
             Debug.Log($"[PushDebug] Hit: {hit.gameObject.name} Tag: {hit.gameObject.tag} HasRB: {body != null}");
        }

        if (body == null || body.isKinematic || !hit.gameObject.CompareTag("pushable"))
        {
            return;
        }

        if (hit.moveDirection.y < -0.3)
        {
            return;
        }

        Debug.Log($"[PushDebug] Valid Push detected on: {hit.gameObject.name}");

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        body.linearVelocity = pushDir * pushForce;

        // Signal to the animator that we are pushing with timer smoothing
        if (_controller != null)
        {
            _currentPushTimer = _pushTimeThreshold;
        }
    }

    private void LateUpdate()
    {
        if (_controller != null)
        {
            if (_currentPushTimer > 0)
            {
                _controller.IsPushing = true;
                _currentPushTimer -= Time.deltaTime;
            }
            else
            {
                _controller.IsPushing = false;
            }
        }
    }
}