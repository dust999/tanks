using UnityEngine;

public class RocketLauncher : BaseWeapon
{
    [SerializeField] private Rocket[] _rockets = null;
    [SerializeField] private float _speed = 50f;
    private int _currentRocket = 0;

    [SerializeField] private float _fireDelay = 1f;
    private float _nextTimeToShoot = 0f;

    [SerializeField] private ParticlesController _launchParticles = null;

    public override void Enable()
    {
        base.Enable();

        if (_rockets == null) return;
        for(int i = 0; i >= _rockets.Length; i++)
        {
            _rockets[i]?.gameObject.SetActive(true);
        }
    }

    public override void Shoot()
    {
        if (Time.time < _nextTimeToShoot ) return;

        _currentRocket++;
        if (_currentRocket >= _rockets.Length) _currentRocket = 0;

        if ( !_rockets[_currentRocket].LaunchRocket( _speed ) ) return;

        _launchParticles?.PlayStop();

        _nextTimeToShoot = Time.time + _fireDelay;
    }

    public override void Disable()
    {
        for (int i = 0; i >= _rockets.Length; i++)
        {
            _rockets[i]?.Reset();
            _rockets[i]?.gameObject.SetActive(false);
        }

        base.Disable();
    }
}
