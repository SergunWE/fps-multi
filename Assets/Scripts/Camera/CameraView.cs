﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraView : MonoBehaviour
{
    [SerializeField] private float xSensitivity;
    [SerializeField] private float ySensitivity;
    
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform cameraHolder;

    private float _xRotation = 0f;
    private float _yRotation = 0f;

    private Vector2 _inputAxis = Vector2.zero;

    private void Awake()
    {
        SetSensitivity();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        View();
    }

    private void View()
    {
        _yRotation += _inputAxis.x * xSensitivity;

        _xRotation -= _inputAxis.y * ySensitivity;
        _xRotation = Mathf.Clamp(_xRotation, -89f, 89f);

        if(cameraHolder == null) return;
        
        orientation.rotation = Quaternion.Euler(0, _yRotation, 0);
        cameraHolder.rotation = Quaternion.Euler(_xRotation, _yRotation, 0);
        
        _inputAxis = Vector2.zero;
    }

    private void SetSensitivity()
    {
        xSensitivity = PlayerPrefs.GetFloat("SensX", 1.0f);
        ySensitivity = PlayerPrefs.GetFloat("SensY", 1.0f);
    }
    
    public void OnView(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        _inputAxis = context.ReadValue<Vector2>();
    }

    private void OnEnable()
    {
        SetSensitivity();
    }

    private void OnDestroy()
    {
        //Debug.Log("Destroy camera");
        for (int i = 0; i < cameraHolder.childCount; i++)
        {
            if (!cameraHolder.TryGetComponent<IOnlineComponent>(out var iOnlineComponent))
            {
                Destroy(cameraHolder.GetChild(i).gameObject);
            }
            
        }
        //Destroy(cameraHolder.GetComponentInChildren<Camera>().gameObject);
    }
}
