using TMPro;
using UnityEngine;

public class UpdateEnemiesCount : MonoBehaviour
{
    private EnemySpawner _enemySpawner = null;
    private TMP_Text _text = null;
    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }
    
    public void Setup(EnemySpawner enemySpawner)
    {
        _enemySpawner = enemySpawner;
        _enemySpawner.OnChangeEnemiesCount.AddListener(UpdateCount);
    }

    private void UpdateCount()
    {
        _text.text = _enemySpawner.GetActiviedEnemies().ToString();
    }
}
