using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardTileGenerator : MonoBehaviour
{
    [SerializeField] public GameObject Tile;
    [SerializeField] public GameObject Checker;
    public Color tileColor = new Color(255, 255, 255);
    public Color highlightColor = new Color(255, 255, 100);
    public int colorScale = 3;
    private GameController gameController;

    void Awake()
    {
        gameController = GetComponent<GameController>();

        gameController.tiles = new Dictionary<int, GameObject>();
        Color darkColor = new Color(tileColor.r / colorScale, tileColor.g / colorScale, tileColor.b / colorScale);
        Color checkerRed = new Color(180, 0, 0);
        Color checkerBlack = new Color(0, 0, 0);

        for(int i = 0; i < 8; i++)
        {
            for(int j = 0; j < 8; j++)
            {
                int tileID = i * 8 + j;

                bool isDarkSquare = (i + j) % 2 == 0;

                GameObject tile = CreateTile(i, j, isDarkSquare, tileID, darkColor);

                if ((tileID < 24 || tileID >= 40) && isDarkSquare)
                {
                    CreateChecker(tile, tileID, tileID < 24, checkerRed, checkerBlack);
                }

                gameController.tiles.Add(tileID, tile);
            }
        }
    }

    GameObject CreateTile(int i, int j, bool isDarkSquare, int tileID, Color darkColor)
    {
        Vector3 scale = transform.localScale / 8;
        float startX = transform.localScale.x * -0.5f - scale.x / 2;
        float startY = transform.localScale.y * -0.5f - scale.y / 2;

        float positionX = (startX + ((scale.x) * (j + 1)));
        float positionY = (startY + ((scale.y) * (i + 1)));
        float positionZ = -0.1f;

        GameObject tile = Instantiate(Tile, new Vector3(positionX, positionY, positionZ), Quaternion.identity, transform);
        tile.transform.localScale = scale / 8;
        SpriteRenderer tileRenderer = tile.GetComponent<SpriteRenderer>();
        TileController tileController = tile.GetComponent<TileController>();

        tileRenderer.color = isDarkSquare ?
            new Color(darkColor.r, darkColor.g, darkColor.b) : 
            new Color(tileColor.r, tileColor.g, tileColor.b);

        tileRenderer.sortingLayerName = "Tiles";

        tileController.id = tileID;
        tileController.gameController = gameController;
        tileController.color = isDarkSquare ? TileColor.Dark : TileColor.Light;
        tileController.baseColor = new Color(tileColor.r, tileColor.g, tileColor.b);
        tileController.highlightColor = new Color(highlightColor.r, highlightColor.g, highlightColor.b);
        tileController.colorScale = colorScale;

        tile.name = "Tile " + tileID;

        return tile;
    }

    void CreateChecker(GameObject tile, int tileID, bool isRed, Color checkerRed, Color checkerBlack)
    {
        GameObject checker = Instantiate(Checker,
                        new Vector3(tile.transform.position.x, tile.transform.position.y, tile.transform.position.z - 0.1f),
                        Quaternion.identity, tile.transform);

        checker.GetComponent<CheckerMovementController>().gameController = gameController;

        checker.transform.localScale *= 4;

        SpriteRenderer checkerRenderer = checker.GetComponent<SpriteRenderer>();
        CheckerData checkerData = checker.GetComponent<CheckerData>();

        checkerRenderer.color = isRed ? 
            new Color(checkerRed.r, checkerRed.g, checkerRed.b) : 
            new Color(checkerBlack.r, checkerBlack.g, checkerBlack.b);

        checkerRenderer.sortingLayerName = "Checkers";

        checkerData.parentTileID = tileID;
        checkerData.color = isRed ? CheckerColor.RED : CheckerColor.BLACK;

        checker.name = isRed ? "Checker Red" : "Checker Black";
    }
}
