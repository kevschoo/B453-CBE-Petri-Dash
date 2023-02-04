using System.Collections.Generic;
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



    void Awake()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider2D>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _stats.CalculateStats();
    }



    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _sightRadius);
    }
}
