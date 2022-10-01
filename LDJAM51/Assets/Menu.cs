using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Menu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI crosshairText;
    [SerializeField] TextMeshProUGUI useGunText;
    [SerializeField] TextMeshProUGUI borderValueText;
    [SerializeField] Slider borderValueSlider;

    void Start()
    {
        UpdateTexts();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void UpdateTexts()
    {
        crosshairText.text = "Crosshair: " + (ManyMouseHandler.showCrosshair ? "ON" : "OFF");
        useGunText.text = "Use light gun: " + (ManyMouseHandler.useMouse ? "OFF" : "ON");
        borderValueText.text = ((int)ShootCanvasManager.borderPixelSize).ToString();
    }

    public void SetCrosshair()
    {
        ManyMouseHandler.showCrosshair = !ManyMouseHandler.showCrosshair;
        UpdateTexts();
    }

    public void SetLightGun()
    {
        ManyMouseHandler.useMouse = !ManyMouseHandler.useMouse;
        UpdateTexts();
    }

    public void SetBorderSize()
    {
        ShootCanvasManager.borderPixelSize = (int)borderValueSlider.value;
        UpdateTexts();
    }
}