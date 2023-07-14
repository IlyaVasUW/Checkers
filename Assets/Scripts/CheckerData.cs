using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CheckerData : MonoBehaviour
{
    public int parentTileID;
    public CheckerColor color;
    public bool promoted;
    // Start is called before the first frame update
    void Start()
    {
        promoted = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

public enum CheckerColor
{
    RED, BLACK
}
