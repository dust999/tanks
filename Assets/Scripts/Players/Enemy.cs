using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent) , typeof(Animator))]
public class Enemy : BasePlayer
{
    private EnemyScriptableObject _enemySettings = null;

    [SerializeField] private BasePlayer _target = null;
    private Transform _playerTransform = null;

    [Space]
    private NavMeshAgent _navMeshAgent = null;
    
    [Space]
    private Animator _animator = null;
    private bool _isHit = false;

    public UnityEvent OnDead = null;

    public override void SetupPlayer(BasePlayerScriptableObject setup, HealthBar healthBar)
    {
        base.SetupPlayer(setup, healthBar);

        _enemySettings = (EnemyScriptableObject)setup;

        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();

        _navMeshAgent.speed = _basePlayerSettings.speed;
        _navMeshAgent.angularSpeed *= _basePlayerSettings.rotateSpeed;

        Reset();
    }

    public void SetTarget(BasePlayer target)
    {
        _target = target;
        _playerTransform = _target.transform;
        AttackPlayer();
    }

    private void PlayDeadAnimation(bool isActive = true)
    {
        _animator.SetBool(_enemySettings.deadBool, isActive);
    }

    private void PlayHitAnimation(bool isActive = true)
    {
        _isHit = isActive;
        _animator.SetBool(_enemySettings.hitBool, _isHit);
    }

    private IEnumerator CheckDistanceToTarget()
    {
        var waitEndOfFrame = new WaitForEndOfFrame();

        while (true) {
            
            if (Vector3.Distance(transform.position, _playerTransform.position) < _enemySettings.hitDistance)
                Hit();
            else
                Walk();

            yield return waitEndOfFrame;
        }
    }

    private void Walk()
    {
        if (_isHit == true)
            PlayHitAnimation(false);

        _navMeshAgent.SetDestination(_playerTransform.position);
    }

    private void Hit()
    { 
        PlayHitAnimation(true);

        Vector3 closestPosition = _playerTransform.position - transform.position;

        _navMeshAgent.SetDestination(transform.position);
        Invoke(nameof(SwitchHitAnimation), _enemySettings.switchHitAnimationTimer);
    }

    private void SwitchHitAnimation()
    {
        int animation = Random.Range(0, _enemySettings.hitAnimationsCount);
        _animator.SetInteger(_enemySettings.hitInt, animation);
    }

    private void SwitchDeathAnimation()
    { 
        int animation = Random.Range(0, _enemySettings.deadAnimationsCount);
        _animator.SetInteger(_enemySettings.deadInt, animation);
    }

    public void MakeHit()
    {
        _target.Hit(_enemySettings.damage);
    }

    private IEnumerator RemoveBody()
    {
        yield return new WaitForSeconds(3f);

        Vector3 newPosition = transform.position;
        var waitEndOfFreame = new WaitForEndOfFrame();

        while(transform.position.y > -5f)
        {
            newPosition.y -= 0.01f;
            transform.position = newPosition;
            yield return waitEndOfFreame;
        }

        gameObject.SetActive(false);

        yield return null;
    }

    public override void Reset()
    {
        StopAllCoroutines();
        _animator.Rebind();

        SwitchHitAnimation();
        SwitchDeathAnimation();

        PlayHitAnimation(false);
        PlayDeadAnimation(false);

        _navMeshAgent.enabled = true;

        base.Reset();
    }

    public void AttackPlayer()
    {
        StartCoroutine(CheckDistanceToTarget());
    }

    public override void Kill()
    {
        StopAllCoroutines();
        PlayDeadAnimation(); 

        _navMeshAgent.enabled = false;

        StartCoroutine(RemoveBody());

        base.Kill();

        OnDead?.Invoke();
    }
}
