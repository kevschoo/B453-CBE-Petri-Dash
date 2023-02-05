using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// enums
using EnumHolder;
using TMPro;
using UnityEngine.UI;

public class PlayerScript : BaseOrganism
{

    Vector2 m_velocity;

    [SerializeField]
    List<Offspring> _children;

    //True means independent thinking, false means flock mode
    [SerializeField]
    bool m_toggleOffspringBehaviour;

    [SerializeField]
    int _foodRequired;

    [SerializeField]
    GameObject _offspringPrefab;

    [SerializeField]
    bool m_controlsEnabled = true;


    public Canvas thing;

    const float FOOD_DECAY_TIME = 1.0f;
    const int FOOD_DECAY_AMOUNT = 1;



    new void Awake()
    {
        base.Awake();

        StartCoroutine(DecayFood());
    }


    private IEnumerator DecayFood()
    {
        while (_stats.IsAlive)
        {
            yield return new WaitForSeconds(FOOD_DECAY_TIME);

            _stats.Food -= FOOD_DECAY_AMOUNT;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_controlsEnabled)
        {
            m_velocity = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
            _rigidbody2D.velocity = m_velocity * _stats.Speed;
        }

        //transform.position += m_velocity * m_speed * Time.deltaTime;

        if (Input.GetKeyUp(KeyCode.X))
        {
            m_toggleOffspringBehaviour = !m_toggleOffspringBehaviour;
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            ProduceOffspring();
        }
    }


    IEnumerator PotatoScientistImage()
    {
       thing.GetComponentInChildren<Image>().CrossFadeAlpha(1,5,false);
       yield return null;
        //yield return new WaitForSeconds(5f);
    }


    private void ProduceOffspring()
    {
        if (_stats.Food >= _foodRequired)
        {
            _stats.Food -= (int)(_foodRequired * 0.75f);
            ScaleWithFood();

            // instantiate the offspring
            GameObject clone = Instantiate(_offspringPrefab,
                                            transform.position,
                                            transform.rotation);
            Offspring offspring = clone.GetComponent<Offspring>();
            offspring.AssignParent(this, _spriteRenderer.sprite,
                                         _spriteRenderer.color);

            foreach (Offspring child in _children)
            {
                child.AssignSibling(offspring);
            }

            _children.Add(offspring);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {

        
        if (collision.CompareTag("Superfood"))
        {
            Trait trait = Utility.PickTrait(_stats.Luck);
            _traits.Add(trait);

            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Food"))
        {
            _stats.HarvestFood(collision.gameObject.GetComponent<FoodScript>().GetFood(false));
            ScaleWithFood();
            _rigidbody2D.velocity = Vector2.zero;
            _rigidbody2D.AddForce(Utility.BounceBack(transform.position, collision.transform.position));
            StartCoroutine(HaltControls());
            thing.GetComponentInChildren<Image>().CrossFadeAlpha(1, 5, false);
            //StartCoroutine(PotatoScientistImage());
        }
        else if (collision.CompareTag("SingleCelledOrganism"))
        {
            SingleCelledOrganism organism = collision.GetComponent<SingleCelledOrganism>();
            if (organism.CanAttack)
                _stats.TakeDamage(organism.Stats.Damage);

            if (CanAttack)
            {
                organism.Stats.TakeDamage(_stats.Damage);
            }

            _rigidbody2D.velocity = Vector2.zero;
            _rigidbody2D.AddForce(Utility.BounceBack(transform.position, collision.transform.position));
            StartCoroutine(HaltControls());
        }
        else if (collision.CompareTag("Offspring"))
        {
            Offspring offspring = collision.GetComponent<Offspring>();
            if (_children.Contains(offspring))
                return;

            if (offspring.CanAttack)
                _stats.TakeDamage(offspring.Stats.Damage);

            if (CanAttack)
            {
                offspring.Stats.TakeDamage(_stats.Damage);

            }
            _rigidbody2D.velocity = Vector2.zero;
            _rigidbody2D.AddForce(Utility.BounceBack(transform.position, collision.transform.position));
            StartCoroutine(HaltControls());
        }
    }

    IEnumerator HaltControls()
    {
        m_controlsEnabled = false;
        yield return new WaitForSeconds(0.1f);
        m_controlsEnabled = true;
    }

}
