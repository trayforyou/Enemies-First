using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 10.0f;
    private bool _isMove;

    public event Action<Enemy> LeftZone;

    private void OnEnable()
    {
        _isMove = true;
    }

    private void OnDisable()
    {
        _isMove = false;
    }

    private void Update()
    {
        if (_isMove)
        transform.Translate(Vector3.forward * Time.deltaTime * _speed);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.TryGetComponent<Terrain>(out _))
            LeftZone?.Invoke(GetComponent<Enemy>());
    }

    public void GetRigidbody(out Rigidbody rigidbody)
    {
        rigidbody = GetComponent<Rigidbody>();
    }
}