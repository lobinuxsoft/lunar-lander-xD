using System;
using CryingOnionTools.ScriptableVariables;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ship Control Settings", menuName = "Crying Onion Tools/ Scriptable Variables/ Ship Control Settings")]
public class ShipControlSettings : BaseScriptableVariable
{
    [Tooltip("Ship configurations that can be saved.")]
    [SerializeField] private ShipControlSettingsStruct shipSettings;
    
    public event Action<ShipControlSettingsStruct> onValueChange;
    
    public void Refuel() => shipSettings.shipFuel = shipSettings.maxFuel;

    public float MaxFuel
    {
        get => shipSettings.maxFuel;
        set
        {
            shipSettings.maxFuel = value;
            onValueChange?.Invoke(shipSettings);
        }
    }

    public float RatioFuelConsumition
    {
        get => shipSettings.ratioFuelConsumition;
        set
        {
            shipSettings.ratioFuelConsumition = value;
            onValueChange?.Invoke(shipSettings);
        }
    }

    public float MaxVelocity
    {
        get => shipSettings.maxVelocity;
        set
        {
            shipSettings.maxVelocity = value;
            onValueChange?.Invoke(shipSettings);
        }
    }

    public float ThrustersPower
    {
        get => shipSettings.thrustersPower;
        set
        {
            shipSettings.thrustersPower = value;
            onValueChange?.Invoke(shipSettings);
        }
    }

    public float RotationSpeed
    {
        get => shipSettings.rotationSpeed;
        set
        {
            shipSettings.rotationSpeed = value;
            onValueChange?.Invoke(shipSettings);
        }
    }

    public float ShipFuel
    {
        get => shipSettings.shipFuel;
        set
        {
            shipSettings.shipFuel = value;
            onValueChange?.Invoke(shipSettings);
        }
    }

    public float ThrustersPotency
    {
        get => shipSettings.thrustersPotency;
        set
        {
            shipSettings.thrustersPotency = value;
            onValueChange?.Invoke(shipSettings);
        }
    }

    public override void SaveData()
    {
        shipSettings.thrustersPotency = 0;
        SaveData(shipSettings);
    }

    public override void LoadData() => shipSettings = LoadData<ShipControlSettingsStruct>();

    public override void EraseSaveFile()
    {
        base.EraseSaveFile();
        shipSettings = new ShipControlSettingsStruct();
    }
}

[Serializable]
public class ShipControlSettingsStruct
{
    [Tooltip("Maximum fuel load")]
    [Range(100, 10000)] public float maxFuel = 1000;
    [Tooltip("Fuel consumption per second")]
    [Range(1, 10000)] public float ratioFuelConsumition = 1;
    [Range(1, 500)] public float thrustersPower = 30;
    [Range(0, 360)] public float rotationSpeed = 180;
    public float maxVelocity = 50;
    [Tooltip("Current fuel.")]
    public float shipFuel;
    [Tooltip("How much thruster power is being used.")]
    public float thrustersPotency;
}