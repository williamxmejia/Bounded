using UnityEngine;

public class MainMenuButtonManager : MonoBehaviour
{
    [SerializeField] MainMenuManager.MainMenuButtons _buttonType;
    public void ButtonClicked()
    {
        MainMenuManager._.MainMenuClicked(_buttonType);
    }
}
