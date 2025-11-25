using System;
using UnityEngine;
using UnityEngine.UI;

public class MainManuUI : MonoBehaviour
{
    [Header("Buttons in Scen")]
    [SerializeField] private Button btnStart;
    [SerializeField] private Button btnNewgame;

    [Header("Title screen managar")]
    [SerializeField] private TitleScreenManager titleScreenManager;

    Action onActionStart;
    Action onActionNewgame;


    void Start()
    {
        btnStart.onClick.RemoveAllListeners();
        btnNewgame.onClick.RemoveAllListeners();

        btnStart.onClick.AddListener(() => PressStart());
        btnNewgame.onClick.AddListener(() => PressNewGame());

    }

    private void PressStart() 
    {
        btnStart.gameObject.SetActive(false);
        titleScreenManager.StartNetworkAsHost();
        btnNewgame.gameObject.SetActive(true);
    }

    private void PressNewGame() 
    {
        btnNewgame.gameObject.SetActive(false);
        titleScreenManager.StartNewGame();
    }

}
