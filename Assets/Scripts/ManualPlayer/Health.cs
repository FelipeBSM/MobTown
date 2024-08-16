using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Health Settings")]
    public float totalHealth = 200f; // upgradable
    public float healthRate;
    public float currentHealth;
    public float currentMoneyHealth;

    [Header("Damage Settings")]
    public float damageNormalzied;
    public float currentDamage;
    public float carCurrentSpeed;


    private Car car;
    private HealthBar healthBar;

    private bool firstHit = true;
    void Start()
    {

        currentHealth = totalHealth;

        currentMoneyHealth = 0f;

        healthBar = GetComponentInChildren<HealthBar>();
        car = this.gameObject.GetComponent<Car>();
        healthBar.SetHealth(GetCurrentHealthNormalized());
       
    }


    private void OnCollisionEnter(Collision collision)
    {
        //bateu?
        //tira vida
        if (firstHit && !car.HUD.textBox.activeInHierarchy)
        {
            string text = "OUCH&¨%$! I think i should go to the mechanical, located at Park Street. ";
            string who = "Me:";    
            car.HUD.ShowSpecialText(text,who);
            firstHit = false;
        }
        carCurrentSpeed = car.GetCarSpeed() * 3.6f; //km/h
        currentDamage = (carCurrentSpeed /2f)*car.HUD.ResistanceLevelNormalizedInverse();
     
        currentHealth -= currentDamage;
        currentMoneyHealth += currentDamage;
        currentHealth = Mathf.Clamp(currentHealth, 0, totalHealth);
        currentMoneyHealth = Mathf.Clamp(currentMoneyHealth, 0, totalHealth);
        healthBar.DamageEffect();
        healthBar.SetHealth(GetCurrentHealthNormalized());
        StartCoroutine(healthBar.KillHolder());
        Debug.Log(currentHealth);
    }

    public void SetHealthValue(float _newHealth)
    {
        this.totalHealth = _newHealth;
    }
    public float GetCurrentHealth()
    {
        return this.currentHealth;
    }
    public void SetCurrentHealth(float _health)
    {
        this.currentHealth = _health;
        healthBar.SetHealth(currentHealth);
    }
    public void SetCurrentDamage(float _damage)
    {
        this.currentMoneyHealth = _damage;
       
    }
    public float GetCurrentHealthNormalized()
    {
        float n = currentHealth / totalHealth;
        
        Debug.Log(n);
        return n;
    }
    public float GetCurrentHealthNormalizedToVelocity()
    {
        float n = currentHealth / totalHealth;
        if (n >= 0.001)
        {
            n = Mathf.Clamp(n, 0.7f, 1);
            Debug.LogError("THISSSS " + n.ToString());
            return n;
        }
        else
        {
            return n;
        }
        
    }
    public float GetCurrentHealthNormalizedToAngle()
    {
        float n = currentHealth / totalHealth;
        if (n >= 0.2)
        {
            n = Mathf.Clamp(n, 0.3f, 1);
            return n;
        }
        else
        {
            return n = Mathf.Clamp(n,0.1f,0.2f);
        }

    }
    public void InMechanicalHUD()
    {
        healthBar.IsOnMec();
    }
    public void OutMechanicalHUD()
    {
        healthBar.OutOffMec();
    }
    public float GetCurrentMoneyHealthNormalized()
    {
        Debug.Log("MNEY HELTH" + this.currentMoneyHealth / totalHealth);
        return this.currentMoneyHealth / totalHealth;
        
    }


}
