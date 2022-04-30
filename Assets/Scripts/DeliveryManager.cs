using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    [SerializeField] private int landingPadIndex = 0;
    [SerializeField, GradientUsage(true)] private Gradient landingColors;
    [SerializeField] private LandingPad[] landingPads;

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
            Debug.Log("CONGRATS, YOU COMPLETE THE DEMO!!!");
        }
        else
        {
            landingPads[landingPadIndex].EnableLanding(landingColors.Evaluate((float)landingPadIndex/landingPads.Length));
        }
    }
}