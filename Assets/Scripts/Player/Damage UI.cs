using UnityEngine;
using UnityEngine.UI;

public class DamageUI : MonoBehaviour
{
    public ManagerGen mangen;
    public Image damageImage;
    public float fadeInSpeed = 2f;
    public float fadeOutSpeed = 7f;

    private void Update()
    {
        if (mangen != null)
        {
            bool isinhealtharea = mangen.isinhealtharea;
            float targetAlpha = isinhealtharea ? 0f : 1f;
            float speed = isinhealtharea ? fadeOutSpeed : fadeInSpeed;

            Color currentColor = damageImage.color;
            currentColor.a = Mathf.Lerp(currentColor.a, targetAlpha, Time.deltaTime * speed);
            damageImage.color = currentColor;
        }
    }
}