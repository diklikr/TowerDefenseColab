using UnityEngine;
using UnityEngine.UI;

public class CooldownUI : MonoBehaviour
{
    public AtaqueArea ataque;
    public Image relleno;      // la imagen oscura que se vacía

    void Update()
    {
        if (ataque == null || relleno == null) return;

        relleno.fillAmount = ataque.PorcentajeCooldown;
    }
}