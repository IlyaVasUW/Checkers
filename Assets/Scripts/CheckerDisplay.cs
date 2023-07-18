using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckerDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Sprite promotePipeSprite;
    [SerializeField] Sprite promotePokeSprite;
    int pipeScale = 30;
    int pokeScale = 20;
    CheckerData data;
    SpriteRenderer spriteRenderer;
    void Start()
    {
        data = GetComponent<CheckerData>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RefreshDisplay()
    {
        float red = data.color == CheckerColor.RED ? CheckerData.redColor.r : CheckerData.blackColor.r;
        float green = 0;
        float blue = 0;

        if(data.promoted)
        {
            red = 255;
            green = 255;
            blue = 255;
        }

        int alpha = data.dead ? 0 : 255;
        spriteRenderer.color = new Color(
                red,
                green,
                blue,
                alpha
                );
    }

    public void UpdatePromoteSprite()
    {
        GetComponent<CircleCollider2D>().radius = 0.1f * (CheckerColor.BLACK == data.color ? pipeScale : pokeScale);
        spriteRenderer.sprite = data.color == CheckerColor.BLACK ? promotePipeSprite : promotePokeSprite;
        transform.localScale /= data.color == CheckerColor.BLACK ? pipeScale : pokeScale;
        spriteRenderer.color = new Color(
                255,
                255,
                255, 
                255);
    }
}
