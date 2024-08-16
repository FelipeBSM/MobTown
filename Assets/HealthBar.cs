using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Image healthBar;
    public Image damageBar;
    public GameObject holder;

    private const float DAMAGE_HEALTH_FADE_TIMER_MAX = 1f;
    private Color damageBarColor;
    private float damageHealthFadeTimer;
    // Start is called before the first frame update

    private void Start()
    {
        holder.SetActive(false);
        damageBarColor = damageBar.color;
        damageBarColor.a = 0f;
        damageBar.color = damageBarColor;
    }
    private void Update()
    {
        if(damageBarColor.a > 0)
        {
            damageHealthFadeTimer -= Time.deltaTime;
            if(damageHealthFadeTimer <= 0)
            {
                float fadeAmmount = 5f;
                damageBarColor.a -= fadeAmmount * Time.deltaTime;
                damageBar.color = damageBarColor;
            }
        }
    }
    public void DamageEffect()
    {
        if (damageBarColor.a <= 0)
        {
            damageBar.fillAmount = healthBar.fillAmount;
        }
       
        damageBarColor.a = 1f;
        damageBar.color = damageBarColor;
        damageHealthFadeTimer = DAMAGE_HEALTH_FADE_TIMER_MAX;
    }
    public void SetHealth(float health)
    {
        healthBar.fillAmount = health;
    }
    public void IsOnMec()
    {
        holder.SetActive(true);
    }
    public void OutOffMec()
    {
        holder.SetActive(false);
    }
    public IEnumerator KillHolder()
    {
        holder.SetActive(true);
        yield return new WaitForSeconds(2f);
        holder.SetActive(false);
    }

}
