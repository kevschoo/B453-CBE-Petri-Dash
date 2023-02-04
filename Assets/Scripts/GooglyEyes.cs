using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooglyEyes : MonoBehaviour
{
    Rigidbody2D m_rigidbody2D;

    private void Start()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();

    }

    //// Update is called once per frame
    //void Update()
    //{
    //    m_rigidbody2D.velocity = -m_rigidbody2D.velocity;
    //}

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    m_rigidbody2D.velocity = -m_rigidbody2D.velocity;
    //}
}
