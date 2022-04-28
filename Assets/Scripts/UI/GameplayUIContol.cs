using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameplayUIContol : MonoBehaviour
{
    [SerializeField] private Gradient warningColor;
    [SerializeField] private ShipControlSettings shipSettings;
    [SerializeField] private string sceneToLoad = "MainMenuScene";
    private VisualElement root;
    
    private Label fuelLabel;
    private VisualElement fuelBar;
    
    private Label thrusterLabel;
    private VisualElement thrusterBar;

    private Label velocityLabel;

    private VisualElement gameplayPanel;
    private VisualElement gameoverPanel;

    private Button mainmenuButton;

    private void Awake()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        
        fuelLabel = root.Q<Label>("fuel-label");
        fuelLabel.text = "FUEL";
        fuelBar = root.Q<VisualElement>("fuel-bar");
        
        thrusterLabel = root.Q<Label>("thruster-label");
        thrusterLabel.text = "THRUSTER";
        thrusterBar = root.Q<VisualElement>("thruster-bar");
        
        velocityLabel = root.Q<Label>("velocity-label");

        gameplayPanel = root.Q<VisualElement>("gameplay-panel");
        gameplayPanel.SetEnabled(true);
        
        gameoverPanel = root.Q<VisualElement>("gameover-panel");
        gameoverPanel.SetEnabled(false);

        mainmenuButton = root.Q<Button>("mainmenu-button");
        mainmenuButton.clicked += ToMainMenu;
    }

    private void Update()
    {
        fuelBar.style.width = Length.Percent((shipSettings.ShipFuel / shipSettings.MaxFuel) * 100);
        thrusterBar.style.width = Length.Percent(shipSettings.ThrustersPotency * 100);
        velocityLabel.text = $"{MsToKmhConversion(shipSettings.CurVelocity):0} Km/h";
        velocityLabel.style.color = warningColor.Evaluate(shipSettings.CurVelocity / shipSettings.MaxVelocity);
    }
    
    /// <summary>
    /// Convert Meters per seconds to Kilometers per hour
    /// </summary>
    private float MsToKmhConversion(float value)
    {
        int metersInKilometers = 1000;
        int secondsInHour = 3600;
        return value / metersInKilometers * secondsInHour;
    }

    public void ShowGameOver()
    {
        gameplayPanel.SetEnabled(false);
        gameoverPanel.SetEnabled(true);
    }

    private void ToMainMenu()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
