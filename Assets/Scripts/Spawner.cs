using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private List<SpawnPoint> _spawnList;
    [SerializeField] private Enemy _enemy;
    [SerializeField] private int _defaultPoolSize = 5;
    [SerializeField] private int _defaultMaxSize = 5;
    [SerializeField] private float _createFrequency = 2f;
    
    private ObjectPool<Enemy> _enemyPool;
    private bool _isSpawning;
    private Coroutine _coroutine;

    private void Awake()
    {
        _enemyPool = new ObjectPool<Enemy>
            (
            createFunc: () => CreateEnemy(),
            actionOnGet: (enemy) => SpawnEnemy(enemy),
            actionOnRelease: (enemy) => enemy.gameObject.SetActive(false),
            actionOnDestroy: (enemy) => DestroyEnemy(enemy),
            collectionCheck: true,
            defaultCapacity: _defaultPoolSize,
            maxSize: _defaultMaxSize
            );

        _isSpawning = true;
    }

    private void OnEnable()
    {
        _coroutine = StartCoroutine(SpawnEnemies());
    }

    private void OnDisable()
    {
        StopCoroutine(_coroutine);
        _isSpawning = false;
    }

    private Enemy CreateEnemy()
    {
        Enemy newEnemy = Instantiate(_enemy);
        newEnemy.TargetTouched += Hide;

        return newEnemy;
    }

    private void SpawnEnemy(Enemy enemy)
    {
        Rigidbody currentRigidbody = enemy.GetRigidbody();

        currentRigidbody.linearVelocity = Vector3.zero;
        currentRigidbody.angularVelocity = Vector3.zero;
        transform.position = default;

        enemy.gameObject.SetActive(true);
    }

    private void Hide(Enemy enemy)
    {
        _enemyPool.Release(enemy);
    }

    private void DestroyEnemy(Enemy enemy)
    {
        enemy.TargetTouched -= Hide;

        Destroy(enemy.gameObject);
    }

    private IEnumerator SpawnEnemies()
    {
        var wait = new WaitForSeconds(_createFrequency);

        Enemy currentEnemy;

        while (_isSpawning)
        {
            SpawnPoint currentSpawnPoint = _spawnList[UnityEngine.Random.Range(0, _spawnList.Count)];
            Target currentTarget = currentSpawnPoint.TargetPosition;
            currentEnemy = _enemyPool.Get();
            currentEnemy.transform.position = currentSpawnPoint.transform.position;

            currentEnemy.GiveTarget(currentTarget);

            yield return wait;
        }
    }
}