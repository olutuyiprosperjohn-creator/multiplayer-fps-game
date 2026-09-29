using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    private NetworkManager networkManager;
    private string currentMatchId;
    private Dictionary<string, GameObject> remotePlayers = new Dictionary<string, GameObject>();
    
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject remotePlayerPrefab;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    
    private void Start()
    {
        networkManager = NetworkManager.Instance;
        networkManager.OnMatchFound += OnMatchFound;
        networkManager.OnPlayerUpdate += OnPlayerUpdate;
        networkManager.OnPlayerHit += OnPlayerHit;
        networkManager.OnPlayerDied += OnPlayerDied;
    }
    
    private void OnMatchFound(string matchId)
    {
        currentMatchId = matchId;
        Debug.Log("Match started: " + matchId);
        // Load game scene
        SceneManager.LoadScene("GameScene");
    }
    
    private void OnPlayerUpdate(Dictionary<string, object> data)
    {
        string socketId = data["socketId"].ToString();
        Vector3 position = (Vector3)data["position"];
        Vector3 rotation = (Vector3)data["rotation"];
        
        if (!remotePlayers.ContainsKey(socketId))
        {
            // Instantiate remote player
            GameObject remotePlayer = Instantiate(remotePlayerPrefab);
            remotePlayers[socketId] = remotePlayer;
        }
        
        GameObject player = remotePlayers[socketId];
        player.transform.position = position;
        player.transform.eulerAngles = rotation;
    }
    
    private void OnPlayerHit(Dictionary<string, object> data)
    {
        string targetId = data["targetId"].ToString();
        int damage = (int)data["damage"];
        int health = (int)data["health"];
        
        if (remotePlayers.ContainsKey(targetId))
        {
            // Update player health UI and visual feedback
            Debug.Log($"Player {targetId} health: {health}");
        }
    }
    
    private void OnPlayerDied(Dictionary<string, object> data)
    {
        string playerId = data["playerId"].ToString();
        string killerId = data["killer"].ToString();
        
        if (remotePlayers.ContainsKey(playerId))
        {
            Destroy(remotePlayers[playerId]);
            remotePlayers.Remove(playerId);
        }
        
        Debug.Log($"Player {playerId} was killed by {killerId}");
    }
    
    private void OnDestroy()
    {
        if (networkManager != null)
        {
            networkManager.OnMatchFound -= OnMatchFound;
            networkManager.OnPlayerUpdate -= OnPlayerUpdate;
            networkManager.OnPlayerHit -= OnPlayerHit;
            networkManager.OnPlayerDied -= OnPlayerDied;
        }
    }
}
