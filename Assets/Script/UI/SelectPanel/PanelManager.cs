using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    public GameObject selectPanel;
    public GameObject cardPrefab;
    public Button selectButton;
    private void Start()
    {

        HideAllPanels();
    }
    public void ShowSelectPanel()
    {
        HideAllPanels();
        selectPanel.SetActive(true);
    }
    public void ShowBattlePanel()
    {
        HideAllPanels();

    }
    public void ShowCardPanel()
    {

    }
    private void HideAllPanels()
    {
        selectPanel.SetActive(false);

    }
}