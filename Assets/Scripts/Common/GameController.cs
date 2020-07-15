using UnityEngine;
using UnityEngine.Animations;

[RequireComponent (typeof(EnemySpawner))]
public class GameController : MonoBehaviour
{
    [SerializeField] private PlayerScriptableObject _playerSettings = null;
    private Player _playerAtScene = null;
    [SerializeField] private Transform _playerSpawnPosition = null;

    [Space]
    [SerializeField] private EnemyScriptableObject[] _enemies = null;
    [SerializeField] private UpdateEnemiesCount _enemiesCount = null;
    private EnemySpawner _enemySpawner = null;
    
    [Space]
    [SerializeField] private HealthBar _healthBar = null;
    [SerializeField] private GameObject _gameOver = null;

    [Space]
    [SerializeField] private Transform[] _spawnPoints = null;

    private void Awake()
    {
        _enemySpawner = GetComponent<EnemySpawner>();
        _enemiesCount?.Setup(_enemySpawner);
    }
    public void Start()
    {
        SpawnPlayer();
        _enemySpawner?. Setup(_enemies, _healthBar , _playerAtScene , _spawnPoints);
        _enemySpawner.GenerateEnemies();
    }

    public void GameOver()
    {
        _gameOver.SetActive(true);    
        _enemySpawner.Reset();
        _playerAtScene.Reset();
    }

    private void SpawnPlayer()
    {
        CreatePlayer();
        _playerAtScene.Reset();
        _playerAtScene.transform.position = _playerSpawnPosition.transform.position;
    }

    private void CreatePlayer()
    {
        if (_playerAtScene != null) return;

        _playerAtScene = (Player) Instantiate(_playerSettings.prefab);
        _playerAtScene.SetupPlayer(_playerSettings, _healthBar);
        _playerAtScene.plyerDie.AddListener(GameOver);

        AttachCamera();
    }

    private void AttachCamera()
    {
        PositionConstraint positonConstraint = Camera.main.gameObject.GetComponent<PositionConstraint>();
        positonConstraint.translationAxis = Axis.X;
        
        ConstraintSource constraintSource = new ConstraintSource();
        constraintSource.sourceTransform = _playerAtScene.transform;
        constraintSource.weight = 1f;
       
        positonConstraint.AddSource(constraintSource);
    }
}
