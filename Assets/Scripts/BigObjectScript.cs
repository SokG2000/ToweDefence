using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigObjectScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MovementAgent small_agent_script = GameObject.Find("Sphere").GetComponent("MovementAgent") as MovementAgent;
        transform.position = small_agent_script.TargetPosition;
    }
}
