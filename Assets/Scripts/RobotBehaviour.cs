using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(7.5f, -6.5f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            transform.position = transform.position + Vector3.up;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.position = transform.position - Vector3.right;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            transform.position = transform.position - Vector3.up;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            transform.position = transform.position + Vector3.right;
        }
    }
}
