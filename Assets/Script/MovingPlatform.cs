using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector3 _startPos ;
    public Vector3 _endPos ;
    public float speed = 2.0f;
    private bool _movingToEnd = true;

    private Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        transform.position = _startPos;
    }

    void FixedUpdate()
    {
        // تعیین مقصد فعلی
        Vector3 target = _movingToEnd ? _endPos : _startPos;

        // حرکت نرم به سمت مقصد با استفاده از MovePosition (برای جابجایی کاراکتر ضروری است)
        Vector3 newPos = Vector3.MoveTowards(_rb.position, target, speed * Time.fixedDeltaTime);
        _rb.MovePosition(newPos);

        // تغییر جهت وقتی به مقصد رسید
        if (Vector3.Distance(_rb.position, target) < 0.01f)
        {
            _movingToEnd = !_movingToEnd;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
    }
}