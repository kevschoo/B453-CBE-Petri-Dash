using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard_Damage : MonoBehaviour
{
    [SerializeField] float _speed = 2f;
    [SerializeField] float _rotationSpeed = 100f;
    [SerializeField] float _maxWanderDistance = 100f;
    [SerializeField] Vector2 _wanderPoint = Vector2.zero;
    [SerializeField] GameObject SpikeBody;
    [SerializeField] GameObject SpikeBodyTwo;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject,30);
        SetNewWanderTarget();
        _speed = Random.Range(5,25);
        _maxWanderDistance = Random.Range(30,100);
        _rotationSpeed = Random.Range(60,180);
    }

    // Update is called once per frame
    void Update()
    {
        SpikeBody.transform.Rotate(0f, 0f, _rotationSpeed * Time.deltaTime);
        SpikeBodyTwo.transform.Rotate(0f, 0f, -_rotationSpeed * Time.deltaTime * 0.5f);

        
        Vector2 direction = _wanderPoint - (Vector2)transform.position;
        float distance = direction.magnitude;
        if (distance > .05f)
        {
            Vector2 velocity = direction.normalized * _speed;
            transform.Translate(velocity * Time.deltaTime, Space.World);
        }
        else
        {
            SetNewWanderTarget();
        }
    }

    void SetNewWanderTarget()
    {
        Vector2 randomPoint = Random.insideUnitCircle * _maxWanderDistance;
        randomPoint += (Vector2)transform.position;
        _wanderPoint = randomPoint;
    }
}
