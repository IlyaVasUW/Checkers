using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckerMovementController : MonoBehaviour
{
    Movement movement;
    bool isMoving = false;
    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<Movement>();
        isMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseDown()
    {
        Debug.Log("clicked checker");
        isMoving = true;
    }
}
