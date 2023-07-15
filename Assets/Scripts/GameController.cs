using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Dictionary<int, GameObject> tiles;
    public CheckerColor colorToMove;

    int[] highlightedTiles;
    int selectedID;
    // Start is called before the first frame update
    void Start()
    {
        colorToMove = CheckerColor.RED;
        selectedID = -1;
        highlightedTiles = new int[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectChecker(int tileID)
    {
        if (tiles[tileID].transform.GetChild(0).GetComponent<CheckerData>().color != colorToMove)
        {
            return;
        }

        ClearAndUnhighlightTiles();

        if (selectedID == tileID)
        {
            UnhighlightSelectedTile();
            selectedID = -1;
            return;
        }

        selectedID = tileID;
        highlightedTiles = GenerateValidMoves(tileID);
        HighlightSelectedTile();
        HighlightTiles();

        return;
    }

    public void SelectTile(int tileID)
    {
        if (selectedID != -1)
        {
            //if
            Debug.Log(selectedID);
            Transform checker = tiles[selectedID].transform.GetChild(0);
            checker.SetParent(tiles[tileID].transform, false);
            checker.transform.position = tiles[tileID].transform.position;
            checker.GetComponent<CheckerData>().parentTileID = tileID;
            ClearAndUnhighlightTiles();
            selectedID = -1;
        }
        return;
    }

    void ClearAndUnhighlightTiles()
    {
        UnhighlightSelectedTile();
        foreach (int tileID in highlightedTiles)
        {
            tiles[tileID].GetComponent<TileController>().StopHighlight();
        }
        highlightedTiles = new int[0];
    }

    void HighlightTiles()
    {
        foreach (int tileID in highlightedTiles)
        {
            tiles[tileID].GetComponent<TileController>().Highlight();
        }
    }

    void HighlightSelectedTile()
    {
        tiles[selectedID].GetComponent<SpriteRenderer>().color = Color.green;
    }

    void UnhighlightSelectedTile()
    {
        if (selectedID < 0 || selectedID > 63)
        {
            return;
        }

        tiles[selectedID].GetComponent<TileController>().DisplayOriginalColor();
    }


    int[] GenerateValidMoves(int tileID)
    {
        int[] ret = { };
        Transform checkerTransform = tiles[tileID].transform.GetChild(0);
        CheckerData checkerData = checkerTransform.GetComponent<CheckerData>();

        if (checkerData.promoted)
        {
        }
        else
        {
            int size = 0;
            int factor = checkerData.color == CheckerColor.RED ? 1 : -1;

            int leftMove = GenerateOffsetUnpromotedMove(7, factor, checkerData, tileID);
            int rightMove = GenerateOffsetUnpromotedMove(9, factor, checkerData, tileID);

            if (leftMove > -1)
            {
                size++;
            }
            if (rightMove > -1)
            {
                size++;
            }
            
            ret = new int[size];

            int moveIndex = 0;
            if (leftMove > -1)
            {
                ret[moveIndex] = leftMove;
                moveIndex++;
            }
            if (rightMove > -1)
            {
                ret[moveIndex] = rightMove;
            }
        }
        return ret;
    }

    int GenerateOffsetUnpromotedMove(int offset, int factor, CheckerData checkerData, int tileID)
    {
        int id = tileID + offset * factor;
        if (id > 63 || id < 0)
        {
            return -1;
        }

        if ((int)(id / 8) == (int)(tileID / 8 + 1 * factor))
        {
            if (tiles[id].transform.childCount > 0)
            {
                if (tiles[id].transform.GetChild(0).GetComponent<CheckerData>().color != checkerData.color)
                {
                    id += offset * factor;
                }
                else
                {
                    id = -1;
                }

            }
            if (id > -1 && (id <= 63 && id >= 0) && tiles[id].transform.childCount > 0)
            {
                id = -1;
            }
        }
        else
        {
            id = -1;
        }

        return id;
    }
}
