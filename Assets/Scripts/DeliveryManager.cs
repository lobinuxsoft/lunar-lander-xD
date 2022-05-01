using UnityEngine;
using UnityEngine.Events;

public class DeliveryManager : MonoBehaviour
{
    [SerializeField] private int landingPadIndex = 0;
    [SerializeField, GradientUsage(true)] private Gradient landingColors;
    [SerializeField] private LandingPad[] landingPads;
    
    public UnityEvent onMissionCompleted;

    private void Start()
    {
        landingPads = FindObjectsOfType<LandingPad>();

        for (int i = 0; i < landingPads.Length; i++)
        {
            landingPads[i].onLandingCompleted += ActivateNextLanding;
        }

        landingPads[landingPadIndex].EnableLanding(landingColors.Evaluate(landingPadIndex));
    }

    private void OnDestroy()
    {
        for (int i = 0; i < landingPads.Length; i++)
        {
            landingPads[i].onLandingCompleted -= ActivateNextLanding;
        }
    }

    private void ActivateNextLanding()
    {
        landingPadIndex++;
        
        if (landingPadIndex == landingPads.Length)
        {
            onMissionCompleted?.Invoke();
        }
        else
        {
            landingPads[landingPadIndex].EnableLanding(landingColors.Evaluate((float)landingPadIndex/landingPads.Length));
        }
    }
}