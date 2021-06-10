using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemVector : Variable
{

    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private InputField inputFieldX;
    [SerializeField] private InputField inputFieldY;
    [SerializeField] private InputField inputFieldZ;
    [SerializeField] private Button confrimBtn;

    private void Start()
    {

    }

    private void OnEnable()
    {
        confrimBtn.onClick.AddListener(ConfrimBtnClick);
        inputFieldX.text = CSPlayerPrefs.GetVector3(nameVariable).x.ToString();
        inputFieldY.text = CSPlayerPrefs.GetVector3(nameVariable).y.ToString();
        inputFieldZ.text = CSPlayerPrefs.GetVector3(nameVariable).z.ToString();
    }

    private void OnDisable()
    {
        confrimBtn.onClick.RemoveAllListeners();
    }

    private void ConfrimBtnClick()
    {
        if (!string.IsNullOrEmpty(inputFieldX.text) && !string.IsNullOrEmpty(inputFieldY.text) && !string.IsNullOrEmpty(inputFieldZ.text))
        {
            float resultX;
            float.TryParse(inputFieldX.text, out resultX);

            float resultY;
            float.TryParse(inputFieldY.text, out resultY);

            float resultZ;
            float.TryParse(inputFieldZ.text, out resultZ);
            Vector3 vec = new Vector3(resultX, resultY, resultZ);
            CSPlayerPrefs.SetVector3(nameVariable, vec);
        }
        else
        {
            Debug.LogError("InputField is Empty!");
        }
    }
}
