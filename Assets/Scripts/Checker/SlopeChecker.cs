﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeChecker : Checker, ITrackingGroundChecker
{
    [SerializeField] private float maxSlopeAngle;
    [SerializeField] private float surfaceDetectionDistance;
    [SerializeField] private LayerMask groundMask;

    private RaycastHit _slopeHit;
    private bool _isGrounded;
    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    private void Update()
    {
        UpdateState();
    }

    protected override bool Check()
    {
        if (!_isGrounded) return false;
        if (!Physics.Raycast(_transform.position, Vector3.down, out _slopeHit,
            surfaceDetectionDistance, groundMask)) return false;
        float angle = Vector3.Angle(Vector3.up, _slopeHit.normal);
        return angle < maxSlopeAngle && angle != 0;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 moveDirection)
    {
        return Vector3.ProjectOnPlane(moveDirection, _slopeHit.normal).normalized;
    }

    #region GameEvent

    public void OnPlayerGround()
    {
        _isGrounded = true;
    }

    public void OnPlayerAir()
    {
        _isGrounded = false;
    }

    #endregion
}