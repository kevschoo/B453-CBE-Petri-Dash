using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D m_rigidbody2D;
    [SerializeField]
    private int m_speed;
    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody2D= GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vector3 = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
        transform.position += vector3 * m_speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
