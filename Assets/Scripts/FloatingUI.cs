using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingUI : MonoBehaviour
{
    Transform cam;
    Transform unit;
    Transform canvas;

    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        unit = transform.parent;
        canvas = GameObject.FindObjectOfType<Canvas>().transform;
        transform.SetParent(canvas);
        transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
        transform.position = unit.position + offset;
    }
}
