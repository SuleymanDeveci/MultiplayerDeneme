using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class Health : NetworkBehaviour
{
    public Slider healthSlider;
    [Networked, OnChangedRender(nameof(HealthChanged))] public float NetworkedHealth { get; set; }
    
    void HealthChanged()
    {
        Debug.Log("Health changed to: " + NetworkedHealth);
    }

    [Rpc(RpcSources.All, RpcTargets.All)] public void DealDamageRpc(float damage)
    {
        Debug.Log("Damage dealed");
        NetworkedHealth -= damage;
        if(NetworkedHealth < 1)
        {
            healthSlider.gameObject.SetActive(false);
        }
        else
        {
            healthSlider.value = NetworkedHealth;
        }
        
    }

}
