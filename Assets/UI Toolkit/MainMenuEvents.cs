using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuEvents : MonoBehaviour
{
    private UIDocument uiDocuments;
    private Button btn;

    private List<Button> menuButtons = new List<Button>();
    void Awake()
    {
        uiDocuments = GetComponent<UIDocument>();
        btn = uiDocuments.rootVisualElement.Q("StartBtn") as Button;   //Q is for query
        btn.RegisterCallback<ClickEvent>(OnPlayGameClick);

        menuButtons = uiDocuments.rootVisualElement.Query<Button>().ToList(); // for multiple button
        for (int i = 0; i < menuButtons.Count; i++)
        {
            menuButtons[i].RegisterCallback<ClickEvent>(BtnClickSound);
        }
    }

    private void OnDisable()
    {
        btn.UnregisterCallback<ClickEvent>(OnPlayGameClick);
        for (int i = 0; i < menuButtons.Count; i++)
        {
            menuButtons[i].UnregisterCallback<ClickEvent>(BtnClickSound);
        }
    }
    void OnPlayGameClick(ClickEvent evnt)
    {
        Debug.Log("Click!");
    }
    void BtnClickSound(ClickEvent sndEvnt)
    {
        Debug.Log("Play Sound");
    }
}
