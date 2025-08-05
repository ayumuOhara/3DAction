using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    float distance = 3.0f;
    float minVerticalAngle = 0.0f;
    float maxVerticalAngle = 50.0f;
    float turnSpeed = 3.5f; // 回転感度
    GameObject player;

    private Vector2 lookInput;
    private PlayerControls controls;
    private float hRotation = 0f;
    private float vRotation = 20f;

    void Awake()
    {
        Application.targetFrameRate = 60;

        player = GameObject.Find("Player").gameObject;

        controls = new PlayerControls();
        controls.Camera.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        controls.Camera.Look.canceled += ctx => lookInput = Vector2.zero;
    }

    void OnEnable() => controls.Camera.Enable();
    void OnDisable() => controls.Camera.Disable();

    void Update()
    {
        UpdateRotation();
        UpdateCameraPosition();
    }

    void UpdateRotation()
    {
        hRotation += lookInput.x * turnSpeed;
        vRotation -= lookInput.y * turnSpeed;
        vRotation = Mathf.Clamp(vRotation, minVerticalAngle, maxVerticalAngle);
    }

    void UpdateCameraPosition()
    {
        if (player == null) return;

        // 回転からカメラの向きを作成
        Quaternion rotation = Quaternion.Euler(vRotation, hRotation, 0);
        Vector3 targetPos = player.transform.position + new Vector3(0, 1.0f, 0);

        // プレイヤー中心から距離を取った位置にカメラを配置
        transform.position = targetPos - rotation * Vector3.forward * distance;
        transform.rotation = rotation;
    }
}
