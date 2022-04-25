using UnityEngine;
using UnityEngine.UIElements;

public class GameplayUIContol : MonoBehaviour
{
    [SerializeField] private Gradient warningColor;
    [SerializeField] private ShipControlSettings shipSettings;
    private VisualElement root;
    
    private Label fuelLabel;
    private VisualElement fuelBar;
    
    private Label thrusterLabel;
    private VisualElement thrusterBar;

    private Label velocityLabel;

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
}
