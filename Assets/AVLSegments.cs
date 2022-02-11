using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AVLSegments : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
class SegmentAVL
{
    public class Node
    {
        public Segment data;
        public Node left;
        public Node right;
        public Node(Segment data)
        {
            this.data = data;
        }
    }
    Node root;
    public void Add(Segment data)
    {
        Node newItem = new Node(data);
        if (root == null)
        {
            root = newItem;
        }
        else
        {
            root = RecursiveInsert(root, newItem);
        }
    }
    private Node RecursiveInsert(Node current, Node n)
    {
        if (current == null)
        {
            current = n;
            return current;
        }
        else if (n.data < current.data)
        {
            current.left = RecursiveInsert(current.left, n);
            current = balance_tree(current);
        }
        else if (n.data > current.data)
        {
            current.right = RecursiveInsert(current.right, n);
            current = balance_tree(current);
        }
        return current;
    }
    private Node balance_tree(Node current)
    {
        int b_factor = balance_factor(current);
        if (b_factor > 1)
        {
            if (balance_factor(current.left) > 0)
            {
                current = RotateLL(current);
            }
            else
            {
                current = RotateLR(current);
            }
        }
        else if (b_factor < -1)
        {
            if (balance_factor(current.right) > 0)
            {
                current = RotateRL(current);
            }
            else
            {
                current = RotateRR(current);
            }
        }
        return current;
    }
    public void Delete(Segment target)
    {//and here
        root = Delete(root, target);
    }
    private Node Delete(Node current, Segment target)
    {
        Node parent;
        if (current == null)
        { return null; }
        else
        {
            //left subtree
            if (target < current.data)
            {
                current.left = Delete(current.left, target);
                if (balance_factor(current) == -2)//here
                {
                    if (balance_factor(current.right) <= 0)
                    {
                        current = RotateRR(current);
                    }
                    else
                    {
                        current = RotateRL(current);
                    }
                }
            }
            //right subtree
            else if (target > current.data)
            {
                current.right = Delete(current.right, target);
                if (balance_factor(current) == 2)
                {
                    if (balance_factor(current.left) >= 0)
                    {
                        current = RotateLL(current);
                    }
                    else
                    {
                        current = RotateLR(current);
                    }
                }
            }
            //if target is found
            else
            {
                if (current.right != null)
                {
                    //delete its inorder successor
                    parent = current.right;
                    while (parent.left != null)
                    {
                        parent = parent.left;
                    }
                    current.data = parent.data;
                    current.right = Delete(current.right, parent.data);
                    if (balance_factor(current) == 2)//rebalancing
                    {
                        if (balance_factor(current.left) >= 0)
                        {
                            current = RotateLL(current);
                        }
                        else { current = RotateLR(current); }
                    }
                }
                else
                {   //if current.left != null
                    return current.left;
                }
            }
        }
        return current;
    }
    public bool Find(Segment key)
    {
        return (Find(key, root) != null);
        /*
        if (Find(key, root).data == key)
        {
            Debug.Log("{" + key + "} was found!");
        }
        else
        {
            Debug.Log("Nothing found!");
        }*/
    }
    private Node Find(Segment target, Node current)
    {

        if (target < current.data)
        {
            if (target == current.data)
            {
                return current;
            }
            else
                return Find(target, current.left);
        }
        else
        {
            if (target == current.data)
            {
                return current;
            }
            else
                return Find(target, current.right);
        }

    }
    public void DisplayTree()
    {
        if (root == null)
        {
            Debug.Log("Tree is empty");
            return;
        }
        InOrderDisplayTree(root);
        Debug.Log('\n');
    }
    private void InOrderDisplayTree(Node current)
    {
        if (current != null)
        {
            InOrderDisplayTree(current.left);
            Debug.Log(current.data.ToString());
            InOrderDisplayTree(current.right);
        }
    }
    private int max(int l, int r)
    {
        return l > r ? l : r;
    }
    private int getHeight(Node current)
    {
        int height = 0;
        if (current != null)
        {
            int l = getHeight(current.left);
            int r = getHeight(current.right);
            int m = max(l, r);
            height = m + 1;
        }
        return height;
    }
    private int balance_factor(Node current)
    {
        int l = getHeight(current.left);
        int r = getHeight(current.right);
        int b_factor = l - r;
        return b_factor;
    }
    private Node RotateRR(Node parent)
    {
        Node pivot = parent.right;
        parent.right = pivot.left;
        pivot.left = parent;
        return pivot;
    }
    private Node RotateLL(Node parent)
    {
        Node pivot = parent.left;
        parent.left = pivot.right;
        pivot.right = parent;
        return pivot;
    }
    private Node RotateLR(Node parent)
    {
        Node pivot = parent.left;
        parent.left = RotateRR(pivot);
        return RotateLL(parent);
    }
    private Node RotateRL(Node parent)
    {
        Node pivot = parent.right;
        parent.right = RotateLL(pivot);
        return RotateRR(parent);
    }

}
/// <summary>
/// Implements a linesegment with Unity vectors as points
/// </summary>
class Segment : IComparable<Segment>
{
    /// <summary>
    /// Startpoint of the line segment in 2D space
    /// </summary>
    Vector2 startPoint;
    /// <summary>
    /// Endpoint of the line segment in 2D space
    /// </summary>
    Vector2 endPoint;

    /// <summary>
    /// Initializes the Segment 
    /// </summary>
    /// <param name="startpoint">Startpoint of the line segment in 2D space</param>
    /// <param name="endpoint">Endpoint of the line segment in 2D space</param>
    public Segment(Vector2 startpoint, Vector2 endpoint)
    {
        startPoint = startpoint;
        endPoint = endpoint;
    }
    public static bool operator <(Segment p, Segment q)
    {
            // p comes before q if y coordinate of p is bigger than y coordinate of q 
            return (p.startPoint.x <= q.startPoint.x);
    }
    public static bool operator >(Segment p, Segment q)
    {
      
            return (p.startPoint.x > q.startPoint.x);
        
    }
    public static bool operator ==(Segment p, Segment q)
    {
        return (p.startPoint.x == q.startPoint.x);
    }
    public static bool operator !=(Segment p, Segment q)
    {
        return (p.startPoint.x != q.startPoint.x);
    }
    public override string ToString()
    {
        return "(Startpoint: {" + startPoint.x + "}) | Endpoint: {" + endPoint.x + "})";
    }
    public int CompareTo(Segment other)
    {
        if (this == other) //If both are fancy (Or both are not fancy, return 0 as they are equal)
        {
            return 0;
        }
        else if (this < other) //Otherwise if A is fancy (And therefore B is not), then return -1
        {
            return -1;
        }
        else if (this > other) //Otherwise it must be that B is fancy (And A is not), so return 1
        {
            return 1;
        }
        else
        {
            throw new System.InvalidOperationException("Two segments have been compared. For some reason, they weren't equal or of higher or lower priority.");
        }
    }
}