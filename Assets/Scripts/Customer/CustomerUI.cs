using UnityEngine;

public class CustomerUI : MonoBehaviour
{
    [SerializeField] private Transform patienceBarFill;
    [SerializeField] private GameObject patienceBarBG;
    [SerializeField] private GameObject waitingIcon;
    [SerializeField] private SpriteRenderer patienceBarRenderer;

    private float maxPatience;
    private bool showingPatience = false;

    public void InitializePatience(float max)
    {
        maxPatience = max;
        ShowPatienceBar(true);
    }

    public void UpdatePatience(float currentPatience)
    {
        if (!showingPatience) return ;

        float ratio = Mathf.Clamp01(currentPatience / maxPatience); // Normalizamos el ratio entre 0 y 1
        Vector3 scale = patienceBarFill.localScale; // Ajustamos la escala en X según el ratio, manteniendo la altura constante
        scale.x = 0.8f * ratio;
        patienceBarFill.localScale = scale;

        // Cambiar color según el ratio: verde → amarillo → rojo
        if (ratio > 0.5f)
        {
            patienceBarRenderer.color = Color.green;
        }
        else if (ratio > 0.25f)
        {
            patienceBarRenderer.color = Color.yellow;
        }
        else
        {
            patienceBarRenderer.color = Color.red;
        }
    }

    public void ShowPatienceBar(bool show)
    {
        showingPatience = show;
        patienceBarBG.SetActive(show);
        patienceBarFill.gameObject.SetActive(show);
    }

    public void ShowWaitingIcon(bool show)
    {
        waitingIcon.SetActive(show);
    }

    public void HideAll()
    {
        ShowPatienceBar(false);
        ShowWaitingIcon(false);
    }
}