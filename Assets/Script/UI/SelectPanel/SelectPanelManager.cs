using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SelectPanelManager : MonoBehaviour
{
    // Start is called before the first frame update\
    public bool TestBool;
    void Start()
    {
        checkButton.onClick.AddListener(ActiveSelectedCard);
        if (TestBool)
        {
            Test();
        }
    }
    
    public void Test()
    {
        List<Card> testCards = new List<Card>();
        for (int i = 0; i < 10; i++)
        {
            Card card=new Card();
            card.cardConfig.name = i.ToString();
            testCards.Add(card);    
        }
        

        ShowSelectCards(testCards);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject effectCardPrefab;

    public Transform cardContainer;
    private TaskCompletionSource<List<Card>> _tcs;
    private readonly List<GameObject> _effectModelList = new();
    
    public Button checkButton;
    public Button cancelButton;
    // 这个只负责让玩家选卡
    public Task<List<Card>> ShowSelectCards(List<Card> cardList,int selectNum=1)
    {
        this._selectNum = selectNum;
        _tcs = new TaskCompletionSource<List<Card>>();

        // 清空旧卡牌
        foreach (var obj in _effectModelList)
            Destroy(obj);
        _effectModelList.Clear();

        // 生成新卡牌
        foreach (var card in cardList)
        {
            GameObject cardModel = Instantiate(effectCardPrefab, cardContainer);
            cardModel.GetComponent<SelectCardDisplay>().Init(card, this); // 点击时回调到 SelectPanelManager
            _effectModelList.Add(cardModel);
        }

        checkButton.onClick.RemoveAllListeners();
        checkButton.onClick.AddListener(ActiveSelectedCard);
        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(CancelSelectedCard);
        
        
        return _tcs.Task; // 返回“还没完成的任务”
    }
    private TaskCompletionSource<CardEffect> _tcs_effect;
    public Task<CardEffect> ShowSelecCardEffect(List<CardEffect> cardEffectsList,int selectNum=1)
    {
        this._selectNum = selectNum;
        _tcs_effect = new TaskCompletionSource<CardEffect>();

        // 清空旧卡牌
        foreach (var obj in _effectModelList)
            Destroy(obj);
        _effectModelList.Clear();

        // 生成新卡牌
        foreach (var cardEffect  in cardEffectsList)
        {
            GameObject cardModel = Instantiate(effectCardPrefab, cardContainer);
            cardModel.GetComponent<SelectCardDisplay>().Init(cardEffect.card, this); // 点击时回调到 SelectPanelManager
            _effectModelList.Add(cardModel);
        }

        checkButton.onClick.RemoveAllListeners();
        checkButton.onClick.AddListener(ActiveSelectedEffect);
        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(CancelSelectedEffect);
        
        
        return _tcs_effect.Task; // 返回“还没完成的任务”
    }

    private int _selectNum = 1; // 最多能选几张，1 就是单选
    private Queue<SelectCardDisplay> _currentSelected = new Queue<SelectCardDisplay>();

    public void SelectCard(SelectCardDisplay selected)
    {
        // 如果已经选过了，再点一下 = 取消选择
        if (_currentSelected.Contains(selected))
        {
            selected.SetActiveState(false);
            // 从队列里移除
            var newQueue = new Queue<SelectCardDisplay>();
            foreach (var s in _currentSelected)
            {
                if (s != selected) newQueue.Enqueue(s);
            }
            _currentSelected = newQueue;
            return;
        }

        // 新选择
        selected.SetActiveState(true);
        _currentSelected.Enqueue(selected);

        // 超过数量限制 → 弹掉最早的
        while (_currentSelected.Count > _selectNum)
        {
            var old = _currentSelected.Dequeue();
            if (old != null)
            {
                old.SetActiveState(false);
            }
        }

        // 打印当前选择
        foreach (var s in _currentSelected)
        {
            Debug.Log($"当前已选: {s.gameObject.name}");
        }
    }


    void ActiveSelectedCard()
    {

        List<Card> result = new List<Card>();
        if (_currentSelected.Count ==  _selectNum)
        {
            for (int i = 0; i < _currentSelected.Count; i++)
            {
                result.Add( _currentSelected.Dequeue().card);
            }
            _tcs?.SetResult(result);
            ClosePanel();
        }

    }

    void CancelSelectedCard()
    {
        List<Card> result = new List<Card>();
        _tcs?.SetResult(result);
        ClosePanel();

    }
    
    void ActiveSelectedEffect()
    {
        //TODO 选择效果而不是卡牌
        CardEffect result = null;
        if (_currentSelected.Count ==  _selectNum)
        {
            result= _currentSelected.Dequeue().card.cardEffectList[0];
            _tcs_effect?.SetResult(result);
            ClosePanel();
        }

    }

    void CancelSelectedEffect()
    {
        _tcs_effect?.SetResult(null);
        ClosePanel();

    }
    void ClosePanel()
    {
        // _tcs?.SetResult(true);
        gameObject.SetActive(false);

        
    }
}
