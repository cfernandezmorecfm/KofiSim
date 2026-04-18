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

    [Header("Precios de los packs")]
    [SerializeField] private float pack500Price = 20f;
    [SerializeField] private float pack1000Price = 30f;
    [SerializeField] private float pack2000Price = 50f;

    [Header("Recomendaciones")]
    [SerializeField] private int recommendationDays = 5;
    [SerializeField] private float safetyMargin = 1.2f; // Margen de seguridad para recomendar un pack más grande

    // Variables para asignar cantidad de gramos por paquete
    private float pack500Grams = 500f;
    private float pack1000Grams = 1000f;
    private float pack2000Grams = 2000f;

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
        pack500Button.onClick.AddListener(() => BuyPack(pack500Grams, pack500Price));
        pack1000Button.onClick.AddListener(() => BuyPack(pack1000Grams, pack1000Price));
        pack2000Button.onClick.AddListener(() => BuyPack(pack2000Grams, pack2000Price));
        StartDayButton.onClick.AddListener(OnStartDayClicked); // Agrega el listener para el botón de iniciar día
        panelRoot.SetActive(false); // Oculta el panel al inicio
    }

    public void Show(int day)
    {
        titleText.text = $"Tienda día {day}";
        UpdateDisplay();
        UpdateRecommendation(day);
        panelRoot.SetActive(true);
    }

    public void Hide()
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
        pack500Text.text = $"Pack 500g — {pack500Price:F0}€ ({pack500Grams / gramsPerCup:F0} cafés)";
        pack1000Text.text = $"Pack 1kg — {pack1000Price:F0}€ ({pack1000Grams / gramsPerCup:F0} cafés)";
        pack2000Text.text = $"Pack 2kg — {pack2000Price:F0}€ ({pack2000Grams / gramsPerCup:F0} cafés)";

        // Activar/desactivar botones según dinero disponible
        pack500Button.interactable = money >= pack500Price;
        pack1000Button.interactable = money >= pack1000Price;
        pack2000Button.interactable = money >= pack2000Price;
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

        int coffeesSoldToday = DayCycleManager.Instance.CoffeesSoldToday;
        float gramsPerCup = IngredientManager.Instance.CoffeGramsPerCup;
        float currentStock = IngredientManager.Instance.CurrentCoffeGrams;
        float currentMoney = MoneyManager.Instance.CurrentMoney;
        
        // Calcular café necesario para mañana
        float gramsNeeded = coffeesSoldToday * gramsPerCup * safetyMargin;
        float gramsShortage = gramsNeeded - currentStock;

        // Lógica si ya tiene suficiente stock
        if (gramsShortage <= 0)
        {
            if (currentMoney <= pack2000Price)
                recommendationText.text = "¡Tienes suficiente café para mañana! Mejor ahorra para comprar un pack más grande.";
            else
                recommendationText.text = "¡Tienes suficiente café para mañana!";    
            return;
        }

        // Lógica por si no hay suficiente stock
        if (currentMoney >= pack2000Price && gramsShortage > pack1000Grams)
            recommendationText.text = "Compra el pack de 2kg para asegurarte de tener suficiente café para mañana.";
        else if (currentMoney >= pack1000Price && gramsShortage > pack500Grams)
            recommendationText.text = "Compra el pack de 1kg para asegurarte de tener suficiente café para mañana.";
        else if (currentMoney >= pack500Price)
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
