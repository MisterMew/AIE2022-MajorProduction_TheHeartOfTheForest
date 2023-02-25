/*
 * Date Created: 01.11.2022
 * Author: Nghia Do
 * Contributors: 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotF
{
    /// <summary>
    /// Controls dissolve shaders
    /// </summary>
    public class DissovleController : MonoBehaviour
    {
        // Dissolve type [ture if meshes only have one material]
        [Tooltip("Dissolve type[ture if meshes only have one material]")]
        [SerializeField] private bool dissovleTypeOld = false;

        [Tooltip("Renderers with single dissolve shader")]
        [SerializeField] GameObject gos;
        [SerializeField] Renderer[] renderers;

        [Tooltip("Materials of renderes with two dissolve shaders")]
        [SerializeField] private Material m_grassMat01;
        [SerializeField] private Material m_grassMat02;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// Setup initial values of disolve shader
        /// /// </summary>
        /// <param name="initValue">Initial value of dissolve amount</param>
        public void Setup(float initValue)
        {
            if (dissovleTypeOld)
            {
                renderers = gos.GetComponentsInChildren<Renderer>();

                for (int idx = 0; idx < renderers.Length; idx++)
                {
                    renderers[idx].sharedMaterial.SetFloat("DissolveAmount_", initValue);
                }
            }
            else
            {
                m_grassMat01.SetFloat("DissolveAmount_", initValue);
                m_grassMat02.SetFloat("DissolveAmount_", initValue);
            }
        }

        public void DisolveOut(float time) => StartCoroutine(DissolveOut(time));
        public void DisolveIn(float time) => StartCoroutine(DissolveIn(time));

        /// <summary>
        /// DissolveOut coroutine
        /// </summary>
        /// <param name="time">Time taken to completely dissolve</param>
        /// <returns></returns>
        IEnumerator DissolveOut(float time)
        {
            float timer = 0;

            while (timer <= time)
            {
                if (dissovleTypeOld)
                {
                    for (int idx = 0; idx < renderers.Length; idx++)
                    {
                        renderers[idx].material.SetFloat("DissolveAmount_", Mathf.Lerp(-1, 1, timer / time));
                    }
                }
                else
                {
                    m_grassMat01?.SetFloat("DissolveAmount_", Mathf.Lerp(-1, 1, timer / time));
                    m_grassMat02?.SetFloat("DissolveAmount_", Mathf.Lerp(-1, 1, timer / time));
                }

                yield return null;
                timer += Time.deltaTime;
            }
            if (dissovleTypeOld)
            {
                for (int idx = 0; idx < renderers.Length; idx++)
                {
                    renderers[idx].sharedMaterial.SetFloat("DissolveAmount_", 1);
                }
            }
            else
            {
                m_grassMat01?.SetFloat("DissolveAmount_", 1);
                m_grassMat02?.SetFloat("DissolveAmount_", 1);
            }
        }

        /// <summary>
        /// DissolveIn coroutine
        /// </summary>
        /// <param name="time">Time taken to completely dissolve</param>
        /// <returns></returns>
        IEnumerator DissolveIn(float time)
        {
            float timer = 0;

            while (timer <= time)
            {
                if (dissovleTypeOld)
                {
                    for (int idx = 0; idx < renderers.Length; idx++)
                    {
                        renderers[idx].sharedMaterial.SetFloat("DissolveAmount_", Mathf.Lerp(1, -1, timer / time));
                    }
                }
                else
                {
                    m_grassMat01?.SetFloat("DissolveAmount_", Mathf.Lerp(1, -1, timer / time));
                    m_grassMat02?.SetFloat("DissolveAmount_", Mathf.Lerp(1, -1, timer / time));
                }

                yield return null;
                timer += Time.deltaTime;
            }

            if (dissovleTypeOld)
            {
                for (int idx = 0; idx < renderers.Length; idx++)
                {
                    renderers[idx].sharedMaterial.SetFloat("DissolveAmount_", -1);
                }
            }
            else
            {
                m_grassMat01?.SetFloat("DissolveAmount_", -1);
                m_grassMat02?.SetFloat("DissolveAmount_", -1);
            }
        }
    }
}