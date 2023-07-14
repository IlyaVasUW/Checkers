using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardTileGenerator : MonoBehaviour
{
    [SerializeField] public GameObject Tile;
    public Color tileColor = new Color(255, 255, 255);
    public int colorScale = 4;
    private GameObject[] tiles;
    // Start is called before the first frame update
    void Start()
    {
        tiles = new GameObject[64];
        Color darkColor = new Color(tileColor.r / colorScale, tileColor.g / colorScale, tileColor.b / colorScale);
        for(int i = 0; i < 8; i++)
        {
            for(int j = 0; j < 8; j++)
            {
                Vector3 scale = transform.localScale / 8;
                float startX = transform.localScale.x * -0.5f - scale.x / 2;
                float startY = transform.localScale.y * -0.5f - scale.y / 2;

                float positionX = (startX + ((scale.x) * (i + 1)));
                float positionY = (startY + ((scale.y) * (j + 1)));
                float positionZ = -0.1f;

                GameObject tile = Instantiate(Tile, new Vector3(positionX, positionY, positionZ), Quaternion.identity, transform);
                tile.transform.SetParent(transform, false);
                tile.transform.localScale = scale / 8;

                tile.GetComponent<SpriteRenderer>().color = (i + j) % 2 == 0 ?
                    new Color(darkColor.r, darkColor.g, darkColor.b) : new Color(tileColor.r, tileColor.g, tileColor.b);
                tiles.Append(tile);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
