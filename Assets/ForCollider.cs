using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForCollider : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject controller;
    private DrawLine dl;
    private SpriteRenderer renderer; 

    public string selectableTag = "Selectable";
    private Color selectedColor = Color.yellow;
    private Color defaultcolor = Color.white;
    private bool selected = false;
   
   
    void Start()
    {
        
        controller = GameObject.Find("Controller");
        dl = controller.GetComponent<DrawLine>();
      
        renderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        if ((Input.GetKey(KeyCode.LeftShift)) && !(Input.GetKey(KeyCode.LeftControl)))
        {
            dl.CreateLine();
        }
        else if (transform.CompareTag(selectableTag)){
            if (renderer != null)
            {
                if (selected)
                {
                    renderer.color = defaultcolor;
                    selected = false;
                    
                }
                else
                {
                    renderer.color = selectedColor;
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
                dl.players.Remove(this.gameObject);
                GameObject.Destroy(this.gameObject);
            }
        }
    }
}
