using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Player : BasePlayer
{
    private PlayerScriptableObject _playerSettings = null;
    private Rigidbody _playerRigidBody = null;
    private Direction _currentDirrection  = Direction.Forward;

    [SerializeField] private BaseWeapon [] _weapons = null;
    private int _currentWeapon = 0;

    public UnityEvent plyerDie = null;

    public override void SetupPlayer(BasePlayerScriptableObject setup, HealthBar healthBar)
    {
        base.SetupPlayer(setup, healthBar);

        _playerSettings = (PlayerScriptableObject) setup;

        CreateWeapons(_playerSettings.weapons);

    }

    private void CreateWeapons(BaseWeaponScriptableObject [] weapons)
    {
        if (weapons == null) return;

        _weapons = new BaseWeapon[weapons.Length];

        for(int i = 0; i < weapons.Length; i++)
        {
            _weapons[i] =  weapons[i].prefab.Clone(transform);
        }
    }

    private void Awake()
    {
        _playerRigidBody = GetComponent<Rigidbody>();

        StartCoroutine(PlayerUpdate());
    }

    private IEnumerator PlayerUpdate()
    {
        var waitFotFixedUpdate = new WaitForFixedUpdate();

        while (true) {

            PlayerMovement();
            WeaponUpdate();

            yield return waitFotFixedUpdate;
        }
    }

    private void PlayerMovement()
    {
        if (Input.GetKey(KeyCode.UpArrow))
            Move(true);
        else if (Input.GetKey(KeyCode.DownArrow))
            Move(false);

        if (Input.GetKey(KeyCode.LeftArrow))
            Rotate(Vector3.down);
        else if (Input.GetKey(KeyCode.RightArrow))
            Rotate(Vector3.up);

        _currentDirrection = Direction.Forward;
    }

    private void WeaponUpdate()
    {
        if (Input.GetKeyDown(KeyCode.X))
            Fire();

        if (Input.GetKeyDown(KeyCode.W))
            SwitchWeapon(Direction.Forward);
        else if (Input.GetKeyDown(KeyCode.Q))
            SwitchWeapon(Direction.Backward);
    }

    public void Move(bool isForward)
    {
        if(!isForward) _currentDirrection = Direction.Backward;
        _playerRigidBody.velocity = Vector3.zero;
        int direction = isForward ? 1 : -1;
        Vector3 deltaPosition =  direction * transform.forward * _basePlayerSettings.speed;
        _playerRigidBody.MovePosition (_playerRigidBody.position + deltaPosition);
    }

    public void Rotate(Vector3 direction)
    {
        _playerRigidBody.angularVelocity = Vector3.zero;
        direction *= ( _currentDirrection == Direction.Backward ) ? -1 : 1;
        Quaternion deltaRotation = Quaternion.Euler(direction * _basePlayerSettings.rotateSpeed);
        _playerRigidBody.MoveRotation(_playerRigidBody.rotation * deltaRotation);
    }

    private void SwitchWeapon(Direction direction) 
    {
        if (_weapons == null) return;

        _weapons[_currentWeapon]?.Disable();

        _currentWeapon += direction == Direction.Forward ? 1 : -1;
        if (_currentWeapon >= _weapons.Length) _currentWeapon = 0;
        else if (_currentWeapon < 0) _currentWeapon = _weapons.Length - 1;

        _weapons[_currentWeapon]?.Enable();
    }

    private void Fire()
    {
        _weapons[_currentWeapon]?.Shoot();       
    }

    public override void Kill()
    {
        StopAllCoroutines();
        plyerDie?.Invoke();
    }

    public override void Reset()
    {
        StopAllCoroutines();
        _currentDirrection = Direction.Forward;
        _currentWeapon = 0;
        transform.eulerAngles = Vector3.zero;

        base.Reset();
        
        StartCoroutine(PlayerUpdate());
    }
}
