using UnityEngine;

public abstract class BaseWeapon: MonoBehaviour
{
    [SerializeField] protected int _damage = 100;

    public virtual void Enable() { gameObject.SetActive(true); }
    public virtual void Disable() { gameObject.SetActive(false); }

    public virtual BaseWeapon Clone(Transform parent = null) { return Instantiate(this, parent); }
    public abstract void Shoot();
}