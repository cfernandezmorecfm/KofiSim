using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SummaryPanelUI : MonoBehaviour
{
    // Singleton para acceder fácilmente al panel de sumario desde otras partes del código
    public static SummaryPanelUI InstanceID { get; private set; }

    [Header("Elementos de la UI del panel de sumario")]
    [SerializeField] private GameObject panelRoot;
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private TextMeshProUGUI incomeText;
    [SerializeField] private TextMeshProUGUI salaryText;
    [SerializeField] private TextMeshProUGUI balanceText;
    [SerializeField] private Button continueButton;

    private System.Action<DayPhaseChangedEvent> dayPhaseChangedHandler;

    private void Awake()
    {
        // Singleton pattern para asegurar que solo haya una instancia del panel de sumario
        if (InstanceID != null && InstanceID != this)
        {
            Destroy(gameObject);
            return;
        }
        InstanceID = this;
    }
    void Start()
    {
        continueButton.onClick.AddListener(OnContinueClicked);
        panelRoot.SetActive(false); // Ocultar el panel al inicio aunque ya se haya hecho desde el inspector, por seguridad
    }

    private void OnEnable()
    {
        dayPhaseChangedHandler = OnDayPhaseChanged;
        EventBus.Subscribe(dayPhaseChangedHandler);

        // PULL inicial para ponerse al día con la fase actual al activarse
        ApplyPhase(DayCycleManager.Instance.CurrentPhase);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe(dayPhaseChangedHandler);
    }
    
    private void OnDayPhaseChanged(DayPhaseChangedEvent evt)
    {
        ApplyPhase(evt.NewPhase);
    }
    private void ApplyPhase(DayPhase phase)
    {
        if (phase == DayPhase.Summary)
        {
            // PULL puntual con los datos económicos en el momento de mostrar
            var manager = DayCycleManager.Instance;
            show(manager.CurrentDay, manager.DayIncome, manager.Barista.Salary);
        }
        else
        {
             Hide();
        }
    }
    private void show(int day, float income, float salary)
    {
        // Actualizar los textos con la información del día, ingresos, salarios y balance
        dayText.text = $"Sumario del día {day}";
        incomeText.text = $"Ingresos: {income:F2} €";
        salaryText.text = $"Salarios: - {salary:F2} €";

        float balance = income - salary;
        balanceText.text = $"Balance: {balance:F2} €";

        panelRoot.SetActive(true);
    }

    private void Hide()
    {
        panelRoot.SetActive(false);
    }
    private void OnContinueClicked()
    {
        DayCycleManager.Instance.ChangeState(new ShoppingState(DayCycleManager.Instance));
    }
}
