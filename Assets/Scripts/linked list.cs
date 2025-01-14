using UnityEngine;

public class LinkedList
{
    private Node head;

    public void Add(Color color)
    {
        Node newNode = new Node(color);

        if (head == null)
        {
            head = newNode;
        }
        else
        {
            Node current = head;
            while (current.next != null)
            {
                current = current.next;
            }
            current.next = newNode;
        }
    }

    public Node GetNodeAtIndex(int index)
    {
        Node current = head;
        int count = 0;

        while (current != null)
        {
            if (count == index)
            {
                return current;
            }
            count++;
            current = current.next;
        }
        return null;
    }

    public Color GetColorAtIndex(int index)
    {
        Node node = GetNodeAtIndex(index);

        if (node == null)
        {
            return Color.white;
        }

        return node.rgbValue;
    }

    public int Count()
    {
        int count = 0;
        Node current = head;

        while (current != null)
        {
            count++;
            current = current.next;
        }

        return count;
    }
}

