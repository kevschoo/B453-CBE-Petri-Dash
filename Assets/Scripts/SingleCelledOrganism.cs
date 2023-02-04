using System.Collections.Generic;
using System.Linq;
using UnityEngine;
// enums
using EnumHolder;
using System.Diagnostics.Tracing;

public class SingleCelledOrganism : BaseOrganism
{
    enum State
    {
        Search,
        Seek,
        Harvest,
        Fight,
    }


    enum Priority
    {
        Superfood,
        Offspring,
        Food
    }


    [SerializeField]
    List<Offspring>     _offspring;
    [SerializeField]
    GameObject          _offspringPrefab;
    [SerializeField]
    int                 _foodRequired;

    Transform           _target;
    State               _state;



    void Awake()
    {
        _state = State.Search;
        _target = null;
    }



    void Start()
    {
        
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

    }


    private void SeekToTarget()
    {

    }


    private void HarvestTarget()
    {

    }


    private void FightTarget()
    {

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
                if (_offspring.Contains(offspring))
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
                if (organism._stats.CanAttack)
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
            _stats.Food -= (int)(_foodRequired * 0.75f);

            // instantiate the offspring
            GameObject clone = Instantiate(_offspringPrefab, 
                                            transform.position, 
                                            transform.rotation);
            Offspring offspring = clone.GetComponent<Offspring>();
            offspring.AssignParent(this);
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
}
