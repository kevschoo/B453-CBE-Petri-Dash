using System.Collections.Generic;
using System.Collections;
using UnityEngine;
// for Trait
using EnumHolder;

public abstract class BaseOrganism : MonoBehaviour
{
    [SerializeField]
    protected Stats           _stats;
    [SerializeField]
    protected List<Trait>     _traits;
    [SerializeField]
    protected float           _sightRadius;

    protected Animator        _animator;
    protected Collider2D      _collider;
    protected Rigidbody2D     _rigidbody2D;
    protected SpriteRenderer  _spriteRenderer;

    protected Transform       _target;

    protected bool            _isShocked;



    protected void Awake()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider2D>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _isShocked = false;

        _stats.CalculateStats();
    }



    public void AddTrait(Trait trait)
    {
        switch (trait)
        {
            case Trait.Efficient:
                _stats.FoodMultiplier += 0.25f;
                break;
            case Trait.Vigorous:
                _stats.RecalculateHealth(0.25f);
                break;
            case Trait.Growing:
                StartCoroutine(Grow());
                break;
            case Trait.Cunning:
                Stats.RecalculateSpeed(0.25f);
                break;
            case Trait.Tough:
                _stats.RecalculateDamage(0.5f);
                break;
            case Trait.Lucky:
                _stats.Luck += 0.05f;
                break;

            case Trait.Clumsy:
                _stats.Food -= (int)(_stats.Food * 0.25f);
                break;
            case Trait.Cowardly:
                _stats.RecalculateDamage(-_stats.DamageMultiplier);
                break;
            case Trait.Unlucky:
                _stats.Luck -= 0.1f;
                break;
            case Trait.Weak:
                _stats.RecalculateHealth(-0.33f);
                break;
            case Trait.Leisurely:
                _stats.RecalculateSpeed(-0.33f);
                break;

            default:
                break;
        }

        _traits.Add(trait);
    }


    private IEnumerator Grow()
    {
        float scale = transform.localScale.x;
        float statBoost = 0.05f;
        float everySeconds = 20.0f;

        while (true)
        {
            _stats.RecalculateHealth(statBoost);
            _stats.RecalculateDamage(statBoost);
            _stats.RecalculateSpeed(statBoost);

            scale += 0.01f;
            transform.localScale = new Vector3(scale, scale, 1.0f);

            yield return new WaitForSeconds(everySeconds);
        }
    }



    public void TakeDamage(int damage)
    {
        _stats.TakeDamage(damage);
        if (!_stats.IsAlive)
        {
            Destroy(gameObject);
        }
    }



    public void ResetTarget()
    {
        _target = null;
    }



    public void ScaleWithFood()
    {
        float scale = 1.0f + Stats.Food * 0.0025f;
        transform.localScale = new Vector3(scale, scale, 1.0f);
    }



    protected IEnumerator Shock()
    {
        _isShocked = true;

        yield return new WaitForSeconds(0.1f);

        _isShocked = false;
    }



    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _sightRadius);
    }



    public Stats Stats { get => _stats; }

    public bool CanAttack { get => _stats.CanAttack; }
}
