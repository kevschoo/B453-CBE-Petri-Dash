using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseOrganism : MonoBehaviour
{
    public int m_health = 0;
    public int m_speed = 0;
    public int m_attack= 0;
    public int m_food = 0;
    public Rigidbody2D m_rigidbody2D;
    //public List<x>list of trait

    Vector3 m_velocity;
    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody2D= GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
