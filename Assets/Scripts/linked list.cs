using UnityEngine;

public class LinkedList
{
    private Node head;
    private Node tail;
    private bool isEnded = false;

    public void Add(Color color)
    {
        Node newNode = new Node(color);

        if (head == null)
        {
            head = newNode;
            tail = newNode;
        }
        else
        {
            Node current = tail;
            current.next = newNode;
            tail = newNode;
        }
    }

    public Color GetColorAtIndex(int index, out bool isAtEnd)
    {
        Node current = head;
        int count = 0;
        isAtEnd = false;

        while (current != null)
        {
            if (count == index)
            {
                if (current == tail && isEnded)
                {
                    isAtEnd = true;
                }
                return current.rgbValue;
            }
            count++;
            current = current.next;
        }

        Debug.LogWarning("buttocks no color");
        return Color.white;
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

    public void SetEnd(bool end)
    {
        isEnded = end;
    }

    public bool IsEnded()
    {
        return isEnded;
    }
}

