using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ClickArea : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Image image;
    [SerializeField] Color targetColor;
    [SerializeField] ColoringMiniGame miniGame;

    public bool IsColored { get; private set; } = false;

    private void Start()
    {
        image.alphaHitTestMinimumThreshold = 0.1f;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (miniGame.CurrentColor != targetColor) return;

        IsColored = true;
        image.color = targetColor;
        miniGame.CheckAreas();
    }
}
