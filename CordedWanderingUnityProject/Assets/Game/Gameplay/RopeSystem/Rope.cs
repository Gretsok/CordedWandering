using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Game.Gameplay.RopeSystem
{
    public class Rope : MonoBehaviour
    {
        [SerializeField] 
        private Joint m_chainLinkPrefab;
        [SerializeField]
        private float m_chainLinkLength = 0.4f;

        [SerializeField]
        private Joint m_firstLink;
        public Joint FirstLink => m_firstLink;
        [SerializeField]
        private Joint m_lastLink;
        public Joint LastLink => m_lastLink;
        
        [SerializeField]
        private float m_ropeLength = 2f;

        private List<Joint> m_instantiatedRopeLinks = new();
            
        [Button]
        public void ComputeRope()
        {
            StartCoroutine(RopeComputingRoutine());
        }

        private IEnumerator RopeComputingRoutine()
        {
            var numberOfLinksFloat = (m_ropeLength / m_chainLinkLength);
            var numberOfLinks = Mathf.RoundToInt(m_ropeLength / m_chainLinkLength);
            Debug.Log($"float {numberOfLinksFloat} | int {numberOfLinks} | floatToInt {(int)numberOfLinksFloat}");
            
            for (int i = m_instantiatedRopeLinks.Count - 1; i >= 0; --i)
            {
                Destroy(m_instantiatedRopeLinks[i].gameObject);
            }
            m_instantiatedRopeLinks.Clear();

            for (int i = 0; i < numberOfLinks; ++i)
            {
                var link = Instantiate(m_chainLinkPrefab, transform);
                m_instantiatedRopeLinks.Add(link);
            }
            yield return null;


            for (int i = 0; i < m_instantiatedRopeLinks.Count; ++i)
            {
                var link = m_instantiatedRopeLinks[i];
                link.autoConfigureConnectedAnchor = i != 0;
                link.transform.position = m_firstLink.transform.position + -m_firstLink.transform.up * (m_chainLinkLength * i);
            }
            m_lastLink.transform.position = m_firstLink.transform.position +
                                            -m_firstLink.transform.up *
                                            (m_chainLinkLength * m_instantiatedRopeLinks.Count);
            
            yield return null;
            
            for (int i = 0; i < m_instantiatedRopeLinks.Count; ++i)
            {
                var link = m_instantiatedRopeLinks[i];
                var previousLink = i > 0 ? m_instantiatedRopeLinks[i - 1] : m_firstLink;
                link.autoConfigureConnectedAnchor = i != 0;
                link.connectedBody = previousLink.GetComponent<Rigidbody>();
            }

            m_lastLink.connectedBody = m_instantiatedRopeLinks[^1].GetComponent<Rigidbody>();


            yield return new WaitForSeconds(0.5f);
            
            for (int i = 0; i < m_instantiatedRopeLinks.Count; ++i)
            {
                var link = m_instantiatedRopeLinks[i];
                
                link.GetComponent<Rigidbody>().isKinematic = false;
            }
        }

        [Button]
        public void DestroyRope()
        {
            for (int i = m_instantiatedRopeLinks.Count - 1; i >= 0; --i)
            {
                var link = m_instantiatedRopeLinks[i];
                Destroy(m_instantiatedRopeLinks[i].gameObject);
            }
            m_instantiatedRopeLinks.Clear();
        }
        
        private void OnDestroy()
        {
            if (m_firstLink)
                Destroy(m_firstLink.gameObject);
            if (m_lastLink)
                Destroy(m_lastLink.gameObject);
        }
    }
}