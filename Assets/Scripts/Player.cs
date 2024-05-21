using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int shapeID;

    private void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = GameplayManager.instance.shapes[shapeID];
    }
}
