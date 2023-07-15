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
        clearAndUnhighlightTiles();

        if (selectedID == tileID)
        {
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
        bool tileID_is_valid_move = false;
        if (selectedID != -1) // prev clicked tile has a checker piece
        {
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
                checker.SetParent(tiles[tileID].transform, false);
                checker.transform.position = tiles[tileID].transform.position;
                checker.GetComponent<CheckerData>().parentTileID = tileID;
                if (tileID > 55 && tileID < 64 && checker.GetComponent<CheckerData>().promoted == false)
                {
                    checker.GetComponent<CheckerData>().promoted = true;
                }
                clearAndUnhighlightTiles();
                selectedID = -1;
            }
        }
        return;
    }

    void clearAndUnhighlightTiles()
    {
        foreach (GameObject tile in highlightedTiles)
        {
            tile.GetComponent<TileController>().StopHighlight();
        }
        highlightedTiles = new GameObject[0];
    }

    int[] GenerateValidMoves(int tileID)
    {
        int[] ret = { };
        Transform checkerTransform = tiles[tileID].transform.GetChild(0);
        CheckerData checkerData = checkerTransform.GetComponent<CheckerData>();

        if(checkerData.promoted)
        {
        } else
        {
            int available = 0;
            

            int factor = checkerData.color == CheckerColor.RED ? 1 : -1;

            int id0 = tileID + 7 * factor;
            if(id0 > 63 || id0 < 0)
            {
                return ret;
            }

            if ((int)(id0 / 8) == (int)(tileID / 8 + 1*factor))
            {
                if (tiles[id0].transform.childCount > 0)
                {
                    if (tiles[id0].transform.GetChild(0).GetComponent<CheckerData>().color != checkerData.color)
                    {
                        id0 += 7 * factor;
                    }
                    else
                    {
                        id0 = -1;
                    }

                }
                if (id0 > -1 && tiles[id0].transform.childCount > 0)
                {
                    id0 = -1;
                }

                if (id0 != -1)
                {
                    available++;
                }
            } else
            {
                id0 = -1;
            }

            int id1 = tileID + 9 * factor;
            if (id0 > 63 || id0 < 0)
            {
                id1 = -1;
            }

            if (id1 > 0 && (int)(id1 / 8) == (int)(tileID / 8 + 1*factor))
            {
                if (tiles[id1].transform.childCount > 0)
                {
                    if (tiles[id1].transform.GetChild(0).GetComponent<CheckerData>().color != checkerData.color)
                    {
                        id1 += 9 * factor;
                    }
                    else
                    {
                        id1 = -1;
                    }
                }

                if (id1 > -1 && tiles[id1].transform.childCount > 0)
                {
                    id1 = -1;
                }

                if (id1 != -1)
                {
                    available++;
                }
            } else
            {
                id1 = -1;
            }

            int relID = 0;
            ret = new int[available];
            if(id0 != -1)
            {
                ret[relID] = id0;
                relID++;
            }

            if(id1 != -1)
            {
                ret[relID] = id1;
            }
        }

        return ret;
    }
}
