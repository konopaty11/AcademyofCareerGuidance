using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ClickArea : MonoBehaviour
{
    [SerializeField] ColoringMiniGame coloring;
    [SerializeField] Color color;

    InputSystemActions inputSystem;

    public static int CountAreasClicked { get; private set; }
    bool isClicked = false;
    void Awake()
    {
        inputSystem = new();
    }

    void OnEnable()
    {
        inputSystem.Player.Attack.Enable();
        inputSystem.Player.Attack.performed += Click;
    }

    void OnDisable()
    {
        inputSystem.Player.Attack.performed -= Click;
        inputSystem.Player.Attack.Disable();
    }

    /// <summary>
    /// клик по экрану
    /// </summary>
    /// <param name="context"></param>
    void Click(InputAction.CallbackContext context)
    {
        Vector2 clickPosition = GetScreenPosition();

        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(clickPosition);

        Debug.DrawRay(worldPosition, Vector2.up * 0.1f, Color.red, 200f);
        Debug.DrawRay(worldPosition, Vector2.down * 0.1f, Color.red, 200f);
        Debug.DrawRay(worldPosition, Vector2.left * 0.1f, Color.red, 200f);
        Debug.DrawRay(worldPosition, Vector2.right * 0.1f, Color.red, 200f);

        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);
        Debug.DrawRay(worldPosition, Vector3.forward * 100, Color.blue, 200f);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.name == gameObject.name)
            {
                if (!isClicked)
                {
                    isClicked = true;
                    CountAreasClicked++;
                }
                StartCoroutine(coloring.StartFill(color));
            }
        }
    }

    /// <summary>
    /// вернуть позицию экрана
    /// </summary>
    /// <returns></returns>
    Vector2 GetScreenPosition()
    {
        if (Mouse.current != null)
            return Mouse.current.position.ReadValue();

        if (Touchscreen.current != null && Touchscreen.current.touches.Count > 0)
            return Touchscreen.current.touches[0].position.ReadValue();

        return Vector2.zero;
    }
}
