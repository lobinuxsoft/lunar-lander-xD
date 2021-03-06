using System;
using System.Threading.Tasks;
using CryingOnionTools.ScriptableVariables;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ship Control Settings", menuName = "Crying Onion Tools/ Scriptable Variables/ Ship Control Settings")]
public class ShipControlSettings : BaseScriptableVariable
{
    [Tooltip("Ship configurations that can be saved.")]
    [SerializeField] private ShipControlSettingsStruct shipSettings;
    
    public async void Refuel(float chargeRatio = 1000)
    {
        while (shipSettings.shipFuel < shipSettings.maxFuel)
        {
            shipSettings.shipFuel += chargeRatio * Time.unscaledDeltaTime;
            await Task.Yield();
        }
        
        shipSettings.shipFuel = shipSettings.maxFuel;
    }

    public async void Repair(float chargeRatio = 1000)
    {
        while (shipSettings.curDurability < shipSettings.maxDurability)
        {
            shipSettings.curDurability += Mathf.CeilToInt(chargeRatio * Time.unscaledDeltaTime);
            await Task.Yield();
        }
        shipSettings.curDurability = shipSettings.maxDurability;
    }

    public int MaxDurability
    {
        get => shipSettings.maxDurability;
        set => shipSettings.maxDurability = value;
    }

    public int CurDurability
    {
        get => shipSettings.curDurability;
        set => shipSettings.curDurability = value;
    }
    
    public float MaxFuel
    {
        get => shipSettings.maxFuel;
        set => shipSettings.maxFuel = value;
    }

    public float RatioFuelConsumition
    {
        get => shipSettings.ratioFuelConsumition;
        set => shipSettings.ratioFuelConsumition = value;
    }

    public float MaxVelocity
    {
        get => shipSettings.maxVelocity;
        set => shipSettings.maxVelocity = value;
    }

    public float CurVelocity
    {
        get => shipSettings.curVelocity;
        set => shipSettings.curVelocity = value;
    }

    public float ThrustersPower
    {
        get => shipSettings.thrustersPower;
        set => shipSettings.thrustersPower = value;
    }

    public float RotationSpeed
    {
        get => shipSettings.rotationSpeed;
        set => shipSettings.rotationSpeed = value;
    }

    public float ShipFuel
    {
        get => shipSettings.shipFuel;
        set => shipSettings.shipFuel = Mathf.Clamp(value, 0, MaxFuel);
    }

    public float ThrustersPotency
    {
        get => shipSettings.thrustersPotency;
        set => shipSettings.thrustersPotency = value;
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
    public int maxDurability = 1000;
    public int curDurability = 1000;
    [Tooltip("Maximum fuel load")]
    [Range(100, 10000)] public float maxFuel = 1000;
    [Tooltip("Fuel consumption per second")]
    [Range(1, 10000)] public float ratioFuelConsumition = 1;
    [Range(1, 500)] public float thrustersPower = 30;
    [Range(0, 360)] public float rotationSpeed = 180;
    public float maxVelocity = 50;
    public float curVelocity = 0;
    public float shipFuel;
    public float thrustersPotency;
}