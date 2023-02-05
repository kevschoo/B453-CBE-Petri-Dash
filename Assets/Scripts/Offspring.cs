using System.Collections.Generic;
using System.Collections;
using UnityEngine;
//
using EnumHolder;

public class Offspring : BaseOrganism
{
    enum State
    {
        Search,
        Seek,
        Harvest,
        Fight,
        Flock,
    }


    [SerializeField]
    List<Offspring>         _siblings;
    [SerializeField]
    BaseOrganism            _parent;

    State                   _state;

    bool                    _wondering;
    bool                    _shouldDeposit;



    new void Awake()
    {
        base.Awake();

        _state = State.Search;
        _target = null;
        _wondering = false;
        _shouldDeposit = false;
    }



    void Update()
    {
        if (_stats.IsAlive == false) { return; }

        switch (_state)
        {
            case State.Search:
                SearchForTarget();
                break;

            case State.Seek:
                SeekToTarget();
                break;

            case State.Harvest:
                HarvestTarget();
                break;

            case State.Fight:
                FightTarget();
                break;

            default:
                break;
        }

        AssessPriorities();
    }


    private void SearchForTarget()
    {
        if (_wondering) { return; }

        if (_target == null)
        {
            StartCoroutine(Wondering());
        }
        else
            _state = State.Seek;
    }

    private IEnumerator Wondering()
    {
        _wondering = true;
        float timer = 0.0f;
        float duration = 3.0f;

        // move in a sensible direction
        Vector3 randomPosition = new Vector3(Random.Range(-50.0f, 50.0f),
                                             Random.Range(-50.0f, 50.0f),
                                             0.0f);
        Vector2 direction = (randomPosition - transform.position).normalized;

        _rigidbody2D.velocity = direction * _stats.Speed;

        while (timer < duration && _target == null && 
               _state != State.Flock)
        {
            yield return new WaitForEndOfFrame();

            timer += Time.deltaTime;
        }

        _rigidbody2D.velocity = Vector2.zero;
        _wondering = false;
    }


    private void SeekToTarget()
    {
        if (_target == null)
        {
            _state = State.Search;
            return;
        }

        Vector2 direction = (_target.position - transform.position).normalized;
        _rigidbody2D.velocity = direction * _stats.Speed;

        if (Vector2.Distance(transform.position, _target.position) < 0.1f)
        {
            _rigidbody2D.velocity = Vector2.zero;

            if (_target.CompareTag("Food"))
                _state = State.Harvest;
            else
                _state = State.Fight;
        }
    }


    private void HarvestTarget()
    {
        if (_target == null)
            _state = State.Search;
        else if (Vector2.Distance(transform.position, _target.position) > 0.1f)
        {
            _state = State.Seek;
        }
    }


    private void FightTarget()
    {
        if (_target == null)
            _state = State.Search;
        else if (Vector2.Distance(transform.position, _target.position) > 0.1f)
        {
            _state = State.Seek;
        }
    }


    private void AssessPriorities()
    {
        if (_shouldDeposit) { return; }

        Collider2D[] colliders = CheckForCollision(transform.position);
        RespondToCollision(colliders);
    }

    private Collider2D[] CheckForCollision(Vector3 position)
    {
        return Physics2D.OverlapCircleAll(position, _sightRadius);
    }


