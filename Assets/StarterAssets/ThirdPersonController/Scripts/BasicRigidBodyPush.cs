using UnityEngine;

public class BasicRigidBodyPush : MonoBehaviour
{
	public LayerMask pushLayers;
	public bool canPush;
	[Range(0.5f, 10f)] public float strength = 1.1f;

	private StarterAssets.ThirdPersonController _controller;
	private float _pushTimeThreshold = 0.1f; // Time to keep animation active
	private float _currentPushTimer;

	void Start()
	{
		_controller = GetComponent<StarterAssets.ThirdPersonController>();
		Debug.Log("[PushDebug] BasicRigidBodyPush Started on: " + gameObject.name);
	}

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if (canPush) PushRigidBodies(hit);
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


    private void PushRigidBodies(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // ۱. بررسی وجود ریجیدبادی و لایه صحیح
        if (body == null || !body.isKinematic) return;

        var bodyLayerMask = 1 << body.gameObject.layer;
        if ((bodyLayerMask & pushLayers.value) == 0) return;

        if (hit.moveDirection.y < -0.3f) return;

        // ۲. محاسبه جهت و میزان حرکت در این فریم
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0.0f, hit.moveDirection.z);
        float moveDistance = strength * Time.deltaTime;

        // ۳. تست برخورد قبل از جابه‌جایی (SweepTest)
        // این خط چک می‌کند که آیا در مسیر حرکت، به چیزی (مثل دیوار) برخورد می‌کند یا نه
        if (!body.SweepTest(pushDir, out RaycastHit wallHit, moveDistance))
        {
            // فقط اگر راه باز بود، حرکت کن
            body.MovePosition(body.position + (pushDir * moveDistance));
            
            // Signal pushing with timer to avoid flickering
            _currentPushTimer = _pushTimeThreshold;
            Debug.Log("[PushDebug] Moving object: " + body.name);
        }
        else
        {
            // اگر مانعی بود، تا نزدیکی آن مانع حرکت کن اما داخلش نرو
            if (wallHit.distance > 0.01f)
            {
                body.MovePosition(body.position + (pushDir * (wallHit.distance - 0.01f)));
            }
        }
    }
    /*
    private void PushRigidBodies(ControllerColliderHit hit)
	{
		// https://docs.unity3d.com/ScriptReference/CharacterController.OnControllerColliderHit.html

		// make sure we hit a non kinematic rigidbody
		Rigidbody body = hit.collider.attachedRigidbody;
		if (body == null || body.isKinematic) return;

		// make sure we only push desired layer(s)
		var bodyLayerMask = 1 << body.gameObject.layer;
		if ((bodyLayerMask & pushLayers.value) == 0) return;

		// We dont want to push objects below us
		if (hit.moveDirection.y < -0.3f) return;

		// Calculate push direction from move direction, horizontal motion only
		Vector3 pushDir = new Vector3(hit.moveDirection.x, 0.0f, hit.moveDirection.z);

        // Apply the push and take strength into account
        body.AddForce(pushDir * strength, ForceMode.Impulse);
		// حرکت دادن مستقیم اجسام کینماتیک بدون درگیر کردن موتور فیزیک سنگین
		//body.transform.position += pushDir * strength * Time.deltaTime;
	}
	*/
}