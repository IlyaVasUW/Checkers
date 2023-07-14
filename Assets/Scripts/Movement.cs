using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public void setTarget(Vector3 target)
    {
        transform.position = target;
    }
}
