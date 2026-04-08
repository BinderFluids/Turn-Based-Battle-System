using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public int CurrentHealth;
    
    public void ChangeHealth(int damage)
    {
        CurrentHealth -= damage;
    }
    
    
}