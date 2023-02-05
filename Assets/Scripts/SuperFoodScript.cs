using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SuperFoodScript : MonoBehaviour
{
    TextMeshProUGUI _canvasText;

    private void Start()
    {
        _canvasText = GetComponentInChildren<TextMeshProUGUI>();
        _canvasText.text = "?";
    }
}
