using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CopyInputFieldText : MonoBehaviour
{
    [SerializeField] private InputField InputFieldCopyFrom = null;
    [SerializeField] private Text TextCopyTo = null;

    private void Start()
    {
        InputFieldCopyFrom.onValueChanged.AddListener(OnInputValueChanged);
    }

    private void OnDestroy()
    {
        if (InputFieldCopyFrom != null)
        {
            InputFieldCopyFrom.onValueChanged.RemoveListener(OnInputValueChanged);
        }
    }

    private void OnInputValueChanged(string newValue)
    {
        TextCopyTo.text = newValue;
    }
}
