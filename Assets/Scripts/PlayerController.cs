using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private Animator _animator;
    [SerializeField] private float forceStrength = 50;

    private Camera mainCamera;
    
    private float _velocityX;
    private float _velocityZ;
    
    private bool isMoving;
    
    private static readonly int VelocityX = Animator.StringToHash("velocityX");
    private static readonly int VelocityZ = Animator.StringToHash("velocityZ");
    
    private Rigidbody _rb;

    private void Awake()
    {
        mainCamera = Camera.main;
        _rb = GetComponent<Rigidbody>();
    }
    
    private void Update()
    {
        _velocityX = Input.GetAxis("Horizontal");
        _velocityZ = Input.GetAxis("Vertical");
        
        isMoving = _velocityX != 0 || _velocityZ != 0;
        
        LookAtMouse();
        Move();
        
        if (Input.GetKeyDown(KeyCode.Space))
            Dash();
    }

    private void LookAtMouse()
    {
        var mousePos = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(mousePos, out var hit)) 
            return;
        var direction = hit.point - transform.position;
        direction.y = 0;
        transform.forward = direction;
    }

    private void Move()
    {
        var pos = new Vector3(_velocityX, 0, _velocityZ) * speed * Time.deltaTime;
        transform.position += pos;

        var animVelocityX = _velocityX * transform.right.x + _velocityZ * transform.forward.x;
        var animVelocityZ = _velocityZ * transform.forward.z - _velocityX * transform.right.z;
        if (isMoving)
        {
            _animator.SetFloat(VelocityX, animVelocityX);
            _animator.SetFloat(VelocityZ, animVelocityZ);
        }
        else
        {
            _animator.SetFloat(VelocityX, 0);
            _animator.SetFloat(VelocityZ, 0);
        }
    }


    private void Dash()
    {
        Vector3 dir;
        if (isMoving)
            dir = new Vector3(_velocityX, 0, _velocityZ);
        else
            dir = transform.forward;
        _rb.AddForce(dir*forceStrength, ForceMode.Impulse);
    }
}