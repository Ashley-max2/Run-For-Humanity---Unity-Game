using System;
using System.Collections.Generic;
using UnityEngine;

namespace RunForHumanity.Data
{
    [System.Serializable]
    public class ONGData
    {
        public string id;
        public string nombre;
        public string categoria; // "Agua", "Educación", "Salud", etc.
        [TextArea] public string descripcion;
        public string impacto; // "1$ = 20 litros de agua"
        public string logoURL;
        public string sitioWeb;
        [Range(0, 100)] public float porcentajeTransparencia;
        public float ratingUsuarios;
        public bool verificada;
        public Color colorTema = Color.white;
        
        public ONGData() { }
        
        public ONGData(string id, string nombre, string categoria)
        {
            this.id = id;
            this.nombre = nombre;
            this.categoria = categoria;
        }
    }

    [System.Serializable]
    public class ONGDistribution
    {
        public string ongId;
        [Range(0, 100)] public float percentage;
        
        // Alias for compatibility
        public float porcentaje 
        { 
            get => percentage; 
            set => percentage = value; 
        }
        
        public ONGDistribution() { }
        
        public ONGDistribution(string ongId, float percentage)
        {
            this.ongId = ongId;
            this.percentage = percentage;
        }
    }

    [System.Serializable]
    public class UserDonationPreferences
    {
        public string userId;
        public List<ONGDistribution> distribucion = new List<ONGDistribution>();
        public float totalAcumuladoMes;
        public float totalAcumuladoHistorico;
        public string proximaTransferenciaVal; // DateTime as string for JSON simplicity or stick to DateTime if using NewtonSoft
        // public List<Certificado> certificados;
    }
}
