using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LG_PlayerSanity : MonoBehaviour
{
    [Header("--- Parámetros de Cordura ---")]
    [SerializeField] private float maxSanity = 100f;
    [SerializeField] private float currentSanity;

    [Header("--- Drenaje y Regeneración ---")]
    [Tooltip("Drenaje pasivo por segundo al explorar/estar fuera de la base")]
    [SerializeField] private float explorationDrainRate = 3f;
    [Tooltip("Regeneración por segundo al estar en la base/zona segura")]
    [SerializeField] private float safeZoneRegenRate = 8f;

    [Header("--- Estados ---")]
    public bool isInSafeZone = false;
    public bool isPenalized = false;

    [Header("--- Referencias de UI ---")]
    [SerializeField] private Slider sanitySlider;
    [SerializeField] private Image sanityFillImage;
    [SerializeField] private TextMeshProUGUI sanityText;
    [SerializeField] private Color normalColor = new Color(0.2f, 0.8f, 1f);
    [SerializeField] private Color criticalColor = new Color(0.9f, 0.1f, 0.1f);

    [Header("--- Referencias de Jugador ---")]
    [SerializeField] private PlayerMove playerMovement; // Para aplicar penalización de velocidad

    // Eventos por si quieres conectar SFX o shaders de distorsión
    public event Action<float> OnSanityChanged;
    public event Action OnSanityZero;
    public event Action OnSanityRestored;

    private void Awake()
    {
        currentSanity = maxSanity;
        if (playerMovement == null) playerMovement = GetComponent<PlayerMove>();
    }

    private void Start()
    {
        UpdateUI();
    }

    private void Update()
    {
        HandleSanityFlow();
    }

    private void HandleSanityFlow()
    {
        if (isInSafeZone)
        {
            // Regeneración en zona segura
            if (currentSanity < maxSanity)
            {
                currentSanity += safeZoneRegenRate * Time.deltaTime;
                currentSanity = Mathf.Min(currentSanity, maxSanity);

                if (isPenalized && currentSanity > (maxSanity * 0.25f))
                {
                    RemovePenalty();
                }
            }
        }
        else
        {
            // Drenaje por exploración continua
            if (currentSanity > 0)
            {
                currentSanity -= explorationDrainRate * Time.deltaTime;
                currentSanity = Mathf.Max(currentSanity, 0);

                if (currentSanity <= 0 && !isPenalized)
                {
                    ApplyPenalty();
                }
            }
        }

        UpdateUI();
        OnSanityChanged?.Invoke(currentSanity);
    }

    /// <summary>
    /// Llamar cuando un Glitch/Enemigo impacta al jugador
    /// </summary>
    public void TakeSanityDamage(float amount)
    {
        currentSanity = Mathf.Max(0, currentSanity - amount);
        UpdateUI();
        OnSanityChanged?.Invoke(currentSanity);

        if (currentSanity <= 0 && !isPenalized)
        {
            ApplyPenalty();
        }
    }

    /// <summary>
    /// Penalización extrema según GDD (Ralentización crítica / bloqueo)
    /// </summary>
    private void ApplyPenalty()
    {
        isPenalized = true;
        OnSanityZero?.Invoke();
        Debug.LogWarning("¡CORDURA EN 0! Penalización extrema activada.");

        // Si PlayerMove tiene control de velocidad, aquí lo limitas
        // Ejemplo: playerMovement.SetSpeedModifier(0.3f); 
    }

    private void RemovePenalty()
    {
        isPenalized = false;
        OnSanityRestored?.Invoke();
        Debug.Log("Cordura recuperada por encima del umbral.");

        // Restaurar velocidad: playerMovement.SetSpeedModifier(1f);
    }

    private void UpdateUI()
    {
        if (sanitySlider != null)
        {
            sanitySlider.maxValue = maxSanity;
            sanitySlider.value = currentSanity;
        }

        if (sanityFillImage != null)
        {
            float ratio = currentSanity / maxSanity;
            sanityFillImage.color = Color.Lerp(criticalColor, normalColor, ratio);
        }

        if (sanityText != null)
        {
            sanityText.text = $"{Mathf.CeilToInt(currentSanity)} / {maxSanity}";
        }
    }

    // Detección de Base/Pilar (Zona Segura)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SafeZone") || other.CompareTag("House"))
        {
            isInSafeZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SafeZone") || other.CompareTag("House"))
        {
            isInSafeZone = false;
        }
    }

}
