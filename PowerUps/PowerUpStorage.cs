using System;
using System.Collections.Generic;

public class PowerUpStorage
{
    private List<Type> _powerUpList;

    public PowerUpStorage()
    {
        _powerUpList = new List<Type>();
        AddPowerUpsToStorage();
    }

    private void AddPowerUpsToStorage()
    {
        _powerUpList.Add(typeof(SizePowerUp));
        _powerUpList.Add(typeof(InvulnerabilityPowerUp));
    }

    public Type GetRandomPowerUp() => _powerUpList[UnityEngine.Random.Range(0, _powerUpList.Count)];
}

