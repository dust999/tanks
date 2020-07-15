using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(Rigidbody), typeof(Collider))]
public class Rocket : MonoBehaviour
{
    [SerializeField] private GameObject _smoke = null;
    [SerializeField] private HitParticles _hit = null;

    private Rigidbody _rigidBody = null;
    private MeshRenderer _meshRenderer = null;

    [SerializeField] private float _flyAwayTime = 3f;
    [SerializeField] private float _hitReactivateTime = 2f;

    [Space]
    [SerializeField] private float _damageRadius = 1f;
    [SerializeField] private int _damage = 10;

    private bool isActivated = false;

    private DeattachableObject _deattachableFX = null;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _meshRenderer = GetComponent<MeshRenderer>();

        _deattachableFX = new DeattachableObject(transform);

        Reset();
    }

    private IEnumerator CheckHit()
    {
        var waitFixedUpdate = new WaitForFixedUpdate();
        RaycastHit hit;
        Ray ray;

        while (true)
        {
            ray = new Ray(transform.position, transform.forward);

            if (Physics.Raycast(ray, out hit, 1.5f))
            {               
                transform.position = hit.point;
                OnCollisionEnter(null);
               
                yield break;
            }
            yield return waitFixedUpdate;
        }
    }

    public bool LaunchRocket( float force )
    {
        if (isActivated) return false;

        Reset();

        SetKinematic(false);

        _rigidBody.velocity = transform.forward * force;

        StopAllCoroutines();
        StartCoroutine(CheckHit());


        if (_smoke != null) _smoke.SetActive(true);

        _deattachableFX.Deattach();

        CancelInvoke();
        Invoke(nameof(Reset), _flyAwayTime);

        return true;
    }
   
    private void OnCollisionEnter(Collision collision)
    {

        if (isActivated) return;

        StopAllCoroutines();

        HitEnemies();

        _meshRenderer.enabled = false;
        _hit.PlayStop();

        CancelInvoke();
        Invoke(nameof(Reset), _hitReactivateTime);

        isActivated = true;

        SetKinematic(true);
    }

    private void HitEnemies()
    {
       RaycastHit[] raycastHits = Physics.SphereCastAll(transform.position, _damageRadius, transform.forward, 0f);
       for( int i = 0; i<raycastHits.Length; i++)
        {
            if (raycastHits[i].transform.gameObject.layer != LayerMask.NameToLayer("Enemies")) continue;
            
            BasePlayer enemy = raycastHits[i].transform.gameObject.GetComponent<BasePlayer>();
            enemy.Hit(_damage);
        }
    }

    public void Reset()
    {
        isActivated = false;

        if (_smoke != null) _smoke.SetActive(false);

        SetKinematic(true);

        _deattachableFX.Attach();

        _meshRenderer.enabled = true;

        StopAllCoroutines();
    }

    private void SetKinematic(bool isActive = true)
    {
        _rigidBody.isKinematic = isActive;
        _rigidBody.velocity = Vector3.zero;
        _rigidBody.angularVelocity = Vector3.zero;
    }
}
