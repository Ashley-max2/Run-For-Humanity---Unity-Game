using UnityEngine;
using System.Collections.Generic;

namespace RunForHumanity.Network
{
    // PDF Page 5: "Posiciones de otros jugadores (fantasmas)"
    
    [System.Serializable]
    public class GhostData
    {
        public string playerId;
        public List<Vector3> recordedPositions;
        // Optimization: Use splines or compressed path data instead of raw Vector3 list
    }

    public class MultiplayerGhosts : MonoBehaviour
    {
        [Header("Recording")]
        public bool isRecording = true;
        public float recordInterval = 0.5f;
        private float timer;
        private GhostData currentRunData;

        [Header("Playback")]
        public GameObject ghostPrefab;
        private List<GameObject> activeGhosts = new List<GameObject>();

        void Start()
        {
            currentRunData = new GhostData();
            currentRunData.recordedPositions = new List<Vector3>();
            currentRunData.playerId = "local_player_" + Random.Range(0,9999);
        }

        void Update()
        {
            if (isRecording)
            {
                timer += Time.deltaTime;
                if (timer >= recordInterval)
                {
                    RecordPosition();
                    timer = 0;
                }
            }
        }

        void RecordPosition()
        {
            if (transform.position.y < -10) return; // Don't record falls
            currentRunData.recordedPositions.Add(transform.position);
        }

        public void UploadRun()
        {
            Debug.Log($"Uploading Ghost Run with {currentRunData.recordedPositions.Count} points.");
            // NetworkManager.Upload(currentRunData);
        }

        public void LoadGhosts(List<GhostData> ghosts)
        {
            // Instantiate ghost visualizers that follow the points
        }
    }
}
