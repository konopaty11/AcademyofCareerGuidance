using UnityEngine;

public class SetColor : MonoBehaviour
{
    [SerializeField] Color color;
    [SerializeField] ColoringMiniGame miniGame;

    public void SetCurrentColor() => miniGame.SetCurrentColor(color);
}
