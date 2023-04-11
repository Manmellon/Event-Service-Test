using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Proyecto26;
using System.IO;

[Serializable]
public struct Event
{
    public string type;
    public string data;
    public bool sendingNow;

    public Event(string type, string data, bool sendingNow = false)
    {
        this.type = type;
        this.data = data;
        this.sendingNow = sendingNow;
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

    public static EventService singleton;

    void Awake()
    {
        if (singleton == null) singleton = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadEventsQueue();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= prevSendTime + cooldownBeforeSend)
        {
            prevSendTime = Time.time;
            SendEvents();
        }
    }

    public void TrackEvent(string type, string data)
    {
        eventsQueue.Add(new Event(type, data));

        SaveEventsQueue();
    }

    public void SendEvents()
    {
        for (int i = 0; i < eventsQueue.Count; i++)
        {
            eventsQueue[i] = new Event(eventsQueue[i].type, eventsQueue[i].data, true);
        }

        string body = JsonHelper.ArrayToJsonString(eventsQueue.ToArray());
        body = body.Replace("Items", "events");
        Debug.Log(body);

        RestClient.Post(serverUrl, body)
        .Then(res =>
        {
            if (res.StatusCode == 200)
            {
                eventsQueue.RemoveAll( (x) => x.sendingNow == true );
                SaveEventsQueue();
            }
            else
            {
                Debug.LogWarning(res.StatusCode);
            }
        })
        .Catch(err =>
        {
            Debug.LogError(err.Message);
        });
    }

    public void SaveEventsQueue()
    {
        using (StreamWriter writer = new StreamWriter("eventsQueue.json"))
        {
            writer.WriteLine(JsonHelper.ArrayToJsonString(eventsQueue.ToArray()));
        }
    }

    public void LoadEventsQueue()
    {
        try
        {
            using (StreamReader reader = new StreamReader("eventsQueue.json"))
            {
                Event[] array = JsonHelper.FromJsonString<Event>(reader.ReadToEnd());
                eventsQueue = new List<Event>();
                eventsQueue.AddRange(array);
            }
        }
        catch (FileNotFoundException e)
        {
            Debug.LogWarning(e.Message);
        }
    }
}
