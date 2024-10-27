using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : NetworkBehaviour
{
    public Slider Slider;

    [Networked, OnChangedRender(nameof(HealthChanged))] public int NetworkedHealth { get; set; }

    private void Start()
    {
        NetworkedHealth = 100;
    }
    private void Update()
    {
        if(HasStateAuthority && Input.GetKeyDown(KeyCode.X))
        {
            NetworkedHealth -= 10;
        }
    }

    void HealthChanged()
    {
        Slider.value = NetworkedHealth;
    }
}
