using Game.MainMenu.ScreensSystem;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;

namespace Game.MainMenu.ConnectionScreen
{
    public class ConnectionScreenHandler : AScreenHandler
    {
        [SerializeField]
        private TMP_InputField m_ipAddressField;
        [SerializeField]
        private TMP_InputField m_portField;

        [SerializeField]
        private Button m_connectAsClientButton;
        [SerializeField]
        private Button m_connectAsHostButton;

        protected override void PostShowLogic()
        {
            var networkManager = NetworkManager.Singleton;
            var transport = (networkManager.NetworkConfig.NetworkTransport as UnityTransport);
            m_ipAddressField.text = transport.ConnectionData.Address;
            m_portField.text = transport.ConnectionData.Port.ToString();
            
            m_connectAsClientButton.onClick.AddListener(ConnectAsClient);
            m_connectAsHostButton.onClick.AddListener(ConnectAsHost);
        }

        protected override void PostHideLogic()
        {
            base.PostHideLogic();
            m_connectAsClientButton.onClick.RemoveListener(ConnectAsClient);
            m_connectAsHostButton.onClick.RemoveListener(ConnectAsHost);
        }


        private void ConnectAsClient()
        {
            var networkManager = NetworkManager.Singleton;
            var transport = (networkManager.NetworkConfig.NetworkTransport as UnityTransport);
            transport.ConnectionData.Address = m_ipAddressField.text;
            transport.ConnectionData.Port = ushort.Parse(m_portField.text);
            
            networkManager.StartClient();
        }

        private void ConnectAsHost()
        {
            var networkManager = NetworkManager.Singleton;
            var transport = (networkManager.NetworkConfig.NetworkTransport as UnityTransport);
            transport.ConnectionData.Address = m_ipAddressField.text;
            transport.ConnectionData.ServerListenAddress = m_ipAddressField.text;
            transport.ConnectionData.Port = ushort.Parse(m_portField.text);
            
            networkManager.StartHost();
        }
    }
}