using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CheckerData : MonoBehaviour
{
    public int parentTileID;
    public CheckerColor color;
    // Start is called before the first frame update
    void Start()
    {

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
