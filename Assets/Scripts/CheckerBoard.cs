using System.Collections.Generic;

using Minimax;

class Checker
{
    public CheckerColor Color;
    public bool Promoted;
    public bool Dead;
}

class CheckerTile
{
#nullable enable
    public Checker? Checker = null;
#nullable disable
    public int Index;

    public CheckerTile(int index)
    {
        Index = index;
    }

    public CheckerTile(Checker checker, int index)
    {
        this.Checker = checker;
        Index = index;
    }
}

class CheckerStep : MinimaxStep
{
    public int StartIndex;
    public int EndIndex;
    public bool IsCapture;
    public CheckerColor PlayerColor;

    public CheckerStep(int startIndex, int endIndex, bool isCapture, CheckerColor playerColor)
    {
        StartIndex = startIndex;
        EndIndex = endIndex;
        IsCapture = isCapture;
        PlayerColor = playerColor;
    }
}

class CheckerBoard : MinimaxNode
{
    CheckerTile[] tiles = new CheckerTile[64];
    public CheckerColor CurrentPlayer = CheckerColor.BLACK;
    public bool prevMoveCapture = false;
    public int prevMoveEndIndex = -1;
    public CheckerBoard()
    {
        // Initialize the board
    }

    public CheckerBoard(CheckerTile[] tiles, CheckerColor currentPlayer)
    {
        this.tiles = tiles;
        this.CurrentPlayer = currentPlayer;
    }

    public override List<MinimaxNode> GetChildren()
    {

        var children = new List<MinimaxNode>();

        if (prevMoveCapture && tiles[prevMoveEndIndex].Checker.Color == CurrentPlayer)
        {
            var tile = tiles[prevMoveEndIndex];

            int[] validMoves = GenerateValidMoves(tile.Index);

            if (validMoves.Length != 0)
            {
                foreach (int move in validMoves)
                {
                    var capture = IsMoveCapture(tile.Index, move);

                    CheckerTile[] newTiles = new CheckerTile[tiles.Length];
                    for (int i = 0; i < tiles.Length; i++)
                    {
                        newTiles[i] = new CheckerTile(tiles[i].Checker, tiles[i].Index);
                    }

                    if (capture != null)
                    {
                        newTiles[capture.Index].Checker = null;
                    }

                    newTiles[tile.Index].Checker = null;
                    newTiles[move].Checker = new Checker
                    {
                        Color = tile.Checker.Color,
                        Promoted = tile.Checker.Promoted,
                        Dead = false
                    };

                    if (move / 8 == 7 || move / 8 == 0)
                    {
                        newTiles[move].Checker.Promoted = true;
                    }

                    CheckerColor nextPlayer = capture != null
                        ? CurrentPlayer
                        : (CurrentPlayer == CheckerColor.BLACK
                            ? CheckerColor.RED
                            : CheckerColor.BLACK);

                    var newChild = new CheckerBoard(newTiles, nextPlayer)
                    {
                        prevMoveCapture = capture != null,
                        prevMoveEndIndex = move
                    };

                    if (capture != null && newChild.GenerateValidMoves(move).Length == 0 && newChild.GetNumBlackCheckers() > 0 && newChild.GetNumRedCheckers() > 0)
                    {
                        newChild.CurrentPlayer = newChild.CurrentPlayer == CheckerColor.RED ? CheckerColor.BLACK : CheckerColor.RED;
                    }

                    children.Add(newChild);
                }
                return children;
            }
        }

        foreach (var tile in tiles)
        {
            if (tile.Checker == null || tile.Checker.Color != CurrentPlayer)
            {
                continue;
            }

            int[] validMoves = GenerateValidMoves(tile.Index);
            foreach (int move in validMoves)
            {
                var capture = IsMoveCapture(tile.Index, move);

                CheckerTile[] newTiles = new CheckerTile[tiles.Length];
                for (int i = 0; i < tiles.Length; i++)
                {
                    newTiles[i] = new CheckerTile(tiles[i].Checker, tiles[i].Index);
                }

                if (capture != null)
                {
                    newTiles[capture.Index].Checker = null;
                }

                newTiles[tile.Index].Checker = null;
                newTiles[move].Checker = new Checker
                {
                    Color = tile.Checker.Color,
                    Promoted = tile.Checker.Promoted,
                    Dead = false
                };

                if (move / 8 == 7 || move / 8 == 0)
                {
                    newTiles[move].Checker.Promoted = true;
                }

                CheckerColor nextPlayer = capture != null 
                    ? CurrentPlayer 
                    : (CurrentPlayer == CheckerColor.BLACK 
                        ? CheckerColor.RED 
                        : CheckerColor.BLACK);
                var newChild = new CheckerBoard(newTiles, nextPlayer)
                {
                    prevMoveCapture = capture != null,
                    prevMoveEndIndex = move
                };

                if (capture != null && newChild.GenerateValidMoves(move).Length == 0 && newChild.GetNumBlackCheckers() > 0 && newChild.GetNumRedCheckers() > 0)
                {
                    newChild.CurrentPlayer = newChild.CurrentPlayer == CheckerColor.RED ? CheckerColor.BLACK : CheckerColor.RED;
                }

                children.Add(newChild);
            }
        }

        return children;
    }

