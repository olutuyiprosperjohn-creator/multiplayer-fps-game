using UnityEngine;
using SocketIOClient;
using System;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance { get; private set; }
    
    private SocketIO socket;
    public string ServerUrl = "http://localhost:3000";
    private string authToken;
    private string matchId;
    
    public event Action OnConnected;
    public event Action<string> OnMatchFound;
    public event Action<Dictionary<string, object>> OnPlayerUpdate;
    public event Action<Dictionary<string, object>> OnPlayerHit;
    public event Action<Dictionary<string, object>> OnPlayerDied;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    private async void Start()
    {
        await InitializeSocket();
    }
    
    private async System.Threading.Tasks.Task InitializeSocket()
    {
        var uri = new Uri(ServerUrl);
        socket = new SocketIO(uri);
        
        socket.OnConnected += Socket_OnConnected;
        socket.OnDisconnected += Socket_OnDisconnected;
        socket.On("match_found", OnMatchFoundEvent);
        socket.On("player_update", OnPlayerUpdateEvent);
        socket.On("player_hit", OnPlayerHitEvent);
        socket.On("player_died", OnPlayerDiedEvent);
        
        await socket.ConnectAsync();
    }
    
    private void Socket_OnConnected(object sender, EventArgs e)
    {
        Debug.Log("Connected to server");
        OnConnected?.Invoke();
    }
    
    private void Socket_OnDisconnected(object sender, string e)
    {
        Debug.Log("Disconnected from server: " + e);
    }
    
    public void JoinQueue(string username, int skill = 1)
    {
        var data = new { username, skill };
        socket.Emit("join_queue", data);
    }
    
    public void LeaveQueue()
    {
        socket.Emit("leave_queue");
    }
    
    public void SendPlayerInput(Vector3 position, Vector3 rotation, Vector3 velocity)
    {
        var data = new { position, rotation, velocity };
        socket.Emit("player_input", data);
    }
    
    public void FireWeapon(string targetSocketId, string weapon, float distance, string hitLocation)
    {
        var data = new { targetSocketId, weapon, distance, hitLocation };
        socket.Emit("fire_weapon", data);
    }
    
    private void OnMatchFoundEvent(SocketIOResponse response)
    {
        var data = response.GetValue();
        matchId = data["matchId"].ToString();
        Debug.Log("Match found: " + matchId);
        OnMatchFound?.Invoke(matchId);
    }
    
    private void OnPlayerUpdateEvent(SocketIOResponse response)
    {
        var data = response.GetValue();
        OnPlayerUpdate?.Invoke((Dictionary<string, object>)data);
    }
    
    private void OnPlayerHitEvent(SocketIOResponse response)
    {
        var data = response.GetValue();
        OnPlayerHit?.Invoke((Dictionary<string, object>)data);
    }
    
    private void OnPlayerDiedEvent(SocketIOResponse response)
    {
        var data = response.GetValue();
        OnPlayerDied?.Invoke((Dictionary<string, object>)data);
    }
}
