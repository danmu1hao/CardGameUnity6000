using UnityEngine;

namespace UIFrame
{
    public abstract class BasePanel
    {
        public UIType UIType;
        public GameObject panel;
        public BasePanel(UIType uitype)
        {
            this.UIType = uitype;
        }
        public virtual void OnStart()
        {
            Debug.Log("start use");
        }
        public virtual void OnEnable()
        {
        
        }
        public virtual void OnDisable()
        {
        
        }

        public virtual void OnDestroy()
        {
        
        }
    }

    
}
