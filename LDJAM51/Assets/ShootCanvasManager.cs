using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootCanvasManager : MonoBehaviour
{
    public static float borderPixelSize = 0.0f;

    [SerializeField] CanvasScaler mainCanvasScaler;
    [SerializeField] RectTransform gameWindow;

    void Start()
    {
        mainCanvasScaler.referenceResolution = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
        UpdateBorder();
    }

    void UpdateBorder()
    {
        borderPixelSize = Mathf.Clamp(borderPixelSize, 0.0f, Screen.height / 2.0f);
        int actualPixelSize = (int)Mathf.Floor(borderPixelSize);
        gameWindow.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width);
        gameWindow.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height);
        gameWindow.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, actualPixelSize, gameWindow.rect.height - actualPixelSize);
        gameWindow.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, actualPixelSize, gameWindow.rect.width - actualPixelSize);
        gameWindow.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, actualPixelSize, gameWindow.rect.width - actualPixelSize);
        gameWindow.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, actualPixelSize, gameWindow.rect.height - actualPixelSize);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.PageUp))
        {
            borderPixelSize += 10.0f * Time.deltaTime;
            UpdateBorder();
        }
        else if (Input.GetKey(KeyCode.PageDown))
        {
            borderPixelSize -= 10.0f * Time.deltaTime;
            UpdateBorder();
        }
    }
}
