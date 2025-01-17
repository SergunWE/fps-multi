﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class PlayerMovement : MonoBehaviour, ITrackingGroundChecker, ITrackingCrouch
{
    [SerializeField] private float baseSpeed;
    [SerializeField] private float movementSpeedMultiplier;
    [SerializeField] private float airMultiplier;
    [SerializeField] private Transform orientation;
    [SerializeField] private SlopeChecker slopeChecker;
    [SerializeField] private float sprintSpeedMultiplier;
    [SerializeField] private float crouchSpeedMultiplier;

    [SerializeField] private Vector2Reference movementAxis; 


    private Vector2 _inputAxis = Vector2.zero;
    private Vector3 _moveDirection;
    private Rigidbody _rigidbody;
    private bool _exitingSlope = false;
    private float _movementSpeed;
    private bool _isCrouched = false;
    private bool _isGrounded = false;
    private bool _isSloped = false;
    private bool _isSprinted = false;


    private void Awake()
    {
        _rigidbody = GetComponentInParent<Rigidbody>();
    }

    private void Update()
    {
        //SpeedChange();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        _moveDirection = _inputAxis.y * orientation.forward  + _inputAxis.x * orientation.right;

        if (!_exitingSlope && _isSloped)
        {
            //Debug.Log("Slope");
            _rigidbody.AddForce(_movementSpeed * movementSpeedMultiplier * 
                                slopeChecker.GetSlopeMoveDirection(_moveDirection), ForceMode.Acceleration);
        }

        else if (_isGrounded)
        {
            _rigidbody.AddForce(movementSpeedMultiplier * _movementSpeed * _moveDirection, ForceMode.Acceleration);
        }
        else
        {
            //Debug.Log("Air");
            _rigidbody.AddForce(movementSpeedMultiplier * _movementSpeed * airMultiplier * _moveDirection,
                ForceMode.Acceleration);
        }

        _rigidbody.useGravity = !_isSloped;

        SpeedControl();
    }

    private void SpeedControl()
    {
        var velocity = _rigidbody.velocity;
        if (!_exitingSlope && _isSloped)
        {
            if (velocity.magnitude > _movementSpeed)
            {
                //Debug.Log("Снижение скорости - склон");
                _rigidbody.velocity = _movementSpeed * velocity.normalized;
            }
        }
        else
        {
            Vector3 flatVel = new Vector3(velocity.x, 0f, velocity.z);

            if (flatVel.magnitude > _movementSpeed)
            {
                //Debug.Log("Снижение скорости");
                Vector3 limitedVel = _movementSpeed * flatVel.normalized;
                _rigidbody.velocity = new Vector3(limitedVel.x, _rigidbody.velocity.y, limitedVel.z);
            }
        }
    }

    private void SpeedChange()
    {
        switch (_isGrounded)
        {
            case true when _isCrouched:
                _movementSpeed = baseSpeed * crouchSpeedMultiplier;
                break;
            case true when _isSprinted:
                _movementSpeed = baseSpeed * sprintSpeedMultiplier;
                break;
            case true:
                _movementSpeed = baseSpeed;
                break;
        }
    }

    #region InputEvent

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //Debug.Log("Sprint");
            _isSprinted = true;
        }
        else
        {
            if (!context.canceled) return;
            _isSprinted = false;
        }

        SpeedChange();
    }

    #endregion

    #region GameEvent

    public void OnMovementChange()
    {
        _inputAxis = movementAxis;
    }
    
    public void OnPlayerGround()
    {
        _isGrounded = true;
        _exitingSlope = false;
        SpeedChange();
    }

    public void OnPlayerAir()
    {
        _isGrounded = false;
        SpeedChange();
    }

    public void OnPlayerPlains()
    {
        _isSloped = false;
    }

    public void OnPlayerSlope()
    {
        _isSloped = true;
    }

    public void OnPlayerJumped()
    {
        _exitingSlope = true;
    }

    public void OnPlayerCrouches()
    {
        _isCrouched = true;
        SpeedChange();
    }

    public void OnPlayerStandUp()
    {
        _isCrouched = false;
        SpeedChange();
    }

    #endregion

    #region Enable/Disable

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    #endregion
}