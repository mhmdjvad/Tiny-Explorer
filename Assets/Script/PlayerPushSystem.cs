using UnityEngine;

public class PlayerPushSystem : MonoBehaviour
{
    [Header("Settings")]
    public float pushForce = 2.0f; // قدرت هل دادن

    // این متد جادویی یونیتی برای تشخیص برخورد فیزیکی است
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // ۱. بررسی می‌کنیم که آیا جسم برخورد شده فیزیک دارد؟
        // ۲. بررسی می‌کنیم که آیا تگ pushable دارد؟
        if (body == null || body.isKinematic || !hit.gameObject.CompareTag("pushable"))
        {
            return;
        }

        // ۳. جلوگیری از هل دادن به سمت پایین (فقط افقی)
        if (hit.moveDirection.y < -0.3)
        {
            return;
        }

        // ۴. محاسبه جهت نیرو (بر اساس سمتی که پلیر حرکت می‌کند)
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        // ۵. وارد کردن نیرو به جسم
        body.linearVelocity = pushDir * pushForce;
    }
}