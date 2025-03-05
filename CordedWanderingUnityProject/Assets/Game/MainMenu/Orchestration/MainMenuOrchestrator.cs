using Game.MainMenu.ConnectionScreen;
using UnityEngine;

namespace Game.MainMenu.Orchestration
{
    public class MainMenuOrchestrator : MonoBehaviour
    {
        [SerializeField]
        private ConnectionScreenHandler m_connectionScreenHandler;

        private void Awake()
        {
            m_connectionScreenHandler.Initialize();
        }

        private void Start()
        {
            m_connectionScreenHandler.Show();
        }
    }
}