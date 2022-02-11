using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableLine : MonoBehaviour
{

    [SerializeField] private GameObject controller;
    private DrawLine dl;
    private LineRenderer renderer;

    [SerializeField] private string selectableTag = "Selectable";
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

    /*
    * <summary>
    * So that we can select a line and delete it.
    * </summary>
    */
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
                dl.defensiveLines.Remove(this.gameObject);
                dl.olineLines.Remove(this.gameObject); // if it was an oline line, the oline line gets removed
                GameObject.Destroy(this.gameObject);
            }
        }
    }
}
