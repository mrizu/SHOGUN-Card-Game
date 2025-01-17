using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapEvent : MonoBehaviour, IPointerClickHandler
{
    public List<MapEvent> _childrenEvents,_parentEvents,_enabledEvents,_eventPath;
    public RectTransform _rectTransform;
    public int healValue=10;
    public string _eventType { get; private set; }
    public Vector2 _eventPlacement { get; private set; }

    [SerializeField] private LineController _line;
    [SerializeField] private Sprite _visited;
    [SerializeField] private Image _imageControler;
    [SerializeField] private AudioClip _clip;
    private CombatManager _combatManager;
    private GridManager _gridManager;
    private int _stageSceneChange=5;
    private GameObject _map;

    public static event Action OnNewStageStarted;
    public static event Action OnBossStageStarted;
    
    public void OnPointerClick(PointerEventData eventData)
    {
       UpdateMap();
       HandleMapEvent();
    }

    public List<MapEvent> GetParents(){
        return _parentEvents;
    }

    public List<MapEvent> GetPath(){
        return _eventPath;
    }

    public void addToPath(List<MapEvent> previousEvents){
        foreach( MapEvent e in previousEvents){
            if(!_eventPath.Contains(e)){
                _eventPath.Add(e);
            }
        }
    }

    private void Awake() {
        _parentEvents=new List<MapEvent>();
        _childrenEvents=new List<MapEvent>();
        _eventPath=new List<MapEvent>();
        _combatManager=GameObject.Find("/Main Canvas/Card System Manager").GetComponent<CombatManager>();
        _gridManager=gameObject.GetComponentInParent<GridManager>();
        
    }

    public void ImportEnabledEvents(List<MapEvent> events){
        _enabledEvents=new List<MapEvent>(events);
    }

    public void setParentEvents(List<MapEvent> parents){
        _parentEvents.AddRange(parents);
        foreach(MapEvent p in parents){
            LineController newLine=Instantiate(_line,transform);
            newLine.setTarget(p.transform,transform);
        }
        _eventPath.AddRange(parents);
    }

    public void addChildrenEvent(MapEvent child){
        _childrenEvents.Add(child);
    }

    public void setEventType(string type){
        _eventType=type;
    }
    public void setEventPlacement(Vector2 placement){
        _eventPlacement=placement;
    }

    private void UpdateMap(){
        _map=GameObject.FindWithTag("MapTag");
        _imageControler.sprite=_visited;

        foreach (MapEvent enabled in _enabledEvents){
            enabled.GetComponent<Image>().raycastTarget=false;
            enabled.GetComponent<Image>().color=new Color32(91,91,91,255);
        }
        _imageControler.color=Color.white;
        _enabledEvents.Clear();

        _enabledEvents.AddRange(_childrenEvents);
        foreach (MapEvent child in _childrenEvents){
            child.GetComponent<Image>().raycastTarget=true;
            child.GetComponent<Image>().color=Color.black;
            child.GetComponent<MapEvent>().ImportEnabledEvents(_enabledEvents);
        }
        if(_eventType!="Scouting") GenerateNextStage(false);
    }

    public void GenerateNextStage(bool scouting){
        _gridManager.HandleUpdate(gameObject.GetComponent<MapEvent>(),scouting);
    }

    private void HandleMapEvent()
    {
        
        if(_eventType=="Combat") OpenCombatScene();
        else if(_eventType=="Scouting") HandleScouting();
        else if(_eventType=="Campfire") HandleCampfire();
        else if(_eventType=="Boss") HandleBoss();
        else if(_eventType=="Treasure") HandleTreasure();
        else Debug.Log("Unknown Event Type");
    }

    private void HandleTreasure(){
        
        playEventClipAudio();
        _combatManager.HandleTreasureChest();
        MapObject.MapInstance.GetComponent<MapObject>().ShowTreasure();
    }

    private void HandleScouting(){
        playEventClipAudio();
        GenerateNextStage(true);
    }

    private void HandleCampfire(){
        playEventClipAudio();
        _combatManager.HealPlayer(healValue);
    }
    private void HandleBoss(){
        _map.transform.Find("Cave Scene").gameObject.SetActive(true);
        _map.transform.Find("Village Scene").gameObject.SetActive(false);
        MapObject.MapInstance.GetComponent<MapObject>().HideMap();
        OnBossStageStarted?.Invoke();
    }
    private void playEventClipAudio(){
        if(_clip!=null){ 
            MapObject.MapInstance.GetComponent<MapObject>().PlayEventSound(_clip);
        }
    }

    public void OpenCombatScene()
    {
        if(_stageSceneChange<=_eventPlacement.x){
            _map.transform.Find("Village Scene").gameObject.SetActive(true);
            _map.transform.Find("Bridge Scene").gameObject.SetActive(false);
        }
        MapObject.MapInstance.GetComponent<MapObject>().HideMap();
        OnNewStageStarted?.Invoke();
    }
}
