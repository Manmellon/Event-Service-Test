using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private Button _startLevelButton;
    [SerializeField] private Button _getRewardButton;
    [SerializeField] private Button _payCoinsButton;

    public static UI singleton;

    void Awake()
    {
        if (singleton == null) singleton = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _startLevelButton.onClick.AddListener( () => EventService.singleton.TrackEvent("levelStart", "level:" + Random.Range(0,10).ToString()) );
        _getRewardButton.onClick.AddListener( () => EventService.singleton.TrackEvent("getReward", "reward:" + Random.Range(0,10).ToString()) );
        _payCoinsButton.onClick.AddListener( () => EventService.singleton.TrackEvent("payCoins", "coins:" + Random.Range(0,10).ToString()) );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
