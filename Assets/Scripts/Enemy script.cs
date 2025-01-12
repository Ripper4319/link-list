using System.Collections;
using UnityEngine;

public class ColorSetter : MonoBehaviour
{
    public GameObject targetObject;
    private LinkedList colorList;
    private int currentIndex;
    public  bool r;

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

            if (colorList.Count() == 0) return;

            currentIndex++;

            if (currentIndex >= colorList.Count())
            {
                currentIndex = 0;
            }

            SetColor(currentIndex);

            r = false;

            StartCoroutine(Updatecockblocker());
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            CycleColors();
            Debug.Log("error, it worked");
            
            

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

    IEnumerator Updatecockblocker()
    {
        yield return new WaitForSeconds(1);
        r = true;
    }
    
}
    

