using UnityEngine;
using UnityEngine.UI;  
using TMPro; 
public class ShopPanelUI : MonoBehaviour
{
    public static ShopPanelUI Instance { get; private set; }

    [Header("Panel")]
    [SerializeField] private GameObject panelRoot;

    [Header("Textos informativos")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI stockText;
    [SerializeField] private TextMeshProUGUI recommendationText;

    [Header("Botones de packs")]
    [SerializeField] private Button pack500Button;
    [SerializeField] private Button pack1000Button;
    [SerializeField] private Button pack2000Button;
    [SerializeField] private Button StartDayButton; 

    [Header("Textos de los botones de packs")]
    [SerializeField] private TextMeshProUGUI pack500Text;
    [SerializeField] private TextMeshProUGUI pack1000Text;
    [SerializeField] private TextMeshProUGUI pack2000Text;

    [Header("Recomendaciones")]
    [SerializeField] private int recommendationDays = 5;
    [SerializeField] private float safetyMargin = 1.2f; // Margen de seguridad para recomendar un pack más grande

    [SerializeField] private PackCatalog packCatalog; // Referencia al catálogo de packs para obtener los gramos y precios

    private System.Action<DayPhaseChangedEvent> dayPhaseChangedHandler;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        // Configura los textos de los botones con los precios
        pack500Button.onClick.AddListener(() => BuyPack(packCatalog.packs[0].grams, packCatalog.packs[0].price));
        pack1000Button.onClick.AddListener(() => BuyPack(packCatalog.packs[1].grams, packCatalog.packs[1].price));
        pack2000Button.onClick.AddListener(() => BuyPack(packCatalog.packs[2].grams, packCatalog.packs[2].price));
        StartDayButton.onClick.AddListener(OnStartDayClicked); // Agrega el listener para el botón de iniciar día
        panelRoot.SetActive(false); // Oculta el panel al inicio
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
        if (phase == DayPhase.Shopping)
        {
            Show(DayCycleManager.Instance.CurrentDay);
        }
        else
        {
            Hide();
        }
    }
    private void Show(int day)
    {
        titleText.text = $"Tienda día {day}";
        UpdateDisplay();
        UpdateRecommendation(day);
        panelRoot.SetActive(true);
    }

    private void Hide()
    {
        panelRoot.SetActive(false);
    }

    private void UpdateDisplay()
    {
        float money = MoneyManager.Instance.CurrentMoney;
        float stock = IngredientManager.Instance.CurrentCoffeGrams;
        float gramsPerCup = IngredientManager.Instance.CoffeGramsPerCup;

        moneyText.text = $"Dinero: {money:F2} €";

        if (stock >= 1000f)
            stockText.text = $"Stock: Quedan {stock / 1000f:F2} kg, llega para {stock / gramsPerCup:F2} cafés";
        else
            stockText.text = $"Stock: {stock:F0} g, llega para {stock / gramsPerCup:F2} cafés";

        // Actualizar textos de los botones con cafés dinámicos
        pack500Text.text = $"Pack 500g — {packCatalog.packs[0].price:F0}€ ({packCatalog.packs[0].grams / gramsPerCup:F0} cafés)";
        pack1000Text.text = $"Pack 1kg — {packCatalog.packs[1].price:F0}€ ({packCatalog.packs[1].grams / gramsPerCup:F0} cafés)";
        pack2000Text.text = $"Pack 2kg — {packCatalog.packs[2].price:F0}€ ({packCatalog.packs[2].grams / gramsPerCup:F0} cafés)";

        // Activar/desactivar botones según dinero disponible
        pack500Button.interactable = money >= packCatalog.packs[0].price;
        pack1000Button.interactable = money >= packCatalog.packs[1].price;
        pack2000Button.interactable = money >= packCatalog.packs[2].price;
    }
    private void BuyPack(float packGrams, float packPrice)
    {
        MoneyManager.Instance.SpendMoney(packPrice);
        IngredientManager.Instance.AddCoffee(packGrams);
        UpdateDisplay(); // Actualizar la UI después de la compra
    }
    private void UpdateRecommendation(int day)
    {
        if (day > 1)
        {
            recommendationText.text = "";
            return;
        }

        int coffeesServedToday = DayCycleManager.Instance.CoffeesServedToday;
        float gramsPerCup = IngredientManager.Instance.CoffeGramsPerCup;
        float currentStock = IngredientManager.Instance.CurrentCoffeGrams;
        float currentMoney = MoneyManager.Instance.CurrentMoney;
        
        // Calcular café necesario para mañana
        float gramsNeeded = coffeesServedToday * gramsPerCup * safetyMargin;
        float gramsShortage = gramsNeeded - currentStock;

        // Lógica si ya tiene suficiente stock
        if (gramsShortage <= 0)
        {
            if (currentMoney <= packCatalog.packs[2].price)
                recommendationText.text = "¡Tienes suficiente café para mañana! Mejor ahorra para comprar un pack más grande.";
            else
                recommendationText.text = "¡Tienes suficiente café para mañana!";    
            return;
        }

        // Lógica por si no hay suficiente stock
        if (currentMoney >= packCatalog.packs[2].price && gramsShortage > packCatalog.packs[1].grams)
            recommendationText.text = "Compra el pack de 2kg para asegurarte de tener suficiente café para mañana.";
        else if (currentMoney >= packCatalog.packs[1].price && gramsShortage > packCatalog.packs[0].grams)
            recommendationText.text = "Compra el pack de 1kg para asegurarte de tener suficiente café para mañana.";
        else if (currentMoney >= packCatalog.packs[0].price)
            recommendationText.text = "Compra el pack de 500g para asegurarte de tener suficiente café para mañana.";
        else
            recommendationText.text = "No tienes suficiente dinero para comprar más café ¡intenta vender más cafés hoy!";
    }

    private void OnStartDayClicked()
    {
        DayCycleManager.Instance.AdvanceToNextDay();
        DayCycleManager.Instance.ChangeState(new ServiceState(DayCycleManager.Instance));
    }
 
}
