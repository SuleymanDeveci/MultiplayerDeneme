using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStamina : NetworkBehaviour
{
    public Slider StaminaSlider;
    public float StaminaGainRate;
    public float StaminaLoseRate;

    [Networked, OnChangedRender(nameof(StaminaChanged))] public float NetworkedStamina { get; set; }

    private void Start()
    {
        NetworkedStamina = 100;
    }
    private void FixedUpdate()
    {
        if(HasStateAuthority && Input.GetKey(KeyCode.LeftShift))
        {
            NetworkedStamina -= StaminaLoseRate * Time.deltaTime;
        }
        else if(HasStateAuthority && !Input.GetKey(KeyCode.LeftShift) && NetworkedStamina < 100)
        {
            
            NetworkedStamina += StaminaGainRate * Time.deltaTime;
        }
    }

    void StaminaChanged()
    {
        StaminaSlider.value = NetworkedStamina;
    }
}
