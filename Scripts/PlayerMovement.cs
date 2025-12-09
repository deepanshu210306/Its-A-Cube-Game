using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class PlayerMovement : MonoBehaviour, PlayerControls.IGroundActions
{
    public Rigidbody rb;
    public float forwardForce = 3500f;
    public float sidewaysForce = 50f;

    private PlayerControls controls;
    private Vector2 moveInput = Vector2.zero;

    void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        controls = new PlayerControls();
        controls.Ground.SetCallbacks(this);
    }

    void OnEnable() => controls.Ground.Enable();
    void OnDisable() => controls.Ground.Disable();
    void OnDestroy() => controls.Dispose();

    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (ctx.control != null && ctx.control.device is Touchscreen)
        {
            if (ctx.phase == InputActionPhase.Canceled) { moveInput = Vector2.zero; return; }
            Vector2 touchPos = ctx.ReadValue<Vector2>();
            if (touchPos == Vector2.zero && Touchscreen.current != null && !Touchscreen.current.primaryTouch.press.isPressed)
            { moveInput = Vector2.zero; return; }
            moveInput = new Vector2(touchPos.x < Screen.width * 0.5f ? -1f : 1f, 0f);
            return;
        }

        if (ctx.control != null && ctx.control.device is XRController)
        {
            Vector2 stick = ctx.ReadValue<Vector2>();
            moveInput = new Vector2(stick.x, 0f);
            return;
        }

        moveInput = ctx.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        rb.AddForce(0f, 0f, forwardForce * Time.fixedDeltaTime);
        float h = moveInput.x;
        if (Mathf.Abs(h) > 0.01f)
            rb.AddForce(h * sidewaysForce * Time.fixedDeltaTime, 0f, 0f, ForceMode.VelocityChange);

        if (rb.position.y < -1f)
            FindObjectOfType<GameManager>()?.EndGame();
    }
}
