using System.Collections;
using UnityEngine;

public class ColorSetter : MonoBehaviour
{
    public GameObject targetObject;
    private LinkedList colorList;
    private int currentIndex;
    public bool r = true;

    private void Start()
    {
        colorList = new LinkedList();
        colorList.Add(Color.red);
        colorList.Add(Color.green);
        colorList.Add(Color.blue);

        SetColor(0);
    }

    public void CycleColors()
    {
        if (r)
        {
            r = false;

            if (colorList.Count() == 0) return;

            currentIndex++;
            bool isAtEnd = false;

            Color colorToSet = colorList.GetColorAtIndex(currentIndex, out isAtEnd);

            if (isAtEnd)
            {
                Debug.Log("End of linked list reached!");
                currentIndex--;
                StartCoroutine(Updatecockblocker());
                return;
            }

            if (currentIndex >= colorList.Count() && !colorList.IsEnded())
            {
                currentIndex = 0;
            }

            SetColor(currentIndex);
            StartCoroutine(Updatecockblocker());
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("Cycling colors...");
            CycleColors();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            bool currentEndState = colorList.IsEnded();
            colorList.SetEnd(!currentEndState);
            Debug.Log("List end toggled to: " + (!currentEndState));
        }
    }

    public void SetColor(int index)
    {
        if (targetObject != null)
        {
            Color colorToSet = colorList.GetColorAtIndex(index, out _);
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

    IEnumerator Updatecockblocker()
    {
        yield return new WaitForSeconds(0.1f);
        r = true;
    }
}


