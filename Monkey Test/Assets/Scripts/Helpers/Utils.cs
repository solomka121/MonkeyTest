using UnityEngine;

public static class Utils
{
    public static Vector2 GetWorldPositionofCanvasElement(RectTransform element)
    {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(element, element.position, Camera.main , out var result);
        return result;
    }
}
