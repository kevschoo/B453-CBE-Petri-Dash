using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodScript : MonoBehaviour
{
    public int m_foodAmount;
    
    void Start()
    {
        m_foodAmount = Random.Range(1,10);
        transform.localScale = new Vector3( m_foodAmount, m_foodAmount);
    }
    
    public void UpdateFoodAmount()
    {
        transform.localScale = new Vector3(m_foodAmount, m_foodAmount);
    }
}
