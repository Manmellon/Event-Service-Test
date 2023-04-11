using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Proyecto26;

[Serializable]
public struct Event
{
    public string type;
    public string data;

    public Event(string type, string data)
    {
        this.type = type;
        this.data = data;
    }
}

public class EventService : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string serverUrl;
    [SerializeField] private float cooldownBeforeSend;

    [Header("Debug")]
    [SerializeField] private float prevSendTime;

    [SerializeField] private List<Event> eventsQueue = new List<Event>();

    private static EventService singleton;

    void Awake()
    {
        if (singleton == null) singleton = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= prevSendTime + cooldownBeforeSend)
        {
            SendEvents();
        }
    }

    public void TrackEvent(string type, string data)
    {
        eventsQueue.Add(new Event(type, data));
    }

    public void SendEvents()
    {
        Debug.Log(JsonUtility.ToJson(eventsQueue));
    }
}
