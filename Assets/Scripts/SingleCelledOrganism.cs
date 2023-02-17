using System.Collections.Generic;
using System.Collections;
using UnityEngine;
// enums
using EnumHolder;

public class SingleCelledOrganism : BaseOrganism
{
    enum State
    {
        Search,
        Seek,
        Harvest,
        Fight,
    }


    [SerializeField]
    List<Offspring>     _children;
    [SerializeField]
    GameObject          _offspringPrefab;
    [SerializeField]
    int                 _foodRequired;
    [SerializeField]
    Sprite[]            _sprites;

    State               _state;

    bool                _wondering;

    static int          _spriteIndex = 0;
    

    const float         FOOD_DECAY_TIME = 3.0f;
    const int           FOOD_DECAY_AMOUNT = 1;




    new void Awake()
    {
        base.Awake();

        _stats.Food = _foodRequired - 1;

        _state = State.Search;
        _target = null;
        _wondering = false;

        _spriteRenderer.sprite = _sprites[_spriteIndex % 5];
        _spriteIndex++;

        switch (_spriteIndex % 7)
        {
            case 0:
                _spriteRenderer.color = Color.blue;
                break;
            case 1:
                _spriteRenderer.color = Color.cyan;
                break;
            case 2:
                _spriteRenderer.color = Color.green;
                break;
            case 3:
                _spriteRenderer.color = Color.magenta;
                break;
            case 4:
                _spriteRenderer.color = Color.red;
                break;
            case 5:
                _spriteRenderer.color = Color.yellow;
                break;

            default:
                _spriteRenderer.color = Color.white;
                break;
        }

        StartCoroutine(DecayFood());
    }


    private IEnumerator DecayFood()
    {
        while (_stats.IsAlive)
        {
            yield return new WaitForSeconds(FOOD_DECAY_TIME);

            if (_stats.Food > 0)
            {
                _stats.Food -= FOOD_DECAY_AMOUNT;
                if (_stats.Food < 0)
                    _stats.Food = 0;
            }
            else// (_stats.Food <= 0)
            {
                _stats.Health--;
                if (_stats.Health <= 0)
                    StartCoroutine(Die());
            }
        }
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
        ProduceOffspring();
    }



    private void SearchForTarget()
    {
        if (_wondering) { return; }

        if (_target == null)
        {
            StartCoroutine(Wandering());
        }
        else
            _state = State.Seek;
    }

