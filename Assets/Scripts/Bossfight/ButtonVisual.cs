using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonVisual : MonoBehaviour
{
    public Sprite idle;
    public Sprite pressed;
    public SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = idle;
    }

    // Update is called once per frame
    void OnMouseDown()
    {
        spriteRenderer.sprite = pressed;
    }

    void OnMouseUp()
    {
        spriteRenderer.sprite = idle;
    }
}
