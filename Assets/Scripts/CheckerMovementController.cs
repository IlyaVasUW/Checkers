using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckerMovementController : MonoBehaviour
{
    public GameController gameController;
    CheckerData data;
    Movement movement;
    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<Movement>();
        data = GetComponent<CheckerData>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseDown()
    {
        // Debug.Log("Selected Checker");
        gameController.SelectChecker(data.parentTileID);
    }
}
