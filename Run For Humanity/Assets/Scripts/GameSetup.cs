using UnityEngine;

/// <summary>
/// Helper script to setup the GameManager hierarchy.
/// Attach this to an empty GameObject called "GameManager" in the MainMenu scene.
/// This script will automatically add all required manager components.
/// </summary>
public class GameSetup : MonoBehaviour
{
    private void Awake()
    {
        // Ensure this GameObject is the GameManager singleton
        var gameManager = gameObject.GetComponent<RunForHumanity.Core.GameManager>();
        if (gameManager == null)
        {
            gameManager = gameObject.AddComponent<RunForHumanity.Core.GameManager>();
        }

        // Add all systems as child GameObjects or components
        SetupManagers();
        
        // This script is no longer needed after setup
        Destroy(this);
    }

    private void SetupManagers()
    {
        // Add AudioManager
        if (GetComponentInChildren<RunForHumanity.Systems.AudioManager>() == null)
        {
            var audioObj = new GameObject("AudioManager");
            audioObj.transform.SetParent(transform);
            audioObj.AddComponent<RunForHumanity.Systems.AudioManager>();
        }

        // Add ParticleManager
        if (GetComponentInChildren<RunForHumanity.Systems.ParticleManager>() == null)
        {
            var particleObj = new GameObject("ParticleManager");
            particleObj.transform.SetParent(transform);
            particleObj.AddComponent<RunForHumanity.Systems.ParticleManager>();
        }

        // Add UIManager
        if (GetComponentInChildren<RunForHumanity.UI.UIManager>() == null)
        {
            var uiObj = new GameObject("UIManager");
            uiObj.transform.SetParent(transform);
            uiObj.AddComponent<RunForHumanity.UI.UIManager>();
        }

        // Add InputManager
        if (GetComponentInChildren<RunForHumanity.Systems.InputManager>() == null)
        {
            var inputObj = new GameObject("InputManager");
            inputObj.transform.SetParent(transform);
            inputObj.AddComponent<RunForHumanity.Systems.InputManager>();
        }

        // Add SensorManager
        if (GetComponentInChildren<RunForHumanity.Systems.SensorManager>() == null)
        {
            var sensorObj = new GameObject("SensorManager");
            sensorObj.transform.SetParent(transform);
            sensorObj.AddComponent<RunForHumanity.Systems.SensorManager>();
        }

        // Add DonationSystem
        if (GetComponentInChildren<RunForHumanity.Systems.DonationSystem>() == null)
        {
            var donationObj = new GameObject("DonationSystem");
            donationObj.transform.SetParent(transform);
            donationObj.AddComponent<RunForHumanity.Systems.DonationSystem>();
        }

        // Add AdManager
        if (GetComponentInChildren<RunForHumanity.Systems.AdManager>() == null)
        {
            var adObj = new GameObject("AdManager");
            adObj.transform.SetParent(transform);
            adObj.AddComponent<RunForHumanity.Systems.AdManager>();
        }

        // Add OrientationManager
        if (GetComponentInChildren<RunForHumanity.Systems.OrientationManager>() == null)
        {
            var orientationObj = new GameObject("OrientationManager");
            orientationObj.transform.SetParent(transform);
            orientationObj.AddComponent<RunForHumanity.Systems.OrientationManager>();
        }

        Debug.Log("[GameSetup] All managers have been set up successfully!");
    }
}
