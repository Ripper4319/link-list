using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ColorSetter : MonoBehaviour
{
    public GameObject targetObject;
    private LinkedList colorList;
    private int currentIndex;
    public float detectionRange = 5;
    public playerMovement player;
    public NavMeshAgent agent;
    public Transform player1;
    public float Health = 3;
    private bool r = true;
    public GameObject Particalesystem;
    public bool Died = false;
    public NavDisabler navdis;

    [SerializeField] private string id;

    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    private void Start()
    {
        colorList = new LinkedList();
        colorList.Add(Color.red);
        colorList.Add(Color.green);
        colorList.Add(Color.blue);

        SetColor(0);

        player = GameObject.FindWithTag("Player").GetComponent<playerMovement>();
        agent = GetComponent<NavMeshAgent>();

        Died = false;
    }

    public void CycleColors()
    {
        if (r)
        {
            r = false;

            if (colorList.Count() == 0) return;

            if (currentIndex < colorList.Count() - 1)
            {
                currentIndex++;
                SetColor(currentIndex);
            }
            else
            {
                Debug.LogWarning("Buttocks, no more.");
            }

            StartCoroutine(UpdateCycleDelay());
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("Cycling Buttocks...");
            CycleColors();
        }

        if (navdis.on)
        {
            {
                float distanceToPlayer = Vector3.Distance(transform.position, player1.position);
                if (distanceToPlayer <= detectionRange)
                {
                    agent.destination = player.transform.position;


                }
            }
        }
       

        if (Health <= 0 && !Died)
        {

            Died = true;

            StartCoroutine(Death());

        }

    }

    public void LoadData(GameData data)
    {
        if (data.enemiesDefeated.TryGetValue(id, out Died) && Died)
        {
            Destroy(gameObject);  
            return;
        }

        if (data.enemyPositions.TryGetValue(id, out Vector3 savedPosition))
        {
            transform.position = savedPosition; 
        }
    }

    public void SaveData(GameData data)
    {
        data.enemiesDefeated[id] = Died;
        data.enemyPositions[id] = transform.position;
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
                Debug.LogWarning("Buttocks.");
            }
        }
        else
        {
            Debug.LogWarning("Buttocks");
        }
    }

   
    private IEnumerator UpdateCycleDelay()
    {
        yield return new WaitForSeconds(0.1f);
        r = true;
    }

    public IEnumerator DestroyTime()
    {
        if (!Died)
        {
            Health--;

            GameObject particales = Instantiate(Particalesystem, transform.position, transform.rotation);

            yield return new WaitForSeconds(2);
            Destroy(particales);
        }
        
    }

    private IEnumerator Death()
    {
        GameObject particales = Instantiate(Particalesystem, transform.position, transform.rotation);

        if (Health <= 0)
            Destroy(gameObject);

        yield return new WaitForSeconds(4);

        Destroy(particales);
    }


}