    public override MinimaxStep GetPathToChild(MinimaxNode child)
    {
        CheckerBoard childBoard = (CheckerBoard)child;

        int startIndex = -1;
        int endIndex = -1;
        bool isCapture = 
            GetNumBlackCheckers() != childBoard.GetNumBlackCheckers()
            || GetNumRedCheckers() != childBoard.GetNumRedCheckers();

        for (int i = 0; i < tiles.Length; i++)
        {
            if (tiles[i].Checker != null && childBoard.tiles[i].Checker == null)
            {
                if (tiles[i].Checker.Color != CurrentPlayer)
                {
                    continue;
                } else
                {
                    startIndex = i;
                }
            }

            if (tiles[i].Checker == null && childBoard.tiles[i].Checker != null)
            {
                endIndex = i;
            }
        }

        return new CheckerStep(startIndex, endIndex, isCapture, CurrentPlayer);
    }

    public int GetNumBlackCheckers()
    {
        int numBlackCheckers = 0;
        foreach (CheckerTile tile in tiles)
        {
            if (
                tile.Checker != null
                && tile.Checker.Color == CheckerColor.BLACK
                && !tile.Checker.Dead
                )
            {
                numBlackCheckers++;
            }
        }
        return numBlackCheckers;
    }

    public int GetNumRedCheckers()
    {
        int numRedCheckers = 0;
        foreach (CheckerTile tile in tiles)
        {
            if (
                tile.Checker != null
                && tile.Checker.Color == CheckerColor.RED
                && !tile.Checker.Dead
                )
            {
                numRedCheckers++;
            }
        }
        return numRedCheckers;
    }

    public override double GetScore()
    {
        return GetNumBlackCheckers() - GetNumRedCheckers();
    }

