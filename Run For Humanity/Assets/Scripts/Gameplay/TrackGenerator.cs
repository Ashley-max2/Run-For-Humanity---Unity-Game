using System.Collections.Generic;
using UnityEngine;

namespace RunForHumanity.Gameplay
{
    /// <summary>
    /// Genera segmentos de track infinitos con dificultad progresiva.
    /// Los objetos (monedas, obstáculos, power-ups) deben estar colocados manualmente en cada prefab de track.
    /// </summary>
    public class TrackGenerator : MonoBehaviour
    {
        [Header("Track Settings")]
        [Tooltip("Array de 4 prefabs: Easy, Medium, Hard, Extreme")]
        public GameObject[] trackPrefabs;
        public float trackLength = 20f;
        public int initialSegments = 5;
        public Transform playerTransform;

        [Header("Difficulty")]
        [Range(0, 1)]
        public float difficultyProgress = 0f; // 0 = fácil, 1 = extremo
        public bool autoIncreaseDifficulty = true;
        public float difficultyIncreaseRate = 0.01f; // Por segundo

        [Header("Safe Zone")]
        [Tooltip("Primeros segmentos siempre fáciles")]
        public int safeSegments = 2;

        private List<GameObject> activeTracks = new List<GameObject>();
        private Queue<GameObject> inactiveTracks = new Queue<GameObject>();
        private float spawnZ = 0f;
        private float safeZone = 30f; // Distance behind player before recycling
        private int segmentCount = 0;
        private int lastTrackIndex = -1; // Para evitar repetir extremos consecutivos

        void Start()
        {
            if (playerTransform == null && GameObject.FindGameObjectWithTag("Player"))
            {
                playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            }

            // Pre-instantiate some tracks for pooling
            for (int i = 0; i < 3; i++)
            {
                foreach (GameObject prefab in trackPrefabs)
                {
                    GameObject track = Instantiate(prefab, transform);
                    track.SetActive(false);
                    inactiveTracks.Enqueue(track);
                }
            }

            // Spawn initial segments
            for (int i = 0; i < initialSegments; i++)
            {
                SpawnTrack();
            }

            Debug.Log($"[TrackGenerator] Initialized with {trackPrefabs.Length} track types, pool size: {inactiveTracks.Count}");
        }

        void Update()
        {
            // Auto increase difficulty
            if (autoIncreaseDifficulty)
            {
                difficultyProgress += difficultyIncreaseRate * Time.deltaTime;
                difficultyProgress = Mathf.Clamp01(difficultyProgress);
            }

            // Check if we need to spawn new track
            if (playerTransform != null)
            {
                if (playerTransform.position.z > spawnZ - (initialSegments * trackLength))
                {
                    SpawnTrack();
                }

                // Recycle old tracks
                RecycleOldTracks();
            }
        }

        void SpawnTrack()
        {
            GameObject trackPrefab = GetTrackPrefab();
            GameObject track;

            // Try to get from pool
            if (inactiveTracks.Count > 0)
            {
                track = inactiveTracks.Dequeue();
                // Si el prefab en pool no coincide con el que necesitamos, destruirlo y crear uno nuevo
                if (track.name.Replace("(Clone)", "").Trim() != trackPrefab.name)
                {
                    Destroy(track);
                    track = Instantiate(trackPrefab, transform);
                }
            }
            else
            {
                track = Instantiate(trackPrefab, transform);
            }

            // Position and activate
            track.transform.position = new Vector3(0, 0, spawnZ);
            track.SetActive(true);
            activeTracks.Add(track);

            spawnZ += trackLength;
            segmentCount++;

            Debug.Log($"[TrackGenerator] Spawned track: {trackPrefab.name} at Z={spawnZ}, Difficulty: {difficultyProgress:F2}");
        }

        GameObject GetTrackPrefab()
        {
            if (trackPrefabs == null || trackPrefabs.Length == 0)
            {
                Debug.LogError("[TrackGenerator] No track prefabs assigned!");
                return null;
            }

            // Si estamos en la zona segura, solo usar tracks fáciles
            if (segmentCount < safeSegments)
            {
                return trackPrefabs[0]; // Easy
            }

            // Seleccionar prefab basado en dificultad
            int index = GetTrackPrefabIndex();
            return trackPrefabs[index];
        }

        int GetTrackPrefabIndex()
        {
            // Asumiendo 4 prefabs: Easy, Medium, Hard, Extreme
            if (trackPrefabs.Length != 4)
            {
                Debug.LogWarning($"[TrackGenerator] Expected 4 track prefabs, got {trackPrefabs.Length}");
                return Random.Range(0, trackPrefabs.Length);
            }

            // Distribución basada en dificultad con variación aleatoria
            float rand = Random.value;
            int newIndex;
            
            if (difficultyProgress < 0.25f)
            {
                // Dificultad baja: mayormente Easy/Medium
                newIndex = rand < 0.7f ? 0 : 1; // 70% Easy, 30% Medium
            }
            else if (difficultyProgress < 0.5f)
            {
                // Dificultad media: Medium/Hard
                newIndex = rand < 0.6f ? 1 : 2; // 60% Medium, 40% Hard
            }
            else if (difficultyProgress < 0.75f)
            {
                // Dificultad alta: Hard con algo de Extreme
                newIndex = rand < 0.7f ? 2 : 3; // 70% Hard, 30% Extreme
            }
            else
            {
                // Dificultad extrema: mayormente Extreme
                newIndex = rand < 0.8f ? 3 : 2; // 80% Extreme, 20% Hard
            }
            
            // Evitar 2 extremos (índice 3) consecutivos
            if (newIndex == 3 && lastTrackIndex == 3)
            {
                newIndex = 2; // Cambiar a Hard
            }
            
            lastTrackIndex = newIndex;
            return newIndex;
        }

        void RecycleOldTracks()
        {
            if (activeTracks.Count == 0) return;

            // Check first track (oldest)
            GameObject firstTrack = activeTracks[0];
            float trackEndZ = firstTrack.transform.position.z + trackLength;

            if (playerTransform.position.z > trackEndZ + safeZone)
            {
                // Recycle this track
                RecycleTrack(firstTrack);
                activeTracks.RemoveAt(0);
            }
        }

        void RecycleTrack(GameObject track)
        {
            track.SetActive(false);
            inactiveTracks.Enqueue(track);
            Debug.Log($"[TrackGenerator] Recycled track: {track.name}");
        }

        public void SetDifficulty(float difficulty)
        {
            difficultyProgress = Mathf.Clamp01(difficulty);
            Debug.Log($"[TrackGenerator] Difficulty set to: {difficultyProgress:F2}");
        }

        // Editor Gizmos
        void OnDrawGizmos()
        {
            if (playerTransform != null)
            {
                // Draw safe zone
                Gizmos.color = Color.red;
                Vector3 safeZonePos = playerTransform.position - new Vector3(0, 0, safeZone);
                Gizmos.DrawWireCube(safeZonePos, new Vector3(10, 2, 1));
            }
        }
    }
}
