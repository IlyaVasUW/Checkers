using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class GameController : MonoBehaviour
{
    public Dictionary<int, GameObject> tiles;
    public CheckerColor colorToMove;

    int[] highlightedTiles;
    int selectedID;
    // Start is called before the first frame update
    void Start()
    {
        colorToMove = CheckerColor.BLACK;
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
        bool tileID_is_valid_move = false;
        if (selectedID != -1) // prev clicked tile has a checker piece
        {
            //if
            Debug.Log(selectedID);
            Transform checker = tiles[selectedID].transform.GetChild(0);
            int[] validTiles = GenerateValidMoves(selectedID);
            for (int i = 0; i < validTiles.Length; i++)
            {
                if (validTiles[i] == tileID)
                {
                    tileID_is_valid_move = true;
                }
            }
            if (tileID_is_valid_move)
            {
                bool isKillMove = false;
                checker.SetParent(tiles[tileID].transform, false);
                checker.transform.position = tiles[tileID].transform.position;
                checker.GetComponent<CheckerData>().parentTileID = tileID;
                if (tileID > 55 && tileID < 64 && checker.GetComponent<CheckerData>().promoted == false)
                {
                    checker.GetComponent<CheckerData>().promoted = true;
                }
                ClearAndUnhighlightTiles();
                selectedID = -1;

                if (!isKillMove)
                {
                    colorToMove = colorToMove == CheckerColor.RED ? CheckerColor.BLACK : CheckerColor.RED; // CHANGES PLAYER TO MOVE
                }
                
            }
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
        int[] ret;
        Transform checkerTransform = tiles[tileID].transform.GetChild(0);
        CheckerData checkerData = checkerTransform.GetComponent<CheckerData>();

        int size = 0;
        int factor = checkerData.color == CheckerColor.RED ? -1 : 1;

        if (checkerData.promoted)
        {
            /*
            int[] topLeftMoves = GenerateOffsetPromotedMoves(7, factor, checkerData, tileID);
            int[] topRightMoves = GenerateOffsetPromotedMoves(9, factor, checkerData, tileID);
            int[] bottomLeftMoves = GenerateOffsetPromotedMoves(-7, factor, checkerData, tileID);
            int[] bottomRightMoves = GenerateOffsetPromotedMoves(-9, factor, checkerData, tileID);

            size = topLeftMoves.Length + topRightMoves.Length + bottomLeftMoves.Length + bottomRightMoves.Length;
            ret = new int[size];
            int moveIndex = 0;

            for (int i = 0; i < topLeftMoves.Length; i++)
            {
                ret[moveIndex++] = topLeftMoves[i];
            }
            for (int i = 0; i < topRightMoves.Length; i++)
            {
                ret[moveIndex++] = topRightMoves[i];
            }
            for (int i = 0; i < bottomLeftMoves.Length; i++)
            {
                ret[moveIndex++] = bottomLeftMoves[i];
            }
            for (int i = 0; i < bottomRightMoves.Length; i++)
            {
                ret[moveIndex++] = bottomRightMoves[i];
            }
            */

            int topLeftMove = GenerateOffsetUnpromotedMove(7, factor, checkerData, tileID);
            int topRightMove = GenerateOffsetUnpromotedMove(9, factor, checkerData, tileID);
            int bottomLeftMove = GenerateOffsetUnpromotedMove(-7, factor, checkerData, tileID);
            int bottomRightMove = GenerateOffsetUnpromotedMove(-9, factor, checkerData, tileID);

            if (topLeftMove > -1)
            {
                size++;
            }
            if (topRightMove > -1)
            {
                size++;
            }
            if (bottomLeftMove > -1)
            {
                size++;
            }
            if (bottomRightMove > -1)
            {
                size++;
            }

            ret = new int[size];

            int moveIndex = 0;
            if (topLeftMove > -1)
            {
                ret[moveIndex] = topLeftMove;
                moveIndex++;
            }
            if (topRightMove > -1)
            {
                ret[moveIndex] = topRightMove;
            }
            if (bottomLeftMove > -1)
            {
                ret[moveIndex] = bottomLeftMove;
                moveIndex++;
            }
            if (bottomRightMove > -1)
            {
                ret[moveIndex] = bottomRightMove;
            }

            return ret;
        }
        else
        {
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

    int[] GenerateOffsetPromotedMoves(int offset, int factor, CheckerData checkerData, int tileID)
    {
        int[] ret = { };
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
                CheckerData childData = tiles[id].transform.GetChild(0).GetComponent<CheckerData>();
                if (childData.color != checkerData.color && childData.dead != true)
                {
                    id += offset * factor;
                    if((int)(id / 8) != (int)(tileID / 8 + 2 * factor))
                    {
                        id = -1;
                    }
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
