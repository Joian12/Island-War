using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DockUICanvasReferences : MonoBehaviour
{   
    public GameObject container;
    public Button ship1, ship2, ship3;
    public Button select;
    public TextMeshProUGUI playerName, shipNameInUse;

    /// -------------- ///
    public GameObject confirmation;
    public Button yes, no;
    public GameObject successfullUI;


    ///-------------///
    public GameObject blackFade;

    void Start(){
        confirmation.SetActive(false);
        blackFade.SetActive(false);
        container.SetActive(true);
        successfullUI.SetActive(false);
    }

    public void CloseSuccessPanel(){
        successfullUI.SetActive(false);
        container.SetActive(true);
    }

    public void PlayerInfo(string playerName_, string shipNameInUse_){
        this.playerName.text = playerName_;
        this.shipNameInUse.text = shipNameInUse_;
    }
}
