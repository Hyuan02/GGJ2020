using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ButtonController : MonoBehaviour
{
    // Start is called before the first frame update
    private SpriteRenderer sr;
    public Sprite pressedImage;
    public Sprite defaultImage;

    public KeyCode keyToPress;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyToPress))
        {
            sr.sprite = pressedImage;
        }
        else if (Input.GetKeyUp(keyToPress))
        {
            sr.sprite = defaultImage;
        }
    }
}
