using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MovementAgent : MonoBehaviour
{
    [SerializeField]
    private Vector3 _StartPosition; 
    [SerializeField]
    private Vector3 _StartSpeed;
    private Vector3 _Speed;
    public Vector3 TargetPosition;
    [SerializeField]
    private float G;
    [SerializeField]
    private float _TargetMass;
    private const float TOLERANCE = 0.1f;
    [SerializeField]
    private GameObject _Target;
    
    //private Camera cam;

    
    void Start()
    {
        transform.position = _StartPosition;
        //cam = Camera.main;
        _Speed = _StartSpeed;
        _Target.transform.position = TargetPosition;
    }

    void FixedUpdate()
    {
        float distance = (TargetPosition - transform.position).magnitude;
        if (distance < TOLERANCE)
        {
            return;
        }
        Vector3 a = (TargetPosition - transform.position).normalized * (G * _TargetMass /
                                                                        (distance * distance));
        transform.position = transform.position + _Speed * Time.fixedDeltaTime +
                             a * (Time.fixedDeltaTime * Time.fixedDeltaTime / 2);
        _Speed = _Speed + a * Time.fixedDeltaTime;
    }
}