    int[] GenerateValidMoves(int tileID)
    {
        var checkerData = tiles[tileID].Checker;
        var ret = new int[0];

        if (checkerData == null)
        {
            return new int[0];
        }

        int size = 0;
        int factor = checkerData.Color == CheckerColor.RED ? -1 : 1;

        if (checkerData.Promoted)
        {
            int[] topLeftMoves = GenerateOffsetPromotedMoves(7, factor, checkerData, tileID);
            int[] topRightMoves = GenerateOffsetPromotedMoves(9, factor, checkerData, tileID);
            int[] bottomLeftMoves = GenerateOffsetPromotedMoves(7, -1 * factor, checkerData, tileID);
            int[] bottomRightMoves = GenerateOffsetPromotedMoves(9, -1 * factor, checkerData, tileID);

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
        else //non Promoted Checker movement rules
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
            if (bottomRightCapture > -1)
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

    int[] GenerateOffsetPromotedMoves(int offset, int factor, Checker checkerData, int tileID)
    {
        int[] ret = { };
        int retLength = 0;
        int prevMove = tileID;
        bool foundEnd = false;
        int foundID = -1;
        while (!foundEnd) //continue down diagnol until out of bounds or found a Checker piece
        {
            foundID = GenerateOffsetUnpromotedMove(offset, factor, checkerData, prevMove);
            if (foundID != -1) //found a piece may continue while loop
            {
                if ((int)(foundID / 8) == (int)(prevMove / 8 + 2 * factor))
                {
                    foundEnd = true;
                }
                //ret[retLength] = foundID;
                prevMove = foundID;
                retLength++;
            }
            else
            {
                foundEnd = true;
            }
        }
        ret = new int[retLength];
        prevMove = tileID;
        for (int i = 0; i < retLength; i++)
        {
            ret[i] = GenerateOffsetUnpromotedMove(offset, factor, checkerData, prevMove);
            prevMove = ret[i];
        }
        return ret;
    }

    int GenerateOffsetUnpromotedMove(int offset, int factor, Checker checkerData, int tileID)
    {
        int id = tileID + offset * factor;
        if (id > 63 || id < 0)
        {
            return -1;
        }

        if ((int)(id / 8) == (int)(tileID / 8 + 1 * factor))
        {
            if (tiles[id].Checker != null)
            {
                Checker childData = tiles[id].Checker;
                if (childData.Color != checkerData.Color && childData.Dead != true)
                {
                    id += offset * factor;
                    if (id > 63 || id < 0)
                    {
                        id = -1;
                    }
                    if ((int)(id / 8) != (int)(tileID / 8 + 2 * factor))
                    {
                        id = -1;
                    }
                }
                else
                {
                    id = -1;
                }

            }
            if (id > -1 && (id <= 63 && id >= 0) && tiles[id].Checker != null)
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

    int GenerateOffsetCapture(int offset, int factor, Checker checkerData, int tileID)
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

        if (tiles[id].Checker == null || tiles[id].Checker.Color == checkerData.Color)
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

        if (tiles[id].Checker != null)
        {
            return -1;
        }

        return id;
    }

    //use for any diagonal capture (forward, backward, long, or short)
    //assumes the move input is a valid move
    //assumes only one Checker can be captured at a time
    //also returns captured Checker transform if there is a capture
    CheckerTile IsMoveCapture(int startTileID, int endTileID)
    {
        CheckerColor movingCheckerColor;
        //checking Color of moving Checker
        if (tiles[startTileID].Checker != null)
        {
            movingCheckerColor = tiles[startTileID].Checker.Color;
        }
        else
        {
            return null; //there is no Checker on the starting tile
        }

        int factor = 1;
        if (startTileID > endTileID) //move goes down the board
        {
            factor = -1;
        }

        bool captured = false;
        CheckerTile capturedChecker = null;
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
                if (tiles[curID].Checker != null) //something is being blocking diagonal
                {
                    Checker checker = tiles[curID].Checker;
                    if (checker.Color != movingCheckerColor) //Checker is opposite of starting Color
                    {
                        if (captured == true) //attempting to capture a second piece, invalid move
                        {
                            return null;
                        }
                        capturedChecker = new CheckerTile(checker, curID); //return pointer to Checker that will be captured
                        captured = true;
                    }
                    else //Checker is same as starting Color, invalid move
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
                if (tiles[curID].Checker != null) //something is being captured
                {
                    Checker checker = tiles[curID].Checker;
                    if (checker.Color != movingCheckerColor) //Checker is opposite of starting Color
                    {
                        if (captured == true) //attempting to capture a second piece, invalid move
                        {
                            return null;
                        }
                        capturedChecker = new CheckerTile(checker, curID); ; //return pointer to Checker that will be captured
                        captured = true;
                    }
                    else //Checker is same as starting Color, invalid move
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