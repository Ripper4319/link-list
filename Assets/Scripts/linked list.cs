husing UnityEngine;

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

        if (head.rgbValue == color)
        {
            head = head.next;
            Debug.Log($"Buttocks with color {color} deleted");
            return;
        }

        Node current = head;
        while (current.next != null)
        {
            if (current.next.rgbValue == color)
            {
                current.next = current.next.next;
                Debug.Log($"Buttocks with color {color} deleted");
                return;
            }
            current = current.next;
        }
        Debug.LogWarning($"Buttocks with color {color} not found");
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