    private void RespondToCollision(Collider2D[] colliders)
    {
        // set up possible targets
        Transform closestSuperfood = null;
        Transform closestFood = null;
        Transform closestOffspring = null;
        Transform closestSingCellOrganism = null;

        // update closest objects based on the current target.
        if (_target != null)
        {
            if (_target.CompareTag("Superfood"))
                closestSuperfood = _target;
            else if (_target.CompareTag("Food"))
                closestFood = _target;
            else if (_target.CompareTag("Offspring"))
                closestOffspring = _target;
            else if (_target.CompareTag("SingleCelledOrganism"))
                closestSingCellOrganism = _target;
        }

        // go through all objects in range and find the closest ones
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Offspring") && _stats.CanAttack)
            {
                Offspring offspring = collider.GetComponent<Offspring>();
                if (offspring.GetInstanceID() == GetInstanceID() ||
                    _siblings.Contains(offspring) ||
                    offspring.CanAttack ||
                    !offspring.Stats.IsAlive)
                    continue;

                if (closestOffspring != null)
                {
                    float distanceA = Mathf.Abs(Vector2.Distance(transform.position, closestOffspring.position));
                    float distanceB = Mathf.Abs(Vector2.Distance(transform.position, collider.transform.position));

                    if (distanceB < distanceA)
                        closestOffspring = collider.transform;
                }
                else
                    closestOffspring = collider.transform;
            }
            else if (collider.CompareTag("SingleCelledOrganism") && _stats.CanAttack)
            {
                SingleCelledOrganism organism = collider.GetComponent<SingleCelledOrganism>();
                if (organism.GetInstanceID() == _parent.GetInstanceID() ||
                    organism.Stats.CanAttack ||
                    !organism.Stats.IsAlive)
                    continue;

                if (closestSingCellOrganism != null)
                {
                    float distanceA = Mathf.Abs(Vector2.Distance(transform.position, closestSingCellOrganism.position));
                    float distanceB = Mathf.Abs(Vector2.Distance(transform.position, collider.transform.position));

                    if (distanceB < distanceA)
                        closestSingCellOrganism = collider.transform;
                }
                else
                    closestSingCellOrganism = collider.transform;
            }
            else if (collider.CompareTag("Food"))
            {
                if (closestFood != null)
                {
                    float distanceA = Mathf.Abs(Vector2.Distance(transform.position, closestFood.position));
                    float distanceB = Mathf.Abs(Vector2.Distance(transform.position, collider.transform.position));

                    if (distanceB < distanceA)
                        closestFood = collider.transform;
                }
                else
                    closestFood = collider.transform;
            }
        }

        // update target
        if (closestSuperfood != null)
            _target = closestSuperfood;
        else if (closestOffspring != null)
            _target = closestOffspring;
        else if (closestSingCellOrganism != null)
            _target = closestSingCellOrganism;
        else
            _target = closestFood;
    }



    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_stats.IsAlive) { return; }

        if (collision.CompareTag("SingleCelledOrganism"))
        {
            SingleCelledOrganism organism = collision.GetComponent<SingleCelledOrganism>();
            if (_parent == organism)
            {
                print("Parent was fed by a child.");
                _parent.Stats.CollectFood(_stats.Food);
                _stats.Food = 0;
            }
            else
            {
                if (organism.CanAttack)
                {
                    print("Offspring was attacked by a Single Cell.");
                    _stats.TakeDamage(organism.Stats.Damage);
                }

                if (CanAttack)
                {
                    print("Offspring attacked a Single cell.");
                    organism.Stats.TakeDamage(_stats.Damage);
                    if (!organism.Stats.IsAlive)
                        StartSearching();
                }
            }


        }
        else if (collision.CompareTag("Offspring"))
        {
            Offspring offspring = collision.GetComponent<Offspring>();
            if (_siblings.Contains(offspring))
                return;

            if (offspring.CanAttack)
            {
                print("Offspring was attacked by an Offspring.");
                _stats.TakeDamage(offspring.Stats.Damage);
            }

            if (CanAttack)
            {
                print("Offspring attacked an Offspring.");
                offspring.Stats.TakeDamage(_stats.Damage);
                if (!offspring.Stats.IsAlive)
                    StartSearching();

            }
        }
        else if (collision.CompareTag("Food"))
        {
            _stats.HarvestFood(collision.GetComponent<FoodScript>().GetFood(false));
            _rigidbody2D.velocity = Vector2.zero;
            _rigidbody2D.AddForce(Utility.BounceBack(transform.position, collision.transform.position));
        }
    }


    private void StartSearching()
    {
        _rigidbody2D.velocity = Vector2.zero;
        _state = State.Search;
        _target = null;
    }


    private void CheckFoodDeposit()
    {
        float foodFill = _stats.Food / (float)_stats.MaxFood;
        if (foodFill > 0.70f)
        {
            _shouldDeposit = true;
            _target = _parent.transform;
        }
    }



    public void AssignParent(BaseOrganism parent, Sprite sprite)
    {
        _parent = parent;

        _spriteRenderer.sprite = sprite;
    }


    public void AssignSibling(Offspring sibling)
    {
        _siblings.Add(sibling);
    }


    public void Flock()
    {
        _state = State.Flock;
    }


    public void DoAsYouWill()
    {
        _state = State.Search;
    }
}
