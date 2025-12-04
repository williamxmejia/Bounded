using UnityEngine;

public class MainMenuButtonManager : MonoBehaviour
{
    [SerializeField] MainMenuManager.MainMenuButtons _buttonType;
    [SerializeField] public AudioSource _audioSource;

    public void ButtonClicked()
    {
        if (_audioSource != null)
        {
            _audioSource.Play();
        }
        MainMenuManager._.MainMenuClicked(_buttonType);
    }
}
