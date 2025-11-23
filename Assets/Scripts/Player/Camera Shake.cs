using UnityEngine;

public class CameraShake : MonoBehaviour
{
    Vector3 originalPos;
    float currentIntensity = 0f;
    float shakeDecay = 2.5f;  

    void Start()
    {
        originalPos = transform.localPosition;
    }

    void Update()
    {
        if (currentIntensity > 0f)
        {
            Vector3 shakeOffset = Random.insideUnitSphere * currentIntensity;
            transform.localPosition = originalPos + shakeOffset;

            currentIntensity = Mathf.MoveTowards(currentIntensity, 0f, shakeDecay * Time.deltaTime);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPos, Time.deltaTime * 10f);
        }
    }

    public void Shake(float intensity)
    {
        currentIntensity += intensity; 
    }
}