    private IEnumerator Wandering()
    {
        _wondering = true;
        float timer = 0.0f;
        float duration = 3.0f;

        // move in a sensible direction
        Vector3 randomPosition = new Vector3(Random.Range(-250.0f, 250.0f),
                                             Random.Range(-250.0f, 250.0f), 
                                             0.0f);
        Vector2 direction = (randomPosition - transform.position).normalized;

        if (!_isShocked)
            _rigidbody2D.velocity = direction * _stats.Speed;

        while (timer < duration && _target == null)
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

        if (!_isShocked)
        {
            Vector2 direction = (_target.position - transform.position).normalized;
            _rigidbody2D.velocity = direction * _stats.Speed;
        }

        if (Vector2.Distance(transform.position, _target.position) < 0.1f)
        {
            _rigidbody2D.velocity = Vector2.zero;

            if (_target.CompareTag("Food") ||
                _target.CompareTag("Superfood"))
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
            if (collider.CompareTag("Superfood"))
            {
                if (closestSuperfood != null)
                {
                    float distanceA = Mathf.Abs(Vector2.Distance(transform.position, closestSuperfood.position));
                    float distanceB = Mathf.Abs(Vector2.Distance(transform.position, collider.transform.position));

                    if (distanceB < distanceA)
                        closestSuperfood = collider.transform;
                }
                else
                    closestSuperfood = collider.transform;
            }
            else if (collider.CompareTag("Offspring") && _stats.CanAttack)
            {
                Offspring offspring = collider.GetComponent<Offspring>();
                if (_children.Contains(offspring) ||
                    offspring.Stats.IsAlive == false)
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
                if (organism.GetInstanceID() == GetInstanceID() ||
                    organism.Stats.CanAttack ||
                    organism.Stats.IsAlive == false)
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


    private void ProduceOffspring()
    {
        if (_stats.Food >= _foodRequired && HasPositiveTrait())
        {
            _stats.Food -= (int)(_foodRequired * 0.80f);
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


    private bool HasPositiveTrait()
    {
        foreach (Trait trait in _traits)
        {
            if (trait == Trait.Efficient)
                return true;
            else if (trait == Trait.Vigorous)
                return true;
            else if (trait == Trait.Growing)
                return true;
            else if (trait == Trait.Cunning)
                return true;
            else if (trait == Trait.Tough)
                return true;
            else if (trait == Trait.Lucky)
                return true;
        }

        return false;
    }



    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Wall"))
        {
            Vector2 direction = -transform.position.normalized;

            _rigidbody2D.velocity = direction * _stats.Speed;
        }
    }



    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_stats.IsAlive) { return; }


        if (collision.CompareTag("Superfood"))
        {
            Trait trait = Utility.PickTrait(_stats.Luck);
            _traits.Add(trait);

            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Food"))
        {
            _stats.HarvestFood(collision.GetComponent<FoodScript>().GetFood(false));
            ScaleWithFood();
            _rigidbody2D.velocity = Vector2.zero;
            _rigidbody2D.AddForce(Utility.BounceBack(transform.position, collision.transform.position));
            StartCoroutine(Shock());
        }
        else if (collision.CompareTag("SingleCelledOrganism") ||
                 collision.CompareTag("Player"))
        {
            BaseOrganism organism = collision.GetComponent<BaseOrganism>();
            if (organism.CanAttack)
            {
                print("Single Cell was attacked by another Single Cell");
                _stats.TakeDamage(organism.Stats.Damage);
                organism.Stats.Food += _stats.StealFood(organism.Stats.Damage);
                if (!_stats.IsAlive)
                    StartCoroutine(Die());
            }

            if (CanAttack && _stats.IsAlive)
            {
                print("Single Cell is attacking another Single Cell");
                organism.Stats.TakeDamage(_stats.Damage);
                _stats.Food += organism.Stats.StealFood(_stats.Damage);
                if (!organism.Stats.IsAlive)
                    StartSearching();
            }
        }
        else if (collision.CompareTag("Offspring"))
        {
            Offspring offspring = collision.GetComponent<Offspring>();
            if (_children.Contains(offspring))
                return;

            if (offspring.CanAttack)
            {
                print("Single Cell was attacked by an offspring");
                _stats.TakeDamage(offspring.Stats.Damage);
                offspring.Stats.Food += _stats.StealFood(offspring.Stats.Damage);
                if (!_stats.IsAlive)
                    StartCoroutine(Die());
            }

            if (CanAttack && _stats.IsAlive)
            {
                print("Single Cell is attacking an offspring");
                offspring.Stats.TakeDamage(_stats.Damage);
                _stats.Food += offspring.Stats.StealFood(_stats.Damage);
                if (!offspring.Stats.IsAlive)
                    StartSearching();

            }
        }
        //Edited Code ---------------------------------------------------------------------------------------------------
        else if (collision.CompareTag("EnvironmentalHazard"))
        {
            //To do
        }
        else if (collision.CompareTag("PushableHazard"))
        {
            //To do
        }
        else if (collision.CompareTag("MovementHazard"))
        {
            //To do
        }
    }


    private void StartSearching()
    {
        _rigidbody2D.velocity = Vector2.zero;
        _state = State.Search;
        _target = null;
    }


    private IEnumerator Die()
    {
        for (int i = 0; i < _children.Count; i++)
        {
            Destroy(_children[i]);

            yield return new WaitForEndOfFrame();
        }

        Destroy(gameObject);
    }


    public float FoodPercentage { get => Stats.Food / (float)_foodRequired; }
}
