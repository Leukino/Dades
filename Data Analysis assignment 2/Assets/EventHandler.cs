using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamekit3D;
using Gamekit3D.Message;
using static Gamekit3D.Damageable;


public class EventHandler : MonoBehaviour, IMessageReceiver
{
    // Start is called before the first frame update
    public void OnReceiveMessage(MessageType type, object sender, object msg)
    {
        switch (type) 
        {
            case MessageType.DAMAGED:
                onDamageRecieved((Damageable)sender, (DamageMessage)msg);
                break;
        }
    }

    private void onDamageRecieved(Damageable sender, DamageMessage msg)
    {
        Debug.Log("Registered Damaged: " + msg.damager.name + "\nAmount: " + msg.amount + "\nPosition: " + msg.damageSource);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
