using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//How to figure out if two lines are intersecting
public class LineLineIntersection : MonoBehaviour
{
    //The start end of each line
    public Transform L1_start;
    public Transform L1_end;
    public Transform L2_start;
    public Transform L2_end;

    //Just attach a line renderer to an empty game object
    public LineRenderer lineRenderer1;
    public LineRenderer lineRenderer2;

    void Start()
    {
        //Give the line renderer a material so we can change the color if they intersect
        lineRenderer1.material = new Material(Shader.Find("Unlit/Color"));
        lineRenderer2.material = new Material(Shader.Find("Unlit/Color"));
    }

    void Update()
    {
     
    }
 
    /// <summary>
    /// Check if two linear lines are interesecting in a 2d space
    /// </summary>
    /// <param name="startVector1">Position of startpoint of first line</param>
    /// <param name="endVector1">Position of endpoint of first line</param>
    /// <param name="startVector2">Position of startpoint of second line</param>
    /// <param name="endVector2">Position of endpoint of second line</param>
    /// <returns>The point of intersection in form of a vector</returns>
    Vector2 IsIntersecting(Vector2 startVector1, Vector2 endVector1, Vector2 startVector2, Vector2 endVector2)
    {
        bool isIntersecting = false;

        //3d -> 2d
        Vector2 l1_start = startVector1;
        Vector2 l1_end = endVector1;

        Vector2 l2_start = startVector2;
        Vector2 l2_end = endVector2;

        //Direction of the lines
        Vector2 l1_dir = (l1_end - l1_start).normalized;
        Vector2 l2_dir = (l2_end - l2_start).normalized;

        //If we know the direction we can get the normal vector to each line
        Vector2 l1_normal = new Vector2(-l1_dir.y, l1_dir.x);
        Vector2 l2_normal = new Vector2(-l2_dir.y, l2_dir.x);


        //Step 1: Rewrite the lines to a general form: Ax + By = k1 and Cx + Dy = k2
        //The normal vector is the A, B
        float A = l1_normal.x;
        float B = l1_normal.y;

        float C = l2_normal.x;
        float D = l2_normal.y;

        //To get k we just use one point on the line
        float k1 = (A * l1_start.x) + (B * l1_start.y);
        float k2 = (C * l2_start.x) + (D * l2_start.y);


        //Step 2: are the lines parallel? -> no solutions
        if (IsParallel(l1_normal, l2_normal))
        {
            Debug.Log("The lines are parallel so no solutions!");

            return Vector2.zero;
        }


        //Step 3: are the lines the same line? -> infinite amount of solutions
        //Pick one point on each line and test if the vector between the points is orthogonal to one of the normals
        if (IsOrthogonal(l1_start - l2_start, l1_normal))
        {
            Debug.Log("Same line so infinite amount of solutions!");

            //Return false anyway
            return Vector2.zero;
        }


        //Step 4: calculate the intersection point -> one solution
        float x_intersect = (D * k1 - B * k2) / (A * D - B * C);
        float y_intersect = (-C * k1 + A * k2) / (A * D - B * C);

        Vector2 intersectPoint = new Vector2(x_intersect, y_intersect);


        //Step 5: but we have line segments so we have to check if the intersection point is within the segment
        if (IsBetween(l1_start, l1_end, intersectPoint) && IsBetween(l2_start, l2_end, intersectPoint))
        {
            Debug.Log("We have an intersection point!");

            return intersectPoint;
        }

        return Vector2.zero;
    }

    //Are 2 vectors parallel?
    bool IsParallel(Vector2 v1, Vector2 v2)
    {
        //2 vectors are parallel if the angle between the vectors are 0 or 180 degrees
        if (Vector2.Angle(v1, v2) == 0f || Vector2.Angle(v1, v2) == 180f)
        {
            return true;
        }

        return false;
    }

    //Are 2 vectors orthogonal?
    bool IsOrthogonal(Vector2 v1, Vector2 v2)
    {
        //2 vectors are orthogonal if the dot product is 0
        //We have to check if close to 0 because of floating numbers
        if (Mathf.Abs(Vector2.Dot(v1, v2)) < 0.000001f)
        {
            return true;
        }

        return false;
    }

    //Is a point c between 2 other points a and b?
    bool IsBetween(Vector2 a, Vector2 b, Vector2 c)
    {
        bool isBetween = false;

        //Entire line segment
        Vector2 ab = b - a;
        //The intersection and the first point
        Vector2 ac = c - a;

        //Need to check 2 things: 
        //1. If the vectors are pointing in the same direction = if the dot product is positive
        //2. If the length of the vector between the intersection and the first point is smaller than the entire line
        if (Vector2.Dot(ab, ac) > 0f && ab.sqrMagnitude >= ac.sqrMagnitude)
        {
            isBetween = true;
        }

        return isBetween;
    }
}
