using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckerDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Sprite promoteSprite;
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
        int alpha = data.dead ? 0 : 255;
        if (data.color == CheckerColor.RED)
        {
            spriteRenderer.color = new Color(
                CheckerData.redColor.r,
                CheckerData.redColor.g,
                CheckerData.redColor.b,
                alpha
                );
        }
        if (data.color == CheckerColor.BLACK)
        {
            spriteRenderer.color = new Color(
                CheckerData.blackColor.r,
                CheckerData.blackColor.g,
                CheckerData.blackColor.b,
                alpha
                );
        }

    }

    public void UpdatePromoteSprite()
    {
        spriteRenderer.sprite = promoteSprite;
    }
}
