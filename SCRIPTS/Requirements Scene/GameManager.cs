using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class GameManager : MonoBehaviourPunCallbacks, IPunObservable
{   
   
    public List<ShipInfo> allStatus = new List<ShipInfo>();
    public ShipCanvas shipCanvas;
    public GameState gameState;

    //CO-Components
    private NetworkSettings networkSettings;

    public void Start(){
        shipCanvas = GameObject.FindGameObjectWithTag("ShipCanvas").GetComponent<ShipCanvas>();
        networkSettings = GetComponent<NetworkSettings>();
        networkSettings.Init();
        
        foreach (var item in PhotonNetwork.PlayerList)
        {   
            var player = new ShipInfo();
                player.PLayerName = item.NickName;
                player.PlayerStatus = true;
            allStatus.Add(player);
        }

        //CountDownState
        gameState = GameState.countDown;
        StartCoroutine(EnableCountDown(1f));
        shipCanvas.DisableOrEnableUI_inScene(0f,0f);



        UpdateGameStateInScene();
        StartCoroutine(DisplayAllPlayersInList());
    }

    [PunRPC]
    public void Test(){
        //allStatus.Clear();
        var newList = new List<ShipInfo>();
        foreach (var item in PhotonNetwork.PlayerList){
            ShipInfo currentInfo = allStatus.Find(x => x.PLayerName == item.NickName);
            newList.Add(currentInfo);
        }

        allStatus.Clear();
        allStatus = newList;

        ResetDisplayList();
        StartCoroutine(DisplayAllPlayersInList());
    }

    IEnumerator  DisplayAllPlayersInList(){
        
        yield return new WaitForSeconds(0.2f);

        foreach (var status in allStatus){
            GameObject go = Instantiate(shipCanvas.playerInfo);
            PlayerInfoContainer info = go.GetComponentInChildren<PlayerInfoContainer>();
            info.playerName.text = status.PLayerName;

            if(status.PlayerStatus == false){
                info.alive.gameObject.SetActive(false);
                info.dead.gameObject.SetActive(true);
            }else{
                info.dead.gameObject.SetActive(false);
                info.alive.gameObject.SetActive(true);
            }   
                
            go.transform.SetParent(shipCanvas.playerListParent,false);
            go.transform.localPosition = new Vector3(97.2905f, -17.5f, 0f);
        }
    }

    public List<GameObject> allTestGO = new List<GameObject>(); 

    private void ResetDisplayList(){
        var allChild = shipCanvas.playerListParent.childCount;
        for (int i = 0; i < allChild; i++){
            //allTestGO.Add(shipCanvas.playerListParent.GetChild(i).gameObject);
            Destroy(shipCanvas.playerListParent.GetChild(i).gameObject);
        }
    }

    [PunRPC]
    public void UpdateShipDisplayStatus(string name){
        
        foreach (var item in allStatus){
            if(item.PLayerName == name){
                item.PlayerStatus = false;
            }
        }

        var allChild = shipCanvas.playerListParent.childCount;
        for (int i = 0; i < allChild; i++){
            if(allStatus[i].PlayerStatus == false){
                Debug.Log(allStatus[i].PLayerName+"=======Dead");
                PlayerInfoContainer info = shipCanvas.playerListParent.GetChild(i).gameObject.GetComponent<PlayerInfoContainer>();
                info.alive.gameObject.SetActive(false);
                info.dead.gameObject.SetActive(true);
            }else{
                Debug.Log(allStatus[i].PLayerName+"=======Alive");
                PlayerInfoContainer info = shipCanvas.playerListParent.GetChild(i).gameObject.GetComponent<PlayerInfoContainer>();
                info.alive.gameObject.SetActive(true);
                info.dead.gameObject.SetActive(false);
            }
        }
    }

    IEnumerator Countdown(float seconds){
        yield return new WaitForSeconds(seconds);
    }

    [PunRPC]
    public void UpdateGameStateInScene(){
        switch(gameState){
            case GameState.countDown:
                CountDown();
                return;
            case GameState.battle:
                Battle();
                return;
            case GameState.end:
                StartCoroutine(End());
                return;
        }
    }

    IEnumerator StartOfBattle(){
        yield return new WaitForSeconds(0.2f);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) { 
        
    }

    void OnApplicationQuit()
    {
        Debug.Log("Application ending after " + Time.time + " seconds");
    }

    // All States

    ///Countdown STATE
    private void CountDown(){
        StartCoroutine(ChangeUIValueUI(1));
        StartCoroutine(ChangeUIValueUI(2));
        StartCoroutine(ChangeUIValueUI(3));
        StartCoroutine(ChangeUIValueUI(4));
    }

    IEnumerator ChangeUIValueUI(float sec){
        yield return new WaitForSeconds(sec);
        if(sec == 4){
            shipCanvas.countDownNumber.gameObject.SetActive(false);
            gameState = GameState.battle;
            shipCanvas.DisableOrEnableUI_inScene(0.65f,1f);
        }else
            shipCanvas.countDownNumber.text = (4-sec).ToString();
    }

    IEnumerator EnableCountDown(float sec){
        yield return new WaitForSeconds(sec);
        shipCanvas.countDownNumber.gameObject.SetActive(true);
    }

    
    ///Battle STATE
    private void Battle(){
        var countAlive = allStatus.Where(x => x.PlayerStatus == true).Count();
        //var info = allStatus.Where(x => x.PlayerStatus == true).First();

        if(countAlive == 1){
            gameState = GameState.end;
            UpdateGameStateInScene();
           // Debug.Log(info.PLayerName);
        }
    }

    ///End STATE
    IEnumerator End(){
        yield return new WaitForSeconds(1.5f);
        shipCanvas.EnableGameEndUI();
        StartCoroutine(SlowTimeDown());
    }

    IEnumerator SlowTimeDown(){
        yield return new WaitForSeconds(4f);
        Time.timeScale = 0.1f;
    }
    
    
}

public enum GameState{
    countDown,
    battle,
    end
}

[System.Serializable]
public class ShipInfo{
    public string PLayerName;
    public bool PlayerStatus;
}
