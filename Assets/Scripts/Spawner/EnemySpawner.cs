using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    private Transform[] _spawnPoints = null;
    private List<Enemy> _enemies = new List<Enemy>();

    private EnemyScriptableObject [] _enemiesOriginal = null;
    private HealthBar _healthBar = null;
    private GameObject _enemySpwanObject = null;
    private BasePlayer _target = null;
    
    [Space]
    [SerializeField] private float _updateEnemiesDelay = 3f;
    [SerializeField] private int _maxEnemies = 10;
    public UnityEvent OnChangeEnemiesCount = null;

    private int _currentSpawnPoint = 0;

    public void Setup (EnemyScriptableObject[] enemies, HealthBar healthBar, BasePlayer target , Transform [] spawnPoints)
    {
        _enemiesOriginal = enemies;
        _spawnPoints = spawnPoints;
        _healthBar = healthBar;

        if(_enemySpwanObject == null)
        _enemySpwanObject = new GameObject("Enemies");

        _target = target;

        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        var delay = new WaitForSeconds(_updateEnemiesDelay);
        while (true)
        {
            GenerateEnimies();
            yield return delay;
        }
    }

    public void GenerateEnimies()
    {
        int needEneimes = _maxEnemies - GetActiviedEnemies();
        if (needEneimes <= 0) return;

        for (int i = 0; i<needEneimes; i++)
        {
            Enemy enemy = SpawnEnemy();

            if (enemy == null) continue;

            enemy.transform.position = _spawnPoints[_currentSpawnPoint].transform.position;
            enemy.gameObject.SetActive(true);
            enemy.AttackPlayer();

            NotifyChangeEnimesCount();

            _currentSpawnPoint++;
            _currentSpawnPoint = _currentSpawnPoint >= _spawnPoints.Length ? 0 : _currentSpawnPoint;
        }
    }

    public int GetActiviedEnemies()
    {
        int count = 0;
        foreach(Enemy enemy in _enemies)
        {
            if (enemy.gameObject.activeInHierarchy) count++;
        }
        return count;
    }

    private Enemy SpawnEnemy()
    {
        foreach (Enemy enemy in _enemies)
        {
            if (enemy.gameObject.activeInHierarchy) continue;

            enemy.Reset();
            return enemy;
        }

        return CreateInstance();
    }

    private Enemy CreateInstance()
    {
        int randomEnemy = Random.Range( 0, _enemiesOriginal.Length );

        if ( _enemiesOriginal[randomEnemy] == null) return null;
        
        Enemy enemy = (Enemy) _enemiesOriginal[randomEnemy].prefab.Clone(_enemySpwanObject.transform);
        enemy.SetupPlayer(_enemiesOriginal[randomEnemy], _healthBar);
        enemy.SetTarget(_target);
        enemy.OnDead.AddListener(NotifyChangeEnimesCount);

        _enemies.Add(enemy);

        return enemy;
    }

    public void Reset()
    {
        StopAllCoroutines();
        
        foreach(Enemy enemy in _enemies)
        {
            Destroy(enemy.gameObject);
        }

        _enemies = new List<Enemy>();
        NotifyChangeEnimesCount();
        _currentSpawnPoint = 0;
    }

    private void NotifyChangeEnimesCount()
    {
        OnChangeEnemiesCount?.Invoke();
    }
}
