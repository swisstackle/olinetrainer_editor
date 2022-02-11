using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IntersectionAlgorithm : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<Vector2[]> test1 = new List<Vector2[]>();
        Vector2 vec = Vector2.one;
        Vector2 vec2 = new Vector2(0,2);
        Vector2[] toAdd = new Vector2[2];
        toAdd[0] = vec;
        toAdd[1] = vec2;
        test1.Add(toAdd);

        List<Vector2[]> test2 = new List<Vector2[]>();
        Vector2 vec3 = Vector2.zero;
        Vector2 vec4 = Vector2.one;
        Vector2[] toAdd2 = new Vector2[2];
        toAdd2[0] = vec3;
        toAdd2[1] = vec4;

        test2.Add(toAdd2);



        IntersectionSolver solver = new IntersectionSolver(test1, test2);
        solver.Print();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class IntersectionSolver
{
    private PriorityQueue<Segment> segments;
    private AVL events;

    public IntersectionSolver(List<Vector2[]> _olineLines, List<Vector2[]> _dLineLines)
    {
        segments = new PriorityQueue<Segment>();

        foreach(Vector2[] line in _olineLines)
        {
            int len = line.Length;
            Vector2 startPoint = line[0];
            
            Vector2 endPoint = line[len - 1];
            Segment toAdd = new Segment(startPoint, endPoint);
            this.segments.Enqueue(toAdd);
           
        }

        foreach (Vector2[] line in _dLineLines)
        {
            int len = line.Length;
            Vector2 startPoint = line[0];

            Vector2 endPoint = line[len - 1];
            Segment toAdd = new Segment(startPoint, endPoint);
            this.segments.Enqueue(toAdd);

        }

    }

    public void Solve()
    {
        foreach(Segment segment in this.segments)
        {

        }
    }

    public void Print()
    {
        foreach (Segment segment in this.segments)
        {
            Debug.Log(segment.ToString()+"\n");
        }
    }



}
