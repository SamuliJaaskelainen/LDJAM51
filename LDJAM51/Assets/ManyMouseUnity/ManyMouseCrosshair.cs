using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ManyMouseUnity;

public class ManyMouseCrosshair : MonoBehaviour
{
    [SerializeField] float smoothing = 1.0f;
    [SerializeField] CanvasScaler crosshairCanvas;
    [SerializeField] RectTransform graphics;
    [SerializeField] float cursorSpeed = 10.0f;
    [SerializeField] UnityEngine.UI.Image selectionImage = null;

    RectTransform rectTransform { get { return transform as RectTransform; } }

    public ManyMouse Mouse { get { return mouse; } }
    ManyMouse mouse;
    Vector2 realPosition;
    bool useMouse;

    /// <summary>
    /// A more savvy way to initialize may be to use a ManyMouse reference rather than 
    /// an ID, but this is to showcase the API
    /// </summary>
    /// <param name="id"></param>
    public void Initialize(int id, bool useMouse)
    {
        this.useMouse = useMouse;

        if (!useMouse)
        {
            if (ManyMouseWrapper.MouseCount > id)
            {
                // If re-initializing, unsubscribe first
                if (mouse != null)
                {
                    //mouse.OnMouseDeltaChanged -= UpdateDelta;
                    mouse.OnMousePositionChanged -= UpdatePosition;
                }

                mouse = ManyMouseWrapper.GetMouseByID(id);
                Debug.Log(gameObject.name + " connected to mouse: " + mouse.DeviceName);

                //mouse.OnMouseDeltaChanged += UpdateDelta;
                mouse.OnMousePositionChanged += UpdatePosition;
            }
            else
            {
                Debug.Log("Mouse ID " + id + " not found. Plug in an extra mouse?");
                Destroy(gameObject);
            }
        }

        crosshairCanvas = GameObject.Find("CrosshairCanvas").GetComponent<CanvasScaler>();
    }

    private void OnEnable()
    {
        ManyMouse.OnAnyMouseUpdated += HighlightLastUpdated;
    }

    private void OnDisable()
    {
        ManyMouse.OnAnyMouseUpdated -= HighlightLastUpdated;
        if (mouse != null)
        {
            //mouse.OnMouseDeltaChanged -= UpdateDelta;
            mouse.OnMousePositionChanged -= UpdatePosition;
        }
    }

    private void UpdatePosition(Vector2 Pos)
    {
        float x = Mathf.Clamp(mouse.Position.x * Screen.currentResolution.width, 0, Screen.currentResolution.width);
        float y = Mathf.Clamp(mouse.Position.y * Screen.currentResolution.height, -Screen.currentResolution.height, 0);
        realPosition = new Vector2(x, y);
        rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, realPosition, Time.smoothDeltaTime * smoothing);
    }

    /// <summary>
    /// There's many ways you can extend this code to get the "true mouse delta" based 
    /// on current screen dimensions and window size, but for the purpose of demonstration,
    /// this is enough. Especially considering that we hide the mouse cursor and assume the
    /// game is running in full-screen
    /// </summary>
    /// <param name="Delta"></param>
    private void UpdateDelta(Vector2 Delta)
    {
        Vector2 delta = mouse.Delta * cursorSpeed * Time.deltaTime;
        rectTransform.anchoredPosition += delta;
        rectTransform.anchoredPosition = new Vector2(Mathf.Clamp(rectTransform.anchoredPosition.x, 0, Screen.currentResolution.width), Mathf.Clamp(rectTransform.anchoredPosition.y, -Screen.currentResolution.height, 0));

        //graphics.anchoredPosition = Vector2.Lerp(graphics.anchoredPosition, rectTransform.anchoredPosition, Time.smoothDeltaTime * smoothing);
    }

    private void HighlightLastUpdated(ManyMouse obj)
    {
        selectionImage.enabled = obj == mouse;
    }

    void Update()
    {
        if (useMouse)
        {
            //Debug.Log(Screen.mainWindowPosition.x + " " + Screen.mainWindowPosition.y);

            //Debug.Log("Screen current resolution width: " + Screen.currentResolution.width);
            //Debug.Log("Rendering rendering width: " + Display.main.renderingWidth);
            //Debug.Log("Mouse x: " + Input.mousePosition.x);

            rectTransform.anchoredPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y - crosshairCanvas.referenceResolution.y);
        }
    }
}