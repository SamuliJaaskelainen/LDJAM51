using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ManyMouseUnity;

public class ManyMouseCrosshair : MonoBehaviour
{
    public static int playerCount;

    [SerializeField] float smoothing = 1.0f;
    [SerializeField] CanvasScaler crosshairCanvas;
    [SerializeField] RectTransform graphics;
    [SerializeField] float cursorSpeed = 10.0f;
    [SerializeField] UnityEngine.UI.Image crosshairImage = null;
    [SerializeField] UnityEngine.UI.Image selectionImage = null;

    [SerializeField] GameObject defaultGun;
    GameObject gun0;
    GameObject gun1;
    GameObject gun2;
    GameObject gun3;
    int bullet = 0;
    Gun[] guns = new Gun[4];

    RectTransform rectTransform { get { return transform as RectTransform; } }

    public ManyMouse Mouse { get { return mouse; } }
    ManyMouse mouse;
    Vector2 realPosition;
    bool useMouse;
    Camera gameCamera;
    Camera crosshairCamera;
    RaycastHit hit;

    public static int player1mouseId = -1;
    public static int player2mouseId = -1;

    float timer;

    /// <summary>
    /// A more savvy way to initialize may be to use a ManyMouse reference rather than 
    /// an ID, but this is to showcase the API
    /// </summary>
    /// <param name="id"></param>
    public void Initialize(int id, bool useMouse)
    {
        this.useMouse = useMouse;

        playerCount = 1;
        player1mouseId = -1;
        player2mouseId = -1;

        if (!useMouse)
        {
            if (ManyMouseWrapper.MouseCount > id)
            {
                // If re-initializing, unsubscribe first
                if (mouse != null)
                {
                    //mouse.OnMouseDeltaChanged -= UpdateDelta;
                    mouse.OnMousePositionChanged -= UpdatePosition;
                    mouse.OnMouseButtonDown -= Shoot;
                }

                mouse = ManyMouseWrapper.GetMouseByID(id);
                Debug.Log(gameObject.name + " connected to mouse: " + mouse.DeviceName);

                //mouse.OnMouseDeltaChanged += UpdateDelta;
                mouse.OnMousePositionChanged += UpdatePosition;
                mouse.OnMouseButtonDown += Shoot;
            }
            else
            {
                Debug.Log("Mouse ID " + id + " not found. Plug in an extra mouse?");
                Destroy(gameObject);
            }
        }

        crosshairCanvas = GameObject.Find("CrosshairCanvas").GetComponent<CanvasScaler>();
        gameCamera = Camera.main;
        crosshairCamera = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();

        gun0 = Instantiate(defaultGun, transform);
        gun1 = Instantiate(defaultGun, transform);
        gun2 = Instantiate(defaultGun, transform);
        gun3 = Instantiate(defaultGun, transform);
        gun0.name = gun0.name.Replace("(Clone)", "");
        gun1.name = gun1.name.Replace("(Clone)", "");
        gun2.name = gun2.name.Replace("(Clone)", "");
        gun3.name = gun3.name.Replace("(Clone)", "");
        guns[0] = gun0.GetComponent<Gun>();
        guns[1] = gun1.GetComponent<Gun>();
        guns[2] = gun2.GetComponent<Gun>();
        guns[3] = gun3.GetComponent<Gun>();
        guns[0].ownerId = id;
        guns[1].ownerId = id;
        guns[2].ownerId = id;
        guns[3].ownerId = id;

        Debug.Log("Guns initialized");
    }

    void Shoot(int buttonId)
    {
        if (useMouse)
        {
            Debug.Log("Shoot direct mouse " + " button " + buttonId);
            player1mouseId = 0;
            crosshairImage.color = Color.cyan;
            selectionImage.color = Color.cyan;
        }
        else
        {
            Debug.Log("Shoot mouse " + mouse.ID + " button " + buttonId);

            if (player1mouseId != mouse.ID && player2mouseId != mouse.ID)
            {
                if (player1mouseId < 0)
                {
                    Debug.Log("P1 assigned to mouse " + mouse.ID);
                    player1mouseId = mouse.ID;
                    crosshairImage.color = Color.red;
                    selectionImage.color = Color.red;
                }
                else if (player2mouseId < 0)
                {
                    Debug.Log("P2 assigned to mouse " + mouse.ID);
                    player2mouseId = mouse.ID;
                    playerCount = 2;
                    ShootCanvasManager.Instance.SetPlayerTwo(true);
                    crosshairImage.color = Color.cyan;
                    selectionImage.color = Color.cyan;
                }
            }
        }

        if (bullet < guns.Length)
        {
            Debug.Log("Shoot with " + guns[bullet].name);

            if (Shop.shopOpen)
            {
                Ray ray = crosshairCamera.ScreenPointToRay(new Vector2(rectTransform.anchoredPosition.x, Screen.currentResolution.height + rectTransform.anchoredPosition.y));
                guns[bullet].Shoot(gameCamera.transform.TransformPoint(ray.origin), gameCamera.transform.TransformDirection(ray.direction));
            }
            else
            {
                Ray ray = gameCamera.ScreenPointToRay(new Vector2(rectTransform.anchoredPosition.x, Screen.currentResolution.height + rectTransform.anchoredPosition.y));
                guns[bullet].Shoot(ray.origin, ray.direction);
            }

            if (useMouse)
            {
                ShootCanvasManager.Instance.SetGunActive(1, bullet, false);
            }
            else
            {
                if (mouse.ID == player1mouseId)
                {
                    ShootCanvasManager.Instance.SetGunActive(1, bullet, false);
                }
                else if (mouse.ID == player2mouseId)
                {
                    ShootCanvasManager.Instance.SetGunActive(2, bullet, false);
                }
            }
            bullet++;
        }
        else
        {
            switch (UnityEngine.Random.Range(0, 5))
            {
                case 0:
                    AudioManager.Instance.PlaySound("impact01", gameCamera.transform.position);
                    break;
                case 1:
                    AudioManager.Instance.PlaySound("impact01", gameCamera.transform.position);
                    break;
                case 2:
                    AudioManager.Instance.PlaySound("impact01", gameCamera.transform.position);
                    break;
                case 3:
                    AudioManager.Instance.PlaySound("impact01", gameCamera.transform.position);
                    break;
                case 4:
                    AudioManager.Instance.PlaySound("impact01", gameCamera.transform.position);
                    break;
            }
        }

        if (HitPoints.hitPoints <= 0)
        {
            if (rectTransform.anchoredPosition.x > Screen.width / 2.0f)
            {
                SceneManager.LoadScene("Menu", LoadSceneMode.Single);
            }
            else
            {
                SceneManager.LoadScene("Main", LoadSceneMode.Single);
            }
        }
    }

