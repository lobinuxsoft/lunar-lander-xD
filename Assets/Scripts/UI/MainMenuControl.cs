using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class MainMenuControl : MonoBehaviour
{ 
    [SerializeField] private string gameplaySceneName;

    [SerializeField] private string matyUrl;
    [SerializeField] private string juanUrl;
    
    private VisualElement root;

    private Label versionLabel;
    
    private Button startButton;
    private Button quitButton;
    
    private Button linkMatias;
    private Button linkJuan;
    
    private void Awake()
    {
            root = GetComponent<UIDocument>().rootVisualElement;

        versionLabel = root.Q<Label>("game-version");
        versionLabel.text = $"version {Application.version}";
        
        startButton = root.Q<Button>("start-button");
        startButton.clicked += StartGame;

        quitButton = root.Q<Button>("quit-button");

#if UNITY_WEBGL
        quitButton.style.visibility = Visibility.Hidden;
#else
        quitButton.clicked += QuitGame;
#endif
        
        linkMatias = root.Q<Button>("linkedin-matias");
        linkMatias.clicked += () => OpenUrl(matyUrl);
        
        linkJuan = root.Q<Button>("linkedin-juan");
        linkJuan.clicked += () => OpenUrl(juanUrl);
        
        startButton.Focus();
    }

    private void StartGame() => SceneManager.LoadScene(gameplaySceneName);

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    } 

    private void OpenUrl(string url) => Application.OpenURL(url);
}