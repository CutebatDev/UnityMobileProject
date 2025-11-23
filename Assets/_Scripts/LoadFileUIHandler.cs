using UnityEngine;
using UnityEngine.UI;

namespace _Scripts
{
    public class LoadFileUIHandler : MonoBehaviour
    {
        [SerializeField] private Image        screenshot;
        [SerializeField] private DataToSave.GameSessionData gameSessionData;

        public void OnClicked()
        {
        }

        public void Initialize(DataToSave.GameSessionData gameSessionData, Sprite sprite)
        {
            this.gameSessionData = gameSessionData;           
            screenshot.sprite = sprite;
            screenshot.gameObject.SetActive(true);
        }
    }
}