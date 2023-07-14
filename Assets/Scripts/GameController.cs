using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Dictionary<int, GameObject> tiles;
    public CheckerColor colorToMove;

    GameObject[] highlightedTiles;
    int selectedID;
    // Start is called before the first frame update
    void Start()
    {
        colorToMove = CheckerColor.RED;
        selectedID = -1;
        highlightedTiles = new GameObject[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectChecker(int tileID)
    {
        foreach(GameObject tile in highlightedTiles)
        {
            tile.GetComponent<TileController>().StopHighlight();
        }

        if (selectedID == tileID)
        {
            highlightedTiles = new GameObject[0];
            selectedID = -1;
            return;
        }

        selectedID = tileID;
        int[] validTiles = GenerateValidMoves(tileID);
        highlightedTiles = new GameObject[validTiles.Length];

        for(int i = 0; i < validTiles.Length; i++)
        {
            tiles[validTiles[i]].GetComponent<TileController>().Highlight();
            highlightedTiles[i] = tiles[validTiles[i]];
        }
        return;
    }

    public void SelectTile(int tileID)
    {
        if (selectedID != -1)
        {
            selectedID = -1;
        }
        return;
    }

    int[] GenerateValidMoves(int tileID)
    {

        // Return all moves as valid for now
        int[] ret = new int[1];
        ret[0] = tileID + 8;

        return ret;
    }
}
