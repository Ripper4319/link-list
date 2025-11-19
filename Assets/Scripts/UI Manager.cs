using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public ManagerGen mangen;
    public Image damageImage;

    public Rigidbody rb;
    public Camera cam;
    public float baseFOV = 70f;
    public float maxFOV = 95f;
    public float velocityForMax = 20f;
    public float fovSmooth = 8f;

    public GameObject streakFX;
    public float streakSpeed = 15f;
    public float streakFade = 6f;

    float streakAlpha;
    Renderer streakRenderer;

    void Start()
    {
        streakRenderer = streakFX.GetComponent<Renderer>();
        Color c = streakRenderer.material.color;
        c.a = 0;
        streakRenderer.material.color = c;
    }

    void Update()
    {
        float tHealth = mangen.isinhealtharea ? 0f : 1f;
        float speedH = mangen.isinhealtharea ? 7f : 2f;
        Color d = damageImage.color;
        d.a = Mathf.Lerp(d.a, tHealth, Time.deltaTime * speedH);
        damageImage.color = d;

        float vel = rb.linearVelocity.magnitude;
        float t = Mathf.Clamp01(vel / velocityForMax);
        float targetFOV = Mathf.Lerp(baseFOV, maxFOV, t);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, Time.deltaTime * fovSmooth);

        float targetAlpha = vel > streakSpeed ? 1f : 0f;
        streakAlpha = Mathf.Lerp(streakAlpha, targetAlpha, Time.deltaTime * streakFade);

        Color sc = streakRenderer.material.color;
        sc.a = streakAlpha;
        streakRenderer.material.color = sc;
    }
}
