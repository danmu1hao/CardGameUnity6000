using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class UIManager : QuickInstance<UIManager>
{
    public GameObject selectPanel;
    public Task<List<Card>> OpenSelectPanel(List<Card> cards,int selectNum=1)
    {
        Debug.Log("打开界面");
        selectPanel.SetActive(true);
        return selectPanel.GetComponent<SelectPanelManager>().ShowSelectCards(cards);
    }
    public Task<CardEffect> OpenSelectPanel(List<CardEffect> cards,int selectNum=1)
    {
        Debug.Log("打开界面");
        selectPanel.SetActive(true);
        return selectPanel.GetComponent<SelectPanelManager>().ShowSelecCardEffect(cards);
    }

}
