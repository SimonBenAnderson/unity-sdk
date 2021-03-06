﻿using UnityEngine;
using System.Collections;
using IBM.Watson.DeveloperCloud.Utilities;

//! This helper class makes sure the Watson configuration is fully loaded before we try to access any of the services.
public class ConfigLoader : MonoBehaviour
{
    [SerializeField]
    private GameObject m_Prefab = null;
    private GameObject m_CreatedObject = null;

    #region OnEnable / OnDisable - Registering events
    void OnEnable(){
        EventManager.Instance.RegisterEventReceiver(Constants.Event.USER_TO_LOGOUT, OnUserLogOut);
    }

    void OnDisable(){
        EventManager.Instance.UnregisterEventReceiver(Constants.Event.USER_TO_LOGOUT, OnUserLogOut);
    }
    #endregion

	// Use this for initialization
	IEnumerator Start ()
    {
        // wait for the configuration to be loaded first..
        while (!Config.Instance.ConfigLoaded)
            yield return null;
        	
        // then initiate a prefab after we are done loading the config.
        m_CreatedObject = GameObject.Instantiate( m_Prefab );
	}

    public void OnUserLogOut(System.Object[] args)
    {
        if (m_CreatedObject != null)
        {
            if (!m_CreatedObject.activeSelf)
                m_CreatedObject.SetActive(true);
            
            m_CreatedObject.SendMessage("DestroyCreatedObject", SendMessageOptions.DontRequireReceiver);
        }
        StartCoroutine(Start());
    }
	

}