    private void OnEnable()
    {
        ManyMouse.OnAnyMouseUpdated += HighlightLastUpdated;
        timer = 0.0f;
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
        rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, realPosition, Time.unscaledDeltaTime * smoothing);
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
        Vector2 delta = mouse.Delta * cursorSpeed * Time.unscaledDeltaTime;
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

            if (Input.GetMouseButtonDown(0))
            {
                Shoot(0);
            }
        }


        timer += Time.deltaTime;
        if (timer >= 10.0f || Shop.shopOpen)
        {
            if (!Shop.shopOpen)
            {
                HitPoints.Instance.Refresh();
                switch (UnityEngine.Random.Range(0, 5))
                {
                    case 0:
                        AudioManager.Instance.PlaySound("reload01", gameCamera.transform.position);
                        break;
                    case 1:
                        AudioManager.Instance.PlaySound("reload02", gameCamera.transform.position);
                        break;
                    case 2:
                        AudioManager.Instance.PlaySound("reload03", gameCamera.transform.position);
                        break;
                    case 3:
                        AudioManager.Instance.PlaySound("reload04", gameCamera.transform.position);
                        break;
                    case 4:
                        AudioManager.Instance.PlaySound("reload05", gameCamera.transform.position);
                        break;
                }
            }
            timer = 0.0f;
            bullet = 0;
            HitPoints.hitPoints = 3;
            ShootCanvasManager.Instance.SetHP(HitPoints.hitPoints);

            for (int i = 0; i < guns.Length; ++i)
            {
                if (useMouse)
                {
                    ShootCanvasManager.Instance.SetGunActive(1, i, true);
                }
                else
                {
                    if (mouse.ID == player1mouseId)
                    {
                        ShootCanvasManager.Instance.SetGunActive(1, i, true);
                    }
                    else if (mouse.ID == player2mouseId)
                    {
                        ShootCanvasManager.Instance.SetGunActive(2, i, true);
                    }
                }
            }
        }
        ShootCanvasManager.Instance.SetTimerArrow(timer / 10.0f);
    }

    public void ChangeGun(int gunId, GameObject newGun)
    {
        switch (gunId)
        {
            case 0:
                ReplaceGun(gunId, ref gun0, newGun);
                break;
            case 1:
                ReplaceGun(gunId, ref gun1, newGun);
                break;
            case 2:
                ReplaceGun(gunId, ref gun2, newGun);
                break;
            case 3:
                ReplaceGun(gunId, ref gun3, newGun);
                break;
        }
    }

    void ReplaceGun(int gunId, ref GameObject oldGun, GameObject newGun)
    {
        DestroyImmediate(oldGun.gameObject);
        oldGun = Instantiate(newGun, transform);
        oldGun.name = oldGun.name.Replace("(Clone)", "");
        guns[gunId] = oldGun.GetComponent<Gun>();
        Debug.Log("Replace gun at slot " + gunId + " with " + guns[gunId].name);

        if (useMouse)
        {
            guns[gunId].ownerId = 0;
        }
        else
        {
            guns[gunId].ownerId = mouse.ID;
        }

        Invoke("UpdateUIMeshes", 0.1f);
    }

    public Gun GetGun(int gunId)
    {
        return guns[gunId];
    }

    void UpdateUIMeshes()
    {
        for (int i = 0; i < guns.Length; ++i)
        {
            if (useMouse)
            {
                ShootCanvasManager.Instance.SetGun(1, i, guns[i].shotMesh);
            }
            else
            {
                if (mouse.ID == player1mouseId)
                {
                    ShootCanvasManager.Instance.SetGun(1, i, guns[i].shotMesh);
                }
                else if (mouse.ID == player2mouseId)
                {
                    ShootCanvasManager.Instance.SetGun(2, i, guns[i].shotMesh);
                }
            }
        }
    }
}