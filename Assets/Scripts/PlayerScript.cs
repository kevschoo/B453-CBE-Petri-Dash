using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : BaseOrganism
{
    private Rigidbody2D m_rigidbody2D;
    [SerializeField]
    private int m_speed;
    Vector2 m_velocity;

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
        m_velocity = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
        //transform.position += m_velocity * m_speed * Time.deltaTime;
        m_rigidbody2D.velocity = m_velocity * m_speed;
        if (Input.GetKeyUp(KeyCode.X))
        {
            m_toggleOffspringBehaviour = !m_toggleOffspringBehaviour;
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            // Produce off spring
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Food")
        {
            collision.gameObject.GetComponent<FoodScript>().m_foodAmount -= 1;
            collision.gameObject.GetComponent<FoodScript>().UpdateFoodAmount();

            _stats.HarvestFood(1);

            float force = 5000f;

            m_rigidbody2D.AddForce(collision.contacts[0].normal * force);
        }
    }
}
