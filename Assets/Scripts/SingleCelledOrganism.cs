using EnumHolder;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    }

    private void ProduceOffspring()
    {
        if (_stats.Food >= _foodRequired &&
            HasPositiveTrait())
        {
            _stats.Food -= (int)(_foodRequired * 0.75f);

            GameObject clone = Instantiate(_offspringPrefab, transform.position, transform.rotation);
            Offspring offspring = clone.GetComponent<Offspring>();

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
