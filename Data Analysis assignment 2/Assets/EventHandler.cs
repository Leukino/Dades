using Gamekit3D;
using Gamekit3D.Message;
using System;
using System.Collections.Generic;
using UnityEngine;
using static Gamekit3D.Damageable;
public class RecordEvent {
    protected Vector3 pos = new Vector3(0, 0, 0);
    protected DateTime date = DateTime.MinValue;
    protected string userID = "UnknownID";
    public RecordEvent(Vector3 position, DateTime start, string userName)
    {
        pos = position;
        date = start;
        userID = userName;
    }
}

public class DamageEvent : RecordEvent
{
    string damager = "UnknownDamager";
    bool death = false;
    public DamageEvent(Vector3 position, DateTime start, string userName, string enemy, bool hasDied): base(position, start, userName) 
    {
        pos = position;
        date = start;
        userID = userName;
        damager = enemy;
        death = hasDied;
    }
}

public class RespawnEvent : RecordEvent
{
    uint attempts = 0;
    public RespawnEvent(Vector3 position, DateTime start, string userName, uint tries) : base(position, start, userName)
    {
        pos = position;
        date = start;
        userID = userName;
        attempts = tries;
    }
}

public class EventHandler : MonoBehaviour, IMessageReceiver
{
    uint tries = 0;
    public string playerName = "Unknown Player";
    public float nameID = 0;
    public List<DamageEvent> damageEvents = new List<DamageEvent>();
    public List<DamageEvent> deathEvents = new List<DamageEvent>();
    public List<RespawnEvent> respawnEvents = new List<RespawnEvent>();
    // Start is called before the first frame update
    public void OnReceiveMessage(MessageType type, object sender, object msg)
    {
        switch (type) 
        {
            case MessageType.DAMAGED:
                {
                    onDamageRecieved((Damageable)sender, (DamageMessage)msg);
                }
                    break;
            case MessageType.DEAD:
                {
                    onDeathReceived((Damageable)sender, (DamageMessage)msg);
                }
                break;
            case MessageType.RESPAWN:
                {
                    tries++;
                    onRespawnRecieved((DamageMessage)msg);
                }
                break;
        }
    }

    private void onDamageRecieved(Damageable sender, DamageMessage msg)
    {
        Debug.Log("Registered Damaged: " + msg.damager.name + "\nAmount: " + msg.amount + "\nPosition: " + msg.damageSource);
        DamageEvent newDamageEvent = new DamageEvent(msg.damageSource, DateTime.UtcNow, playerName, msg.damager.name, false);
        damageEvents.Add(item: newDamageEvent);
    }

    private void onDeathReceived(Damageable sender, DamageMessage msg)
    {
        Debug.Log("Registered Kill from" + msg.damager + "\n in Position: " + msg.damageSource);
    }

    private void onRespawnRecieved(DamageMessage msg) 
    {
        Debug.Log("Respawned player: " + playerName + "\nAttempt: " + tries + "\nIn position: " + msg.damageSource);
        RespawnEvent newresp = new RespawnEvent(msg.damageSource, DateTime.UtcNow, playerName, tries);
        respawnEvents.Add(newresp);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
