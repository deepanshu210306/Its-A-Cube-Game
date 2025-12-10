using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class PlayerMovement : MonoBehaviour, PlayerControls.IGroundActions
{
    public Rigidbody rb;
    public float forwardForce = 3500f;
    public float sidewaysForce = 50f;

    // tweak this for touchscreen sensitivity (1 = full, 0.5 = half, etc.)
    [SerializeField] private float touchSensitivity = 0.5f;

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

    // For XR and Keyboard/Mouse (keeps original behavior)
    public void OnMove(InputAction.CallbackContext ctx)
    {
        // ignore touch here — touchscreen is handled in Update()
        if (ctx.control != null && ctx.control.device is XRController)
        {
            Vector2 stick = ctx.ReadValue<Vector2>();
            moveInput = new Vector2(stick.x, 0f);
            return;
        }

        if (ctx.control != null && !(ctx.control.device is Touchscreen))
        {
            // keyboard / gamepad / mouse etc.
            moveInput = ctx.ReadValue<Vector2>();
        }
    }

    void Update()
    {
        // ----- TOUCHSCREEN POLLING (works reliably: stops immediately on lift) -----
        if (Touchscreen.current != null)
        {
            var primary = Touchscreen.current.primaryTouch;
            // if finger is pressed, apply left/right force; otherwise stop horizontal input
            if (primary.press.isPressed)
            {
                Vector2 pos = primary.position.ReadValue();
                float dir = pos.x < Screen.width * 0.5f ? -1f : 1f;
                moveInput = new Vector2(dir * touchSensitivity, 0f);
            }
            else
            {
                // finger lifted -> stop horizontal movement immediately
                moveInput = Vector2.zero;
            }
        }
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
