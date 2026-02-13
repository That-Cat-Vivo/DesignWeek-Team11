using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawn : MonoBehaviour
{
    [field: SerializeField] public Transform[] SpawnPoints { get; private set; }
    [field: SerializeField] public Color[] PlayerColors { get; private set; }
    public int PlayerCount { get; private set; }

    public TurretControl turret;

    public CarController car;

    public Timer timer;

    public GameObject D1Title;
    public GameObject D2Title;
    public GameObject D1Text;
    public GameObject D2Text;

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        int maxPlayerCount = Mathf.Min(SpawnPoints.Length, PlayerColors.Length);
        if (maxPlayerCount < 1)
        {
            string msg =
                $"You forgot to assign {name}'s {nameof(PlayerSpawn)}.{nameof(SpawnPoints)}" +
                $"and {nameof(PlayerSpawn)}.{nameof(PlayerColors)}!";
            Debug.Log(msg);
        }

        // Prevent adding in more than max number of players
        if (PlayerCount >= maxPlayerCount)
        {
            // Delete new object
            string msg =
                $"Max player count {maxPlayerCount} reached. " +
                $"Destroying newly spawned object {playerInput.gameObject.name}.";
            Debug.Log(msg);
            Destroy(playerInput.gameObject);
            return;
        }

        // Assign spawn transform values
        playerInput.transform.position = SpawnPoints[PlayerCount].position;
        playerInput.transform.rotation = SpawnPoints[PlayerCount].rotation;
        Color color = PlayerColors[PlayerCount];

        // Increment player count
        PlayerCount++;

        // Assign incoming input to correlating conroller
        if (PlayerCount == 1)
        {
            car.AssignPlayerInputDevice(playerInput);
            D1Title.SetActive(false);
            D1Text.SetActive(false);
        }
        else if (PlayerCount == 2)
        {
            turret.AssignPlayerInputDevice(playerInput);
            D2Title.SetActive(false);
            D2Text.SetActive(false);
            timer.GameStart();
        }

    }

    public void OnPlayerLeft(PlayerInput playerInput)
    {
        // Not handling anything right now.
        Debug.Log("Player left...");
    }
}
