using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckerMovementController : MonoBehaviour
{
    public GameController gameController;
    CheckerData data;
    Movement movement;
    bool isMoving = false;
    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<Movement>();
        data = GetComponent<CheckerData>();
        isMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseDown()
    {
        isMoving = true;
        gameController.SelectChecker(data.parentTileID);
    }
}
