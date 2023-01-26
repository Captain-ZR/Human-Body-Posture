using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube_Control : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
           
    }
    void Update()
    {

        float rotateSpeed = 180;
        this.transform.Rotate(0, rotateSpeed * Time.deltaTime, 0, Space.Self);
    }
}
