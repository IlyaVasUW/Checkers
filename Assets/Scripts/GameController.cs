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
    Transform deadCheckers;
    bool isCaptureInEffect;
    // Start is called before the first frame update
    void Start()
    {
        colorToMove = CheckerColor.BLACK;
        selectedID = -1;
        highlightedTiles = new int[0];
        deadCheckers = transform.Find("Dead Checkers");
        isCaptureInEffect = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectChecker(int tileID)
    {
        if (isCaptureInEffect) //prevent clicking off cpaturing checker
        {
            return;
        }
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
            Transform checker = tiles[selectedID].transform.GetChild(0);
            int[] validTiles = GenerateValidMoves(selectedID);
            for (int i = 0; i < validTiles.Length; i++)
            {
                if (validTiles[i] == tileID)
                {
                    tileID_is_valid_move = true;
                    break;
                }
            }
            if (tileID_is_valid_move) //move is valid
            {
                Transform capturedChecker = IsMoveCapture(selectedID, tileID);
                checker.SetParent(tiles[tileID].transform, false);
                checker.transform.position = tiles[tileID].transform.position;
                checker.GetComponent<CheckerData>().parentTileID = tileID;
                if (colorToMove == CheckerColor.BLACK && tileID > 55 && tileID < 64 && checker.GetComponent<CheckerData>().promoted == false)
                {
                    checker.GetComponent<CheckerData>().promoted = true;
                    checker.GetComponent<CheckerDisplay>().UpdatePromoteSprite();
                }
                if (colorToMove == CheckerColor.RED && tileID > -1 && tileID < 8 && checker.GetComponent<CheckerData>().promoted == false)
                {
                    checker.GetComponent<CheckerData>().promoted = true;
                    checker.GetComponent<CheckerDisplay>().UpdatePromoteSprite();
                }
                ClearAndUnhighlightTiles();
                selectedID = -1;
                if (capturedChecker == null)
                {
                    colorToMove = colorToMove == CheckerColor.RED ? CheckerColor.BLACK : CheckerColor.RED; // CHANGES PLAYER TO MOVE
                    isCaptureInEffect = false;
                }
                else
                {
                    capturedChecker.SetParent(deadCheckers, false);
                    capturedChecker.GetComponent<CheckerData>().SetDead(true);
                    isCaptureInEffect = false;
                    SelectChecker(tileID);
                    if (highlightedTiles.Length == 0)
                    {
                        UnhighlightSelectedTile();
                        ClearAndUnhighlightTiles();
                        selectedID = -1;
                        colorToMove = colorToMove == CheckerColor.RED ? CheckerColor.BLACK : CheckerColor.RED; // CHANGES PLAYER TO MOVE
                        isCaptureInEffect = false;
                    }
                    else 
                    { 
                        isCaptureInEffect = true;
                    }
                }
            }
        }
        RefreshCheckersDisplay();
        return;
    }

    void RefreshCheckersDisplay()
    {
        for (int i = 0; i < 63; i++)
        {
            if (tiles[i].transform.childCount > 0)
            {
                tiles[i].transform.GetChild(0).GetComponent<CheckerDisplay>().RefreshDisplay();
            }
        }
        
        for (int i = 0; i < deadCheckers.childCount; i++)
        {
            deadCheckers.GetChild(i).GetComponent<CheckerDisplay>().RefreshDisplay();
        }
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
            int[] topLeftMoves = GenerateOffsetPromotedMoves(7, factor, checkerData, tileID);
            int[] topRightMoves = GenerateOffsetPromotedMoves(9, factor, checkerData, tileID);
            int[] bottomLeftMoves = GenerateOffsetPromotedMoves(7, -1*factor, checkerData, tileID);
            int[] bottomRightMoves = GenerateOffsetPromotedMoves(9, -1*factor, checkerData, tileID);

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
            return ret;
        }
        else //non promoted checker movement rules
        {
            int leftMove = GenerateOffsetUnpromotedMove(7, factor, checkerData, tileID);
            int rightMove = GenerateOffsetUnpromotedMove(9, factor, checkerData, tileID);
            int bottomLeftCapture = GenerateOffsetCapture(7, factor * -1, checkerData, tileID);
            int bottomRightCapture = GenerateOffsetCapture(9, factor * -1, checkerData, tileID);

            if (leftMove > -1)
            {
                size++;
            }
            if (rightMove > -1)
            {
                size++;
            }
            if (bottomLeftCapture > -1)
            {
                size++;
            }
            if (bottomRightCapture> -1)
            {
                size++;
            }

            ret = new int[size];

            int moveIndex = 0;
            if (leftMove > -1)
            {
                ret[moveIndex++] = leftMove;
            }
            if (rightMove > -1)
            {
                ret[moveIndex++] = rightMove;
            }
            if (bottomLeftCapture > -1)
            {
                ret[moveIndex++] = bottomLeftCapture;
            }
            if (bottomRightCapture > -1)
            {
                ret[moveIndex++] = bottomRightCapture;
            }
        }
        return ret;
    }

    int[] GenerateOffsetPromotedMoves(int offset, CheckerData checkerData, int tileID)
    {
        int[] ret = { };
        int index = 0;
        int prevMove = tileID;
        bool foundEnd = false;
        int foundID = -1;
        while (!foundEnd) //continue down diagnol until out of bounds or found a checker piece
        {
            foundID = GenerateOffsetUnpromotedMove(offset, factor, checkerData, prevMove);
            if (foundID != -1) //found a piece may continue while loop
            {
                if ((int)(foundID / 8) == (int)(prevMove / 8 + 2 * factor))
                {
                    foundEnd = true;
                }
                //ret[index] = foundID;
                prevMove = foundID;
                index++;
            }
            else
            {
                foundEnd = true;
            }
        }
        ret = new int[index];
        prevMove = tileID;
        for (int i = 0; i < index; i++)
        {
            ret[i] = GenerateOffsetUnpromotedMove(offset, factor, checkerData, prevMove);
            prevMove = foundID;
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
                CheckerData childData = tiles[id].transform.GetChild(0).GetComponent<CheckerData>();
                if (childData.color != checkerData.color && childData.dead != true)
                {
                    id += offset * factor;
                    if(id > 63 || id < 0)
                    {
                        id = -1;
                    }
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

    int GenerateOffsetCapture(int offset, int factor, CheckerData checkerData, int tileID)
    {
        int id = tileID + offset * factor;
        if (id > 63 || id < 0)
        {
            return -1;
        }

        if ((int)(id / 8) != (int)(tileID / 8 + 1 * factor))
        {
            return -1;
        }

        if (tiles[id].transform.childCount == 0 || tiles[id].transform.GetChild(0).GetComponent<CheckerData>().color == checkerData.color)
        {
            return -1;
        }

        id += offset * factor;

        if (id > 63 || id < 0)
        {
            return -1;
        }

        if ((int)(id / 8) != (int)(tileID / 8 + 2 * factor))
        {
            return -1;
        }

        if (tiles[id].transform.childCount != 0)
        {
            return -1;
        }

        return id;
    }

    //use for any diagnol capture (forward, backward, long, or short)
    //assumes the move input is a valid move
    //assumes only one checker can be captured at a time
    //also returns captured checker transform if there is a capture
    Transform IsMoveCapture(int startTileID, int endTileID)
    {
        CheckerColor movingCheckerColor;
        //checking color of moving checker
        if (tiles[startTileID].transform.childCount != 0)
        {
            movingCheckerColor = tiles[startTileID].transform.GetChild(0).GetComponent<CheckerData>().color;
        }
        else
        {
            return null; //there is no checker on the starting tile
        }

        int factor = 1;
        if (startTileID > endTileID) //move goes down the board
        {
            factor = -1;
        }

        bool captured = false;
        Transform capturedChecker = null;
        int curID = startTileID;
        if (startTileID % 8 > endTileID % 8) //move goes left
        {
            int movementfactor = 0;
            if (factor == 1)
            {
                movementfactor = 7;
            }
            else
            {
                movementfactor = -9;
            }
            curID += movementfactor;
            while (curID % 8 > endTileID % 8) //going down the diagnol
            {
                if (tiles[curID].transform.childCount > 0) //something is being blocking diagonal
                {
                    Transform checkerTransform = tiles[curID].transform.GetChild(0);
                    if (checkerTransform.GetComponent<CheckerData>().color != movingCheckerColor) //checker is opposite of starting color
                    {
                        if (captured == true) //attempting to capture a second piece, invalid move
                        {
                            return null;
                        }
                        capturedChecker = checkerTransform; //return pointer to checker that will be captured
                        captured = true;
                    }
                    else //checker is same as starting color, invalid move
                    {
                        return null;
                    }
                }
                curID += movementfactor;
            }
        }
        else //move goes right
        {
            int movementfactor = 0;
            if (factor == 1)
            {
                movementfactor = 9;
            }
            else
            {
                movementfactor = -7;
            }
            curID += movementfactor;
            while (curID % 8 < endTileID % 8) //going down the diagnol
            {
                if (tiles[curID].transform.childCount > 0) //something is being captured
                {
                    Transform checkerTransform = tiles[curID].transform.GetChild(0);
                    if (checkerTransform.GetComponent<CheckerData>().color != movingCheckerColor) //checker is opposite of starting color
                    {
                        if (captured == true) //attempting to capture a second piece, invalid move
                        {
                            return null;
                        }
                        capturedChecker = checkerTransform; //return pointer to checker that will be captured
                        captured = true;
                    }
                    else //checker is same as starting color, invalid move
                    {
                        return null;
                    }
                }
                curID += movementfactor;
            }
        }

        return capturedChecker;
    }
}
