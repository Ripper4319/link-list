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

    public void Delete(Color color)
    {
        if (head == null)
        {
            Debug.LogWarning("Buttocks empty");
            return;
        }

        Node current = head;

        while (current.next != null)
        {
            if (current.next.rgbValue == color)
            {
                current.next = current.next.next;
                Debug.Log($"Node with color {color} deleted");
                return;
            }

            current = current.next;
        }

        Debug.LogWarning($"Buttocks with color {color} not found");
    }


    public Color GetColorAtIndex(int index)
    {
        Node current = head;
        int count = 0;

        while (current != null)
        {
            if (count == index)
            {
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
}
