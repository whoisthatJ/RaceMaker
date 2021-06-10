using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemVariable : Variable
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private InputField inputField;
    [SerializeField] private Button confrimBtn;

    // Use this for initialization
    private void Start()
    {

    }

    private void OnEnable()
    {
        confrimBtn.onClick.AddListener(ConfrimBtnClick);
        inputField.text = CSPlayerPrefs.GetFloat(nameVariable).ToString();
    }

    private void OnDisable()
    {
        confrimBtn.onClick.RemoveAllListeners();
    }

    private void ConfrimBtnClick()
    {
        if (!string.IsNullOrEmpty(inputField.text))
        {
            float result;
            float.TryParse(inputField.text, out result);
            CSPlayerPrefs.SetFloat(nameVariable, result);
        }
        else
        {
            Debug.LogError("InputField is Empty!");
        }
    }
}
