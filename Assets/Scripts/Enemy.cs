using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 10.0f;

    private Target _target;

    private Rigidbody _rigidbody;
    public event Action<Enemy> TargetTouched;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, Time.deltaTime * _speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent(out Target currentTarget) && (currentTarget == _target))
            TargetTouched?.Invoke(this);
    }

    public void GiveTarget(Target target)
    {
        _target = target;
    }

    public Rigidbody GetRigidbody()
    {
        return _rigidbody;
    }
}