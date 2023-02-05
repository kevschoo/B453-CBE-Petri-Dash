using System;
using UnityEngine;

[System.Serializable]
public class Stats
{
    // health
    public int      Health;
    public int      MaxHealth;
    public float    HealthMultiplier;
    // damage
    public int      Damage;
    public int      MaxDamage;
    public float    DamageMultiplier;
    // food
    public int      Food;
    public int      MaxFood;
    public float    FoodMultiplier;
    // speed
    public float    Speed;
    public float    MaxSpeed;
    public float    SpeedMultiplier;
    // luck
    public float    Luck;



    public void CalculateStats()
    {
        MaxHealth = (int)(MaxHealth * HealthMultiplier);
        Health = MaxHealth;

        Damage = (int)(MaxDamage * DamageMultiplier);

        Speed = (int)(MaxSpeed * SpeedMultiplier);
    }

    

    public void Heal(int amount)
    {
        Health += amount;
        if (Health > MaxHealth)
            Health = MaxHealth;
    }


    public void TakeDamage(int amount)
    {
        Health -= amount;
        if (Health < 0)
            Health = 0;
    }


    public void CollectFood(int amount)
    {
        Food += amount;
        if (Food > MaxFood)
            Food = MaxFood;
    }


    public void HarvestFood(int amount)
    {
        Food += (int)(amount * FoodMultiplier);
        if (Food > MaxFood)
            Food = MaxFood;
    }


    public int StealFood(int amount)
    {
        if (amount > Food)
        {
            amount = Food;
            Food = 0;
        }
        else
            Food -= amount;

        return amount;
    }



    public void RecalculateHealth(float multiplier)
    {
        int oldMaxHealth = MaxHealth;

        HealthMultiplier += multiplier;
        MaxHealth = (int)(MaxHealth * HealthMultiplier);
        Health += MaxHealth - oldMaxHealth;
    }


    public void RecalculateDamage(float multiplier)
    {
        DamageMultiplier += multiplier;
        Damage = (int)(MaxDamage * DamageMultiplier);
    }


    public void RecalculateSpeed(float multiplier)
    {
        SpeedMultiplier += multiplier;
        Speed = (int)(MaxSpeed * SpeedMultiplier);
    }



    public float HealthPercentage { get => Health / (float)MaxHealth; }
    public float FoodPercentage { get => Food / (float)MaxFood; }
    public bool IsAlive { get => Health > 0; }
    public bool CanAttack { get => Damage > 0; }
}
