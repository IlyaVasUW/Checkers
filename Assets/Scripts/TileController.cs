using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    public int id;
    public GameController gameController;
    public TileColor color;
    public Color baseColor;
    public Color highlightColor;
    public int colorScale;

    SpriteRenderer spriteRenderer;
    bool highlighted;
    int highlightScale;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        highlighted = false;
        highlightScale = colorScale / 2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseUp()
    {
        Debug.Log("clicked tile");
        gameController.SelectTile(id);
        if (highlighted)
        {
            StopHighlight();
        } else
        {
            Highlight();
        }
    }

    public void Highlight()
    {
        if(color == TileColor.Light)
        {
            spriteRenderer.color = new Color(highlightColor.r, highlightColor.g, highlightColor.b);
        } else
        {
            spriteRenderer.color = new Color(
                highlightColor.r / highlightScale, highlightColor.g / highlightScale, highlightColor.b / highlightScale);
        }

        highlighted = true;
    }

    public void StopHighlight()
    {
        if(color == TileColor.Light)
        {
            spriteRenderer.color = new Color(baseColor.r, baseColor.g, baseColor.b);
        } else
        {
            spriteRenderer.color = new Color(baseColor.r / colorScale, baseColor.g / colorScale, baseColor.b / colorScale);
        }

        highlighted = false;
    }
}

public enum TileColor
{
    Dark, Light
}