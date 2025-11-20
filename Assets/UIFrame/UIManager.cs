using System.Collections.Generic;
using UnityEngine;

namespace UIFrame
{
    public class UIManager : QuickInstance<UIManager>
    {
        public Dictionary<string, GameObject> dict_UIObj;
        public Stack<BasePanel> stack_ui;
        public GameObject CanvasObj;
        public void GetCanvasObj()
        {
            CanvasObj = GameObject.Find("Canvas");
        }
        
        public GameObject GetSingleObject(UIType uitype)
        {
            if (dict_UIObj.ContainsKey(uitype.Name))
            {
                return dict_UIObj[uitype.Name];
            }

            if (CanvasObj == null)
            {
                Debug.LogWarning("NO CANVAS");
                return null;
            }
            GameObject ui_obj = GameObject.Instantiate<GameObject>
                (Resources.Load<GameObject>(uitype.Path),CanvasObj.transform);

            return ui_obj;
            Debug.LogWarning("NO SUCH UI");
            return null;
        }

        public void Push(BasePanel panel)
        {
            Debug.Log("PUSH"+panel.UIType.Name);
            if (stack_ui.Count > 0)
            {
               stack_ui.Peek().OnDisable(); 
            }
            GameObject ui_obj = GetSingleObject(panel.UIType);
            dict_UIObj.Add(panel.UIType.Name,ui_obj);
            
            if(stack_ui.Count == 0)
            {
                stack_ui.Push(panel);
            }
            else
            {
                if (stack_ui.Peek().UIType.Name!=panel.UIType.Name)
                {
                    stack_ui.Push(panel);   
                }
            }
            panel.OnStart();
            
            //panel.OnEnable()
        }

        public void Pop(bool isload)
        {
            if (isload == true)
            {
                if(stack_ui.Count > 0)
                {
                    stack_ui.Peek().OnDisable();
                    stack_ui.Peek().OnDestroy();
                    GameObject.Destroy(dict_UIObj[stack_ui.Peek().UIType.Name]);
                    stack_ui.Pop();
                    // 递归弹出？
                    Pop(true);
                }
            }
            else
            {
                if (stack_ui.Count > 0)
                {
                    stack_ui.Peek().OnDisable();
                    stack_ui.Peek().OnDestroy();
                    GameObject.Destroy(dict_UIObj[stack_ui.Peek().UIType.Name]);
                    stack_ui.Pop();
                    if (stack_ui.Count > 0)
                    {
                        stack_ui.Peek().OnEnable();
                    }
                    
                }
            }
        }
    }
}
