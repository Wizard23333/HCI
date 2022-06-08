/*==============================================================================
Copyright (c) 2017 PTC Inc. All Rights Reserved.

Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using UnityEngine;
using Vuforia;

/// <summary>
/// A custom handler that implements the ITrackableEventHandler interface.
///
/// Changes made to this file could be overwritten when upgrading the Vuforia version.
/// When implementing custom event handler behavior, consider inheriting from this class instead.
/// </summary>
public class MyDefaultTrackableEventHandler : MonoBehaviour, ITrackableEventHandler
{
    #region PROTECTED_MEMBER_VARIABLES

    public GameObject Toon_Chicken_Prefab;
    public GameObject Toon_Chick_Prefab;
    public GameObject RFX_HitBlood3;
    private GameObject chicken;
    private AudioSource audioSource;
    public AudioClip chickSound;
    private bool m_source = false;

    protected TrackableBehaviour mTrackableBehaviour;
    protected TrackableBehaviour.Status m_PreviousStatus;
    protected TrackableBehaviour.Status m_NewStatus;

    #endregion // PROTECTED_MEMBER_VARIABLES

    #region UNITY_MONOBEHAVIOUR_METHODS

    protected virtual void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
            mTrackableBehaviour.RegisterTrackableEventHandler(this);

        audioSource = GetComponent<AudioSource>();
        m_source = true;
    }

    protected virtual void OnDestroy()
    {
        if (mTrackableBehaviour)
            mTrackableBehaviour.UnregisterTrackableEventHandler(this);
    }

    #endregion // UNITY_MONOBEHAVIOUR_METHODS

    #region PUBLIC_METHODS

    /// <summary>
    ///     Implementation of the ITrackableEventHandler function called when the
    ///     tracking state changes.
    /// </summary>
    public void OnTrackableStateChanged(
        TrackableBehaviour.Status previousStatus,
        TrackableBehaviour.Status newStatus)
    {
        m_PreviousStatus = previousStatus;
        m_NewStatus = newStatus;

        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
            OnTrackingFound();
        }
        else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
                 newStatus == TrackableBehaviour.Status.NO_POSE)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
            OnTrackingLost();
        }
        else
        {
            // For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
            // Vuforia is starting, but tracking has not been lost or found yet
            // Call OnTrackingLost() to hide the augmentations
            OnTrackingLost();
        }
    }

    #endregion // PUBLIC_METHODS

    #region PROTECTED_METHODS

    protected virtual void OnTrackingFound()
    {
        // 获取鸡的数值
        if (!PlayerPrefs.HasKey("chicken_value"))
        {
              PlayerPrefs.SetInt("chicken_value", 100);
        }
        int chickenValue = PlayerPrefs.GetInt("chicken_value");

        Debug.Log(PlayerPrefs.GetInt("chicken_value"));


        if(chickenValue >= 200)
        {
            chicken = GameObject.Instantiate(Toon_Chicken_Prefab, new Vector3(0, 1, 0), Quaternion.Euler(new Vector3(0, 180, 0)), transform);
        } else
        {
            chicken = GameObject.Instantiate(Toon_Chick_Prefab, new Vector3(0, 1, 0), Quaternion.Euler(new Vector3(0, 180, 0)), transform);
        }
        chicken.name = "longChicken(Clone)";


        Debug.Log(chicken.transform.localScale);
        chicken.transform.localScale = ((float)chickenValue / 100) * new Vector3(1f, 1f, 1f);

        GameObject effect1 = GameObject.Instantiate(RFX_HitBlood3, transform.position, Quaternion.Euler(new Vector3(0, 180, 0)), this.transform);
        Destroy(effect1, 2.0f);

        audioSource.Play();

    }


    protected virtual void OnTrackingLost()
    {
        Destroy(chicken);
        Destroy(GameObject.Find("food"));

        if (m_source)
        {
            audioSource.Pause();
        }
        
    }

    #endregion // PROTECTED_METHODS

    public void ReloadModel()
    {
        OnTrackingLost();
        OnTrackingFound();
    }

}
