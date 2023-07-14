using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Dictionary<int, GameObject> tiles;
    public CheckerColor colorToMove;
    // Start is called before the first frame update
    void Start()
    {
        colorToMove = CheckerColor.RED;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectChecker(int tileID)
    {
        return;
    }

    public void SelectTile(int tileID)
    {
        return;
    }
}
