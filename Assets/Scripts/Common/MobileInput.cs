using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileInput : MonoBehaviour
{

    private Joystick _joystick = null;
    [SerializeField] private Player _player = null;
    // 1Start is called before the first frame update
    private void Awake()
    {
        _joystick = GetComponent<DynamicJoystick>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_joystick.Vertical >= 0.2f)
            _player.Move(true);
        else if (_joystick.Vertical <= -0.2f)
            _player.Move(false);    
        
        if(_joystick.Horizontal <= -0.2f)
            _player.Rotate(Vector3.down);
        else if(_joystick.Horizontal >= 0.2f)
            _player.Rotate(Vector3.up);
        
    }
}
