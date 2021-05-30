using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableLine : MonoBehaviour
{

    public GameObject controller;
    private DrawLine dl;
    private LineRenderer renderer;

    public string selectableTag = "Selectable";
    private Color selectedColor = Color.yellow;
    private Color defaultcolor = Color.black;
    private bool selected = false;

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("Controller");
        dl = controller.GetComponent<DrawLine>();

        renderer = GetComponent<LineRenderer>();
    }

    private void OnMouseDown()
    {
        if (transform.CompareTag(selectableTag))
        {
            if (renderer != null)
            {
                if (selected)
                {
                    renderer.material.color = defaultcolor;
                    selected = false;

                }
                else
                {
                    renderer.material.color = selectedColor;
                    selected = true;
                }

            }
        }



    }

    // Update is called once per frame
    void Update()
    {
        if (selected)
        {
            if (Input.GetKey(KeyCode.Delete))
            {
                dl.lines.Remove(this.gameObject);
                GameObject.Destroy(this.gameObject);
            }
        }
    }
}
