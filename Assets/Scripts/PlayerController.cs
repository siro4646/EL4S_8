using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class playerController : MonoBehaviour
{
    private Transform cameraTransform;
    private Animator anim;
    private CharacterController controller;

    private float verticalVelocity;

    [SerializeField] private float moveSpeed = 4.5f;
    [SerializeField] private float rotationSpeed = 12f;
    [SerializeField] private float gravity = -9.81f;

    [Header("Animator Params")]
    [SerializeField] private string speedParam = "Speed";
    [SerializeField] private float animDamp = 0.12f;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        if (anim == null)
            anim = GetComponent<Animator>();
    }

    private void Update()
    {
        // 入力
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 input = new Vector3(h, 0f, v);
        input = Vector3.ClampMagnitude(input, 1f);

        // カメラ基準ベクトル取得
        Vector3 camForward = Vector3.forward;
        Vector3 camRight = Vector3.right;

        if (cameraTransform != null)
        {
            camForward = cameraTransform.forward;
            camRight = cameraTransform.right;

            camForward.y = 0f;
            camRight.y = 0f;

            camForward.Normalize();
            camRight.Normalize();
        }

        // 移動方向計算
        Vector3 move = camRight * input.x + camForward * input.z;
        move = Vector3.ClampMagnitude(move, 1f);

        bool isMoving = move.sqrMagnitude > 0.0001f;

        // 移動
        controller.Move(move * moveSpeed * Time.deltaTime);

        // 回転
        if (isMoving)
        {
            Quaternion targetRot = Quaternion.LookRotation(move, Vector3.up);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                rotationSpeed * Time.deltaTime);
        }

        // 重力
        if (controller.isGrounded && verticalVelocity < 0f)
            verticalVelocity = -2f;

        verticalVelocity += gravity * Time.deltaTime;
        controller.Move(Vector3.up * verticalVelocity * Time.deltaTime);

        //アニメーション
        if (anim != null)
        {
            anim.SetFloat(speedParam, input.magnitude, animDamp, Time.deltaTime);
        }
    }
}
