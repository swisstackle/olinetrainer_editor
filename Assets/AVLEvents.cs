using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AVLEvents : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
        //Testing Insert. Case p.y==q.y and p.x > q.x
        Event a = new Event(new Vector2(1, 0), new Vector2(0, 1));
        Event b = new Event(new Vector2(2, 0), new Vector2(0, 1));

        Event c = new Event(new Vector2(1, 0), new Vector2(0, 3));
        Event d = new Event(new Vector2(1, 0), new Vector2(0, 1));

        Event e = new Event(new Vector2(4, 0), new Vector2(0, 1));
        Event f = new Event(new Vector2(1, 0), new Vector2(0, 1));

        Event g = new Event(new Vector2(1, 0), new Vector2(0, 1));
        Event h = new Event(new Vector2(1, 0), new Vector2(0, 5));

        Event i = new Event(new Vector2(5, 0), new Vector2(0, 1));

        AVL tree = new AVL();
        tree.Add(a);
        tree.Add(b);
        tree.Add(c);
        tree.Add(d);
        tree.Add(e);
        tree.Add(f);
        tree.Add(g);
        tree.Add(h);
        tree.Add(i);

        tree.Delete(b);


        tree.DisplayTree();
        Event m = new Event(new Vector2(8, 0), new Vector2(0, 1));
        Debug.Log(tree.Find(m));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

class AVL
{
    class Node
    {
        public Event data;
        public Node left;
        public Node right;
        public Node(Event data)
        {
            this.data = data;
        }
    }
    Node root;
    public void Add(Event data)
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
    public void Delete(Event target)
    {//and here
        root = Delete(root, target);
    }
    private Node Delete(Node current, Event target)
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
    public bool Find(Event key)
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
    private Node Find(Event target, Node current)
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
/// Implements an Event with Unity vectors as points
/// </summary>
class Event
{
    /// <summary>
    /// Positon of event
    /// </summary>
    Vector2 x, y;
    //int index;

    //bool isLeft;
    /// <summary>
    /// Initializes event
    /// </summary>
    /// <param name="_x">The x coordinate of the event</param>
    /// <param name="_y">The y coordinate of the event</param>
    public Event(Vector2 _x, Vector2 _y)
    {
        x = _x;
        y = _y;
    }

    public static bool operator <(Event p, Event q)
    {
        if(p.y.y == q.y.y)
        {
            return (p.x.x < q.x.x);
        }
        else
        {
            // p comes before q if y coordinate of p is bigger than y coordinate of q 
            return (p.y.y > q.y.y);
        }
       
    }
    public static bool operator >(Event p, Event q)
    {
        if (p.y.y == q.y.y)
        {
            return (p.x.x > q.x.x);
        }
        else
        {
            // p comes after q if y coordinate of p is smaller than y coordinate of q 
            return (p.y.y < q.y.y);
        }
    }
    public static bool operator ==(Event p, Event q)
    {
        return ((p.y.y == q.y.y) && (p.x.x == q.x.x));
    }
    public static bool operator !=(Event p, Event q)
    {
        return ((p.y.y != q.y.y) || (p.x.x != q.x.x));
    }
    public override string ToString()
    {
        return "(Y coordinate: {" + y + "}) | X coordinate: {" + x + "})";
    }
}

