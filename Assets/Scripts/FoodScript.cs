using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodScript : MonoBehaviour
{
    public int m_foodAmount;
    void Start()
    {
        m_foodAmount = Random.Range(1,10);
    }
    

}
