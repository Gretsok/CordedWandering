using TMPro;
using UnityEngine;

namespace Game.RuntimeDebugTools.Logs
{
    public class LogConsole : MonoBehaviour
    {
        public static void Log(string a_message)
        {
            Debug.Log(a_message);

            var log = Instantiate(Instance.m_logTextPrefab, Instance.m_logContainer);
            log.text = $"<color=white>{GetTime()} : {a_message}</color>";
        }

        public static void LogError(string a_message)
        {
            Debug.LogError(a_message);
            
            var log = Instantiate(Instance.m_logTextPrefab, Instance.m_logContainer);
            log.text = $"<color=red>{GetTime()} : {a_message}</color>";
        }

        public static void LogWarning(string a_message)
        {
            Debug.LogWarning(a_message);
            
            var log = Instantiate(Instance.m_logTextPrefab, Instance.m_logContainer);
            log.text = $"<color=yellow>{GetTime()} : {a_message}</color>";
        }

        public static string GetTime()
        {
            return $"{System.DateTime.Now.Hour:00}:{System.DateTime.Now.Minute:00}:{System.DateTime.Now.Second:00}";
        }
        
        private static LogConsole s_instance;

        public static LogConsole Instance
        {
            get
            {
                if (s_instance == null)
                { 
                    s_instance = Instantiate(Resources.Load<LogConsole>("LogConsole"));
                    s_instance.m_root.SetActive(false);
                    DontDestroyOnLoad(s_instance);
                }
                return s_instance;
            }
        }

        [SerializeField]
        private GameObject m_root;
        [SerializeField]
        private Transform m_logContainer;
        [SerializeField]
        private TMP_Text m_logTextPrefab;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                ToggleConsole();
            }
        }

        private void ToggleConsole()
        {
            m_root.SetActive(!m_root.activeSelf);
        }
    }
}