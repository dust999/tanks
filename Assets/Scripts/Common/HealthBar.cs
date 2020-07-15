using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private PositionConstraint _positionConstraint = null;
    [SerializeField] private TMP_Text _healthText = null;
    [SerializeField] private Slider _healthScrollBar = null;
    [SerializeField] private Image _scrollImage = null;
    [SerializeField] private Gradient _healthColor = null;
    private int _maxHealth = 100;
    private int _lastHealth = 0; // USED FOR UPDATE TEXT
    
    [Space]
    [SerializeField] private Slider _armorScrollBar = null;
    private int _maxArmor = 100;

    private BasePlayer _player = null;

    public void SetupHealthBar(Transform healthBarposition, BasePlayer player )
    {
        _player = player;
        _player.OnHitEvent.AddListener(GotHit);

        SaveStartValues();
        SetupTransforms(healthBarposition);
        SetupConstraint(healthBarposition);
        UpdateIndicators();
    }

    private void SetupTransforms(Transform healthBarposition)
    {
        //transform.position = healthBarposition.transform.position;
        transform.rotation = healthBarposition.rotation;
        transform.localScale = healthBarposition.localScale;
    }

    private void SetupConstraint(Transform healthBarposition)
    {
        _positionConstraint = gameObject.AddComponent<PositionConstraint>();

        ConstraintSource constraintSource = new ConstraintSource();
        constraintSource.sourceTransform = _player.transform;
        constraintSource.weight = 1f;
        
        _positionConstraint.AddSource(constraintSource);
        _positionConstraint.translationOffset = healthBarposition.localPosition;
        _positionConstraint.constraintActive = true;
    }

    private void SaveStartValues()
    {
        if (_player == null) return;

        _maxHealth = _player.Health;
        _lastHealth = _maxHealth;

        _maxArmor = _player.Arrmor;
    }

    private void GotHit()
    {
        if (_player.Health > 0)
            UpdateIndicators();
        else
            NoHealth();
    }

    private void UpdateIndicators()
    {
        int health = _player.Health;
        int arrmor = _player.Arrmor;
        
        UpdateHealthBar(health);

        UpdateArmor(arrmor);

        UpdateHealthText(health);
    }

    private void UpdateArmor(int arrmor)
    {
        if (arrmor <= 0) _armorScrollBar.gameObject.SetActive(false);
        if (arrmor > 0 && !_armorScrollBar.gameObject.activeSelf) _armorScrollBar.gameObject.SetActive(true);
        
        if(_maxArmor > 0)
        _armorScrollBar.value = arrmor * 1f / _maxArmor * 1f;
    }

    private void UpdateHealthBar(int health)
    {
        float healthPercent = health * 1f / _maxHealth * 1f;
        _healthScrollBar.value = healthPercent;
        _scrollImage.color = _healthColor.Evaluate(healthPercent);
    }

    private void UpdateHealthText(int health)
    {
        if (health == _lastHealth) return;

        _lastHealth = health;
        _healthText.text = health.ToString();
    }

    private void NoHealth()
    {
        _player.OnHitEvent.RemoveListener(GotHit);
        UpdateIndicators();
        gameObject.SetActive(false);
    }

    public void Reset()
    {
        UpdateIndicators();
        _lastHealth = _maxHealth;
        _player.OnHitEvent.AddListener(GotHit);
        gameObject.SetActive(true);
    }

    public HealthBar Clone(Transform parent = null)
    {
        HealthBar healthBar =  Instantiate(this, parent);       
        return healthBar;
    }
}