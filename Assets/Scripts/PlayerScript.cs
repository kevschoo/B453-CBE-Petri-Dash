using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D m_rigidbody2D;
    [SerializeField]
    private int m_speed;


    //True means independent thinking, false means flock mode
    [SerializeField]
    bool m_toggleOffspringBehaviour;
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

        if (Input.GetKeyUp(KeyCode.X))
        {
            m_toggleOffspringBehaviour = !m_toggleOffspringBehaviour;
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            // Produce off spring
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Food")
        {
            collision.gameObject.GetComponent<FoodScript>().m_foodAmount -= 1;
            collision.gameObject.GetComponent<FoodScript>().UpdateFoodAmount();

            //float speed = transform.forward.magnitude;
            //Vector3 direction = Vector3.Reflect(transform.forward.normalized, collision.GetContacts().vr)
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Food")
        {
            collision.gameObject.GetComponent<FoodScript>().m_foodAmount -= 1;
            collision.gameObject.GetComponent<FoodScript>().UpdateFoodAmount();

            float speed = transform.forward.magnitude;
            Vector3 direction = Vector3.Reflect(transform.forward.normalized, collision.contacts[0].normal);

            m_rigidbody2D.velocity = direction * speed * Time.deltaTime;
        }

    }
}
