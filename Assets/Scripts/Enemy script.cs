using UnityEngine;

public class ColorSetter : MonoBehaviour
{
    public GameObject targetObject;
    private LinkedList colorList;
    private int currectIndex = 0;

    private void Start()
    {
        colorList = new LinkedList();
        colorList.Add(Color.red);
        colorList.Add(Color.green);
        colorList.Add(Color.blue);
        SetColor(0);
    }

    public void Update()
    {
        Input.GetKeyUp(KeyCode.W);
        {
            CycleColors();


        }
    }

    public void SetColor(int index)
    {
        if (targetObject != null)
        {
            Color colorToSet = colorList.GetColorAtIndex(index);
            Renderer renderer = targetObject.GetComponent<Renderer>();

            if (renderer != null)
            {
                renderer.material.color = colorToSet;
            }
            else
            {
                Debug.LogWarning("Buttocks");
            }
        }
        else
        {
            Debug.LogWarning("Buttocks2");
        }
    }

    public void CycleColors()
    {
        int count = colorList.Count();
        for (int i = 0; i < count; i++)
        {
            SetColor(i);
        }
    }
}
