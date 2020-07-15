using System.Collections;
using UnityEngine;

public class MachineGun : BaseWeapon
{   
    [SerializeField] private ParticlesController _gunFX = null;
    [SerializeField] private ParticlesController _hitFX = null;

    [SerializeField] private Transform _bulletShells = null;
    [SerializeField] private float _hitRange = 30f;
    [SerializeField] private float _randomOffset = 0.5f;
    [SerializeField] private float _restBullerShellsTime = 10f;

    private bool _isActivatedHit = false;

    private DeattachableObject _deattachableBulletShells = null;

    private bool _isShooting = false;

    private void Awake()
    {
        _deattachableBulletShells = new DeattachableObject(_bulletShells);
        _gunFX?.StartStopLoop();
    }
    public override void Shoot()
    {
        if (_isShooting)
        {
            StopShoting();
            return;
        }

        ResetBuletShels();

        ActivateShooting();

        StopAllCoroutines();
        StartCoroutine(CheckHit());
    }

    private void StopShoting()
    {
        StopAllCoroutines();

        ActivateShooting(false);

        ActivateHit(false);
    }

    private IEnumerator CheckHit()
    {
        var waitFixedUpdate = new WaitForFixedUpdate();
        Ray ray;
        RaycastHit hit;
        Vector3 randomOffset = Vector3.zero;
        BasePlayer enemy = null;

        while (true)
        {
            ray = new Ray(transform.position, transform.up * -1);
            
            if( Physics.Raycast(ray, out hit, _hitRange) ){

                if (!_isActivatedHit)
                    ActivateHit(true);

                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Enemies"))
                {
                    enemy = hit.transform.gameObject.GetComponent<BasePlayer>();
                    enemy.Hit(_damage);
                }

                randomOffset.x = Random.Range(-1 * _randomOffset, _randomOffset);
                randomOffset.y = Random.Range(-1 * _randomOffset, _randomOffset);

                _hitFX.transform.position = hit.point + randomOffset;

                yield return waitFixedUpdate;
                continue;
            }

            ActivateHit(false);

            yield return waitFixedUpdate;
        }

    }

    private void ActivateShooting(bool isActive = true)
    {
        _isShooting = isActive;
        _gunFX?.PlayStop(isActive);
    }

    private void ActivateHit ( bool isActive = true)
    {
        _isActivatedHit = isActive;
        _hitFX?.PlayStop(isActive);
    }

    public override void Disable()
    {
        StopShoting();

        _deattachableBulletShells.Deattach();
        Invoke( nameof(ResetBuletShels), _restBullerShellsTime );

        base.Disable();
    }

    private void ResetBuletShels()
    {
        _deattachableBulletShells.Attach();
    }
}
