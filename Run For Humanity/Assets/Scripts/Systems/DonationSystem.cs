using UnityEngine;
using System;
using System.Collections.Generic;
using RunForHumanity.Core;
using RunForHumanity.Data;

namespace RunForHumanity.Systems
{
    /// <summary>
    /// Manages ONG selection and donation distribution
    /// SOLID: Single Responsibility - Handles donation logic
    /// </summary>
    public class DonationSystem : MonoBehaviour, IInitializable
    {
        [Header("ONG Database")]
        [SerializeField] private List<ONGData> _availableONGs = new List<ONGData>();
        
        [Header("Revenue Tracking")]
        [SerializeField] private float _totalRevenue;
        [SerializeField] private float _pendingDonation;
        [SerializeField] private DateTime _nextDonationDate;
        
        private PlayerData _playerData;
        private Dictionary<string, ONGData> _ongDatabase = new Dictionary<string, ONGData>();
        
        public static DonationSystem Instance { get; private set; }
        public bool IsInitialized { get; private set; }
        
        public event Action<float> OnRevenueGenerated;
        public event Action<float, Dictionary<string, float>> OnDonationProcessed;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        public void Initialize()
        {
            // Initialize default ONGs if none are set
            if (_availableONGs.Count == 0)
            {
                InitializeDefaultONGs();
            }
            
            // Build ONG database
            foreach (var ong in _availableONGs)
            {
                if (!_ongDatabase.ContainsKey(ong.id))
                {
                    _ongDatabase.Add(ong.id, ong);
                }
            }
            
            // Load player data
            _playerData = new PlayerData(); // In real app, load from save system
            _nextDonationDate = DateTime.Now.AddMonths(1);
            
            ServiceLocator.RegisterService(this);
            IsInitialized = true;
            Debug.Log($"[DonationSystem] Initialized with {_availableONGs.Count} ONGs");
        }
        
        private void InitializeDefaultONGs()
        {
            _availableONGs.Add(new ONGData("water_org", "Water.org", "Agua")
            {
                descripcion = "Proporciona acceso a agua potable y saneamiento",
                impacto = "$1 = 20 litros de agua limpia",
                colorTema = new Color(0.2f, 0.6f, 1f)
            });
            
            _availableONGs.Add(new ONGData("education_fund", "Education Fund", "Educación")
            {
                descripcion = "Proporciona educación a niños en países en desarrollo",
                impacto = "$1 = 1 día de escuela",
                colorTema = new Color(1f, 0.7f, 0.2f)
            });
            
            _availableONGs.Add(new ONGData("health_world", "Health World", "Salud")
            {
                descripcion = "Atención médica básica en zonas vulnerables",
                impacto = "$1 = 3 vacunas esenciales",
                colorTema = new Color(1f, 0.3f, 0.3f)
            });
            
            _availableONGs.Add(new ONGData("green_earth", "Green Earth", "Medioambiente")
            {
                descripcion = "Reforestación y conservación ambiental",
                impacto = "$1 = 2 árboles plantados",
                colorTema = new Color(0.3f, 0.8f, 0.3f)
            });
            
            _availableONGs.Add(new ONGData("food_relief", "Food Relief", "Alimentación")
            {
                descripcion = "Combate el hambre y la malnutrición",
                impacto = "$1 = 4 comidas nutritivas",
                colorTema = new Color(1f, 0.5f, 0.2f)
            });
        }
        
        public void ProcessGameRevenue(float amount)
        {
            if (amount <= 0) return;
            
            // 80% of revenue goes to ONGs
            float donationAmount = amount * GameConstants.DONATION_PERCENTAGE;
            
            _totalRevenue += amount;
            _pendingDonation += donationAmount;
            
            OnRevenueGenerated?.Invoke(donationAmount);
            
            Debug.Log($"[DonationSystem] Revenue processed: ${amount:F2} | Donation: ${donationAmount:F2}");
        }
        
        public void DistributeDonations()
        {
            if (_pendingDonation <= 0) return;
            
            Dictionary<string, float> distribution = new Dictionary<string, float>();
            
            // Distribute based on player preferences
            foreach (var dist in _playerData.ongDistribution)
            {
                float amount = _pendingDonation * (dist.porcentaje / 100f);
                distribution[dist.ongId] = amount;
                
                Debug.Log($"[DonationSystem] {dist.ongId}: ${amount:F2}");
            }
            
            // Create certificate
            DonationCertificate certificate = new DonationCertificate(_pendingDonation, distribution);
            _playerData.certificates.Add(certificate);
            _playerData.totalDonated += _pendingDonation;
            
            OnDonationProcessed?.Invoke(_pendingDonation, distribution);
            
            _pendingDonation = 0;
            _nextDonationDate = DateTime.Now.AddMonths(1);
        }
        
        public void UpdateONGDistribution(List<ONGDistribution> newDistribution)
        {
            // Validate that percentages sum to 100
            float total = 0;
            foreach (var dist in newDistribution)
            {
                total += dist.porcentaje;
            }
            
            if (Mathf.Abs(total - 100f) > 0.1f)
            {
                Debug.LogError($"[DonationSystem] Distribution must sum to 100%! Current: {total}%");
                return;
            }
            
            _playerData.ongDistribution = newDistribution;
            Debug.Log($"[DonationSystem] ONG distribution updated with {newDistribution.Count} organizations");
        }
        
        public List<ONGData> GetAvailableONGs()
        {
            return new List<ONGData>(_availableONGs);
        }
        
        public ONGData GetONGData(string ongId)
        {
            return _ongDatabase.TryGetValue(ongId, out ONGData ong) ? ong : null;
        }
        
        public string GetImpactDescription()
        {
            float totalDonated = _playerData.totalDonated + _pendingDonation;
            
            // Calculate total impact across all ONGs
            string impact = $"Total donated: ${totalDonated:F2}\n\n";
            
            foreach (var dist in _playerData.ongDistribution)
            {
                ONGData ong = GetONGData(dist.ongId);
                if (ong != null)
                {
                    float ongAmount = totalDonated * (dist.porcentaje / 100f);
                    impact += $"{ong.nombre}: ${ongAmount:F2} ({dist.porcentaje:F1}%)\n";
                    impact += $"Impact: {ong.impacto}\n\n";
                }
            }
            
            return impact;
        }
        
        public float GetPendingDonation()
        {
            return _pendingDonation;
        }
        
        public DateTime GetNextDonationDate()
        {
            return _nextDonationDate;
        }
    }
}
