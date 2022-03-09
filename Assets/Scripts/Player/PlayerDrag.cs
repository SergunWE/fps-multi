﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDrag : MonoBehaviour, ITrackingGroundChecker
{
    private Rigidbody _rigidbody;

    [SerializeField] private float groundDrag;
    private bool _isGrounded;

    private void Awake()
    {
        _rigidbody = GetComponentInParent<Rigidbody>();
    }

    #region GameEvent

    public void OnPlayerGround()
    {
        _rigidbody.drag = groundDrag;
    }

    public void OnPlayerAir()
    {
        _rigidbody.drag = 0;
    }

    #endregion
}