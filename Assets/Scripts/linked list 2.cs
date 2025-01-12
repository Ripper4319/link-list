using UnityEngine;

public class Node
{
    public Color rgbValue;
    public Node next;

    public Node(Color color)
    {
        rgbValue = color;
        next = null;
    }
}

