using Fusion;
using UnityEngine;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    public GameObject playerPrefab;   // Spwan edecegimiz prefabi tanimladik
    public void PlayerJoined(PlayerRef player)    // IPlayerJoined Interface'sinden gelen ve lobiye her oyuncu katildigi zaman cagrilan metod
    {
        if(player == Runner.LocalPlayer)    // eger katilan oyuncu kendisi ise true doner
        {
            Runner.Spawn(playerPrefab, new Vector3(0, 1, 0), Quaternion.identity);  // playerPrefab objesini belirlenen konumda ve rotasyonda spawn eder
        }
    }
}
