using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard_Block : MonoBehaviour
{

    [SerializeField] float pushForce = 7.5f;
    [SerializeField] float pushDuration = 5f;

    [SerializeField] bool isBeingPushed = false;
    [field:SerializeField] public bool isDangerous {get; set;} = false;
    [SerializeField] Vector2 pushDirection;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] SpriteRenderer sprite;

    void Start()
    {
        Destroy(this.gameObject,30);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D otherRigidbody = collision.collider.GetComponent<Rigidbody2D>();
        if (otherRigidbody != null)
        {
            pushDirection = collision.GetContact(0).normal;
            isBeingPushed = true;
            StartCoroutine(BecomeDangerous(.5f));
            StartCoroutine(StopPush(pushDuration));
        }
    }

    IEnumerator BecomeDangerous(float duration)
    {
        yield return new WaitForSeconds(duration);
        isDangerous = true;
        sprite.color = Color.red;
    }


    IEnumerator StopPush(float duration)
    {
        yield return new WaitForSeconds(duration);
        isBeingPushed = false;
        isDangerous = false;
        sprite.color = Color.green;
    }

    void FixedUpdate()
    {
        if (isBeingPushed)
        {
            rb.velocity += pushDirection * pushForce * Time.fixedDeltaTime;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

   
}
