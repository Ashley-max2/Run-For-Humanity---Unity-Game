using UnityEngine;
using System.Collections.Generic;
using RunForHumanity.Core;

namespace RunForHumanity.Systems
{
    /// <summary>
    /// Particle effects management system
    /// SOLID: Single Responsibility - Manages all particle effects
    /// </summary>
    public class ParticleManager : MonoBehaviour, IInitializable
    {
        [System.Serializable]
        public class ParticleEffect
        {
            public string name;
            public ParticleSystem prefab;
            public bool attachToParent;
            public float lifetime = 2f;
        }
        
        [Header("Effect Library")]
        [SerializeField] private List<ParticleEffect> _effects = new List<ParticleEffect>();
        
        [Header("Pooling")]
        [SerializeField] private int _poolSize = 20;
        
        private Dictionary<string, ParticleEffect> _effectDict = new Dictionary<string, ParticleEffect>();
        private Dictionary<string, Queue<ParticleSystem>> _particlePools = new Dictionary<string, Queue<ParticleSystem>>();
        
        public bool IsInitialized { get; private set; }
        
        public void Initialize()
        {
            // Build effect dictionary
            foreach (var effect in _effects)
            {
                if (!_effectDict.ContainsKey(effect.name))
                {
                    _effectDict.Add(effect.name, effect);
                    _particlePools[effect.name] = new Queue<ParticleSystem>();
                    
                    // Pre-instantiate pool
                    for (int i = 0; i < _poolSize; i++)
                    {
                        CreatePooledParticle(effect);
                    }
                }
            }
            
            ServiceLocator.RegisterService(this);
            IsInitialized = true;
            Debug.Log($"[ParticleManager] Initialized with {_effectDict.Count} effects");
        }
        
        private ParticleSystem CreatePooledParticle(ParticleEffect effect)
        {
            ParticleSystem ps = Instantiate(effect.prefab, transform);
            ps.gameObject.SetActive(false);
            _particlePools[effect.name].Enqueue(ps);
            return ps;
        }
        
        public void PlayEffect(string name, Vector3 position, Transform parent = null)
        {
            if (_effectDict.TryGetValue(name, out ParticleEffect effect))
            {
                ParticleSystem ps = GetPooledParticle(name);
                
                if (ps != null)
                {
                    ps.transform.position = position;
                    
                    if (effect.attachToParent && parent != null)
                    {
                        ps.transform.SetParent(parent);
                    }
                    else
                    {
                        ps.transform.SetParent(transform);
                    }
                    
                    ps.gameObject.SetActive(true);
                    ps.Play();
                    
                    StartCoroutine(ReturnToPool(ps, effect.name, effect.lifetime));
                }
            }
            else
            {
                Debug.LogWarning($"[ParticleManager] Effect '{name}' not found!");
            }
        }
        
        public void PlayEffect(string name, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            if (_effectDict.TryGetValue(name, out ParticleEffect effect))
            {
                ParticleSystem ps = GetPooledParticle(name);
                
                if (ps != null)
                {
                    ps.transform.position = position;
                    ps.transform.rotation = rotation;
                    
                    if (effect.attachToParent && parent != null)
                    {
                        ps.transform.SetParent(parent);
                    }
                    else
                    {
                        ps.transform.SetParent(transform);
                    }
                    
                    ps.gameObject.SetActive(true);
                    ps.Play();
                    
                    StartCoroutine(ReturnToPool(ps, effect.name, effect.lifetime));
                }
            }
        }
        
        private ParticleSystem GetPooledParticle(string effectName)
        {
            if (_particlePools.TryGetValue(effectName, out Queue<ParticleSystem> pool))
            {
                if (pool.Count > 0)
                {
                    return pool.Dequeue();
                }
                else
                {
                    // Create new if pool is empty
                    return CreatePooledParticle(_effectDict[effectName]);
                }
            }
            
            return null;
        }
        
        private System.Collections.IEnumerator ReturnToPool(ParticleSystem ps, string effectName, float lifetime)
        {
            yield return new WaitForSeconds(lifetime);
            
            ps.Stop();
            ps.gameObject.SetActive(false);
            ps.transform.SetParent(transform);
            
            if (_particlePools.TryGetValue(effectName, out Queue<ParticleSystem> pool))
            {
                pool.Enqueue(ps);
            }
        }
        
        public void StopEffect(string name)
        {
            // Stop all instances of this effect
            foreach (Transform child in transform)
            {
                ParticleSystem ps = child.GetComponent<ParticleSystem>();
                if (ps != null && ps.gameObject.name.Contains(name))
                {
                    ps.Stop();
                }
            }
        }
        
        public void StopAllEffects()
        {
            foreach (Transform child in transform)
            {
                ParticleSystem ps = child.GetComponent<ParticleSystem>();
                if (ps != null)
                {
                    ps.Stop();
                }
            }
        }
    }
}
