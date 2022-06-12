using System;
using System.Threading.Tasks;
using CryingOnionTools.ScriptableVariables;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ship Control Settings", menuName = "Crying Onion Tools/ Scriptable Variables/ Ship Control Settings")]
public class ShipControlSettings : ScriptableVariable<ShipControlSettingsStruct>
{
    public async void Refuel(float chargeRatio = 1000)
    {
        while (value.shipFuel < value.maxFuel)
        {
            value.shipFuel += chargeRatio * Time.unscaledDeltaTime;
            await Task.Yield();
        }
        
        value.shipFuel = value.maxFuel;
    }

    public async void Repair(float chargeRatio = 1000)
    {
        while (value.curDurability < value.maxDurability)
        {
            value.curDurability += Mathf.CeilToInt(chargeRatio * Time.unscaledDeltaTime);
            await Task.Yield();
        }
        value.curDurability = value.maxDurability;
    }

    public int MaxDurability
    {
        get => value.maxDurability;
        set => this.value.maxDurability = value;
    }

    public int CurDurability
    {
        get => value.curDurability;
        set => this.value.curDurability = value;
    }
    
    public float MaxFuel
    {
        get => value.maxFuel;
        set => this.value.maxFuel = value;
    }

    public float RatioFuelConsumition
    {
        get => value.ratioFuelConsumition;
        set => this.value.ratioFuelConsumition = value;
    }

    public float MaxVelocity
    {
        get => value.maxVelocity;
        set => this.value.maxVelocity = value;
    }

    public float CurVelocity
    {
        get => value.curVelocity;
        set => this.value.curVelocity = value;
    }

    public float ThrustersPower
    {
        get => value.thrustersPower;
        set => this.value.thrustersPower = value;
    }

    public float GravityBreak
    {
        get => value.gravityBreak;
        set => this.value.gravityBreak = value;
    }

    public float RotationSpeed
    {
        get => value.rotationSpeed;
        set => this.value.rotationSpeed = value;
    }

    public float ShipFuel
    {
        get => value.shipFuel;
        set => this.value.shipFuel = Mathf.Clamp(value, 0, MaxFuel);
    }

    public float ThrustersPotency
    {
        get => value.thrustersPotency;
        set => this.value.thrustersPotency = value;
    }

    // public override void SaveData()
    // {
    //     shipSettings.thrustersPotency = 0;
    //     shipSettings.gravityBreak = 0;
    //     SaveData(shipSettings);
    // }
    //
    // public override void LoadData() => shipSettings = LoadData<ShipControlSettingsStruct>();

    public override void EraseData()
    {
        base.EraseData();
        value = new ShipControlSettingsStruct();
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
    public float gravityBreak;
}