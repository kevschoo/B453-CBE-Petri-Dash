using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HUD : MonoBehaviour
{
    PlayerScript    _player;
    ProgressBar     _healthBar;
    ProgressBar     _foodBar;


    void Awake()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        _player = playerObject.GetComponent<PlayerScript>();
    }
}
