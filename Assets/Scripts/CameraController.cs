using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float followSpeed = 12f;
    [SerializeField] float yawSpeed = 240f;
    [SerializeField] KeyCode rotateLeft = KeyCode.J;
    [SerializeField] KeyCode rotateRight = KeyCode.L;

    private void Start()
    {
        if (!target)
        {
            target = GameObject.FindGameObjectWithTag("Player")?.transform;
            if (!target)
                Debug.LogError("PlayerにPlayerTagつけて");
        }
    }

    void LateUpdate()
    {
        if (!target) return;

        // プレイヤーに追従
        Vector3 desiredPos = target.position;
        transform.position = Vector3.Lerp(transform.position, desiredPos, followSpeed * Time.deltaTime);

        // Y回転
        float input = 0f;
        if (Input.GetKey(rotateLeft)) input -= 1f;
        if (Input.GetKey(rotateRight)) input += 1f;

        float yaw = input * yawSpeed * Time.deltaTime;
        transform.Rotate(0f, yaw, 0f, Space.World);
    }

    public void ResetTarget()
    {
        target = GameObject.FindGameObjectWithTag("Player")?.transform;
    }
}
