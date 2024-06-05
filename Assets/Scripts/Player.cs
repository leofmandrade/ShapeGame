// ----------------------------------------------------------------------------------------------------------------------------
// DISCLAIMER
// Algumas partes deste código foram inspiradas no vídeo(https://www.youtube.com/watch?v=3i3HUkvRz9I) de GameGrind. Agradecimentos ao autor por compartilhar conhecimentos valiosos que ajudaram no desenvolvimento deste jogo.
// ----------------------------------------------------------------------------------------------------------------------------

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
