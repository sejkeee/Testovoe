using Game;
using UnityEngine;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Transform GrayBG;
        [SerializeField] private Transform StartWindow;

        [SerializeField] private UnityEngine.UI.Text Score;

        private Transform currentWindow;

        private void Awake()
        {
            StartGame.ShowStart?.AddListener(() => OpenWindow(StartWindow));
            StartGame.CloseStart?.AddListener(() => CloseWindow(StartWindow));
            MainManager.ScoreChanged?.AddListener((val) => Score.text = val.ToString());
        }
        
        private void OpenWindow(Transform window)
        {
            if(currentWindow != null)
                CloseCurrentWindow();
            
            GrayBG.gameObject.SetActive(true);
            window.gameObject.SetActive(true);
        }
        
        private void CloseCurrentWindow()
        {
            GrayBG.gameObject.SetActive(false);
            currentWindow.gameObject.SetActive(false);
        }
        
        private void CloseWindow(Transform window)
        {
            GrayBG.gameObject.SetActive(false);
            window.gameObject.SetActive(false);
        }
    }
}