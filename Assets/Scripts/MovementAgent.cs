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
    private float _Mass;
    [SerializeField]
    private float _TargetMass;
    private const float TOLERANCE = 0.1f;
    
    private Camera cam;

    
    void Start()
    {
        transform.position = _StartPosition;
        cam = GameObject.Find("Main Camera").GetComponent("Camera") as Camera;
        _Speed = _StartSpeed;
    }

    void FixedUpdate()
    {
        float distance = (TargetPosition - transform.position).magnitude;
        if (distance < TOLERANCE)
        {
            return;
        }
        
        Vector3 viewPos = cam.WorldToViewportPoint(transform.position);
        var x = viewPos.x;
        if ((x < 0 && _Speed.x < 0) || (x > 1 && _Speed.x > 0))
        {
            _Speed.x = -_Speed.x;
        }
        var y = viewPos.y;
        if ((y < 0 && _Speed.y < 0) || (y > 1 && _Speed.y > 0))
        {
            _Speed.y = -_Speed.y;
        }
        var z = viewPos.z;
        if (z < 0 && _Speed.z < 0)
        {
            _Speed.z = -_Speed.z;
        }

        Vector3 a = (TargetPosition - transform.position).normalized * (G * _TargetMass /
                                                                        (distance * distance));
        transform.position = transform.position + _Speed * Time.fixedDeltaTime +
                             a * (Time.fixedDeltaTime * Time.fixedDeltaTime);
        _Speed = _Speed + a * Time.fixedDeltaTime;
        
    }
}
