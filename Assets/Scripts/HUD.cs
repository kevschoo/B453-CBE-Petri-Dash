using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField]
    Slider          _healthBar;
    [SerializeField]
    Slider          _foodBar;
    [SerializeField]
    RectTransform   _offspringTransform;

    PlayerScript    _player;


    void Awake()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        _player = playerObject.GetComponent<PlayerScript>();
        
        Sprite offspringSprite = playerObject.GetComponent<SpriteRenderer>().sprite;
        for (int i = 0; i < _player.ChildCount; i++)
        {
            Image child = _offspringTransform.GetChild(i).GetComponent<Image>();
            child.sprite = offspringSprite;
        }
    }


    void LateUpdate()
    {
        if (_player != null)
        {
            _healthBar.value = _player.Stats.HealthPercentage;
            _foodBar.value = _player.FoodPercentage;
        }
    }
}
