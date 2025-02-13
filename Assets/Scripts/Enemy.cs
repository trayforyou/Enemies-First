using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 10.0f;

    public event Action<Enemy> HasLeft;

    private void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * _speed);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.TryGetComponent<Terrain>(out _))
            HasLeft?.Invoke(GetComponent<Enemy>());
    }

    public Rigidbody GetRigidbody()
    {
        return GetComponent<Rigidbody>();
    }
}