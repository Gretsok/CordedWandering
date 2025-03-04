using UnityEngine;

namespace Game.MainMenu.ScreensSystem
{
    public abstract class AScreenHandler : MonoBehaviour
    {
        public bool IsShown { get; private set; } = false;

        public void Initialize()
        {
            Hide(true);
        }

        public void Show()
        {
            if (IsShown)
                return;
            
            ProcessShow();
            IsShown = true;
            
            PostShowLogic();
        }

        protected virtual void ProcessShow()
        {
            gameObject.SetActive(true);
        }

        protected virtual void PostShowLogic()
        { }
        
        public void Hide(bool a_force = false)
        {
            bool hideForced = false;
            if (!IsShown)
            {
                if (!a_force)
                    return;
                else
                    hideForced = true;
            }
            
            ProcessHide();  
            IsShown = false;
            
            if (!hideForced)
                PostHideLogic();
        }

        protected virtual void ProcessHide()
        {
            gameObject.SetActive(false);
        }
        
        protected virtual void PostHideLogic()
        { }
    }
}