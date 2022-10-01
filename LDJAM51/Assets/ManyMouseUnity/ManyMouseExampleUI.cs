using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ManyMouseUnity;
using System;

namespace ManyMouseExample
{
    public class ManyMouseExampleUI : MonoBehaviour
    {
        [SerializeField] ManyMouseHandler exampleScript;
        [SerializeField] GameObject debugUi;
        [SerializeField] Text idField;
        [SerializeField] Text nameField;
        [SerializeField] Text xField;
        [SerializeField] Text yField;

        private void OnEnable()
        {
            ManyMouse.OnAnyMouseUpdated += UpdateUI;
        }

        private void OnDisable()
        {
            ManyMouse.OnAnyMouseUpdated -= UpdateUI;
        }

        private void UpdateUI(ManyMouse m)
        {
            if (m.ID < exampleScript.Crosshairs.Count)
            {
                idField.text = "ID:" + m.ID.ToString();
                nameField.text = "HID Name: " + m.DeviceName;
                var c = exampleScript.Crosshairs[m.ID].transform as RectTransform;
                xField.text = "X Pos: " + c.anchoredPosition.x.ToString();
                yField.text = "Y Pos: " + c.anchoredPosition.y.ToString();
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                debugUi.SetActive(!debugUi.activeSelf);
            }
        }
    }
}