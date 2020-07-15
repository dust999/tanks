using UnityEngine;
using UnityEngine.Events;

[RequireComponent (typeof(Collider))]
public abstract class BasePlayer : MonoBehaviour, IHealthable
{
    protected BasePlayerScriptableObject _basePlayerSettings = null;

    public int Health { get; protected set; } = 100;
    public int Arrmor { get; protected set; } = 100;

    public UnityEvent OnHitEvent;

    [SerializeField] private Transform _healthbarPosition = null;
    private HealthBar _healthBar = null;
    private Collider _collider = null;

    public BasePlayer Clone(Transform parent = null)
    {
        BasePlayer player = Instantiate(this, parent);
        player.Health = Health;
        player.Arrmor = Arrmor;
        return player;
    }

    public virtual void SetupPlayer(BasePlayerScriptableObject setup, HealthBar healthBar)
    {
        _basePlayerSettings = setup;

        Health =  _basePlayerSettings.health;
        Arrmor =  _basePlayerSettings.arrmor;

        _healthBar = healthBar.Clone(transform.parent);
        _healthBar.SetupHealthBar(_healthbarPosition, this);
    }

    public virtual void Hit(int hit)
    {
        if (HasArmor())
            Arrmor -= hit;
        else 
            Health -= hit;

        if (Arrmor < 0)
        {
            Health += Arrmor;
            Arrmor = 0;
        }
        
        OnHitEvent?.Invoke();

        if (!IsAlaive())
            Kill();
    }

    public virtual void Kill()
    {
        _collider.enabled = false;
    }

    public virtual bool IsAlaive()
    {
        return Health > 0;
    }

    public virtual bool HasArmor()
    {
        return Arrmor > 0;
    }

    public virtual void Reset()
    {
        if (_collider == null)
            _collider = GetComponent<Collider>();

        Health = _basePlayerSettings.health;
        Arrmor = _basePlayerSettings.arrmor;

        _collider.enabled = true;
        _healthBar?.Reset();
    }

    private void OnDestroy()
    {
        if(_healthBar != null)
        Destroy(_healthBar.gameObject);
    }
}
