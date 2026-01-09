using UnityEngine;
using System.Collections.Generic;

namespace RunForHumanity.Systems.Notifications
{
    public class SmartNotifications : MonoBehaviour
    {
        // PDF Page 6: Notificaciones inteligentes (máx 3/día)
        
        public void ScheduleNotifications()
        {
            // MOCK IMPLEMENTATION of Mobile Notification Scheduling
            // In Unity we would use "Mobile Notifications package"

            List<string> pendingNotifications = new List<string>();

            // 1. Motivacional
            pendingNotifications.Add("Tu grupo necesita 5km más para la meta.");
            
            // 3. Impacto
            pendingNotifications.Add("Tus esfuerzos han generado 50L de agua hoy.");

            foreach(var msg in pendingNotifications)
            {
                Debug.Log($"[Notification Scheduled]: {msg}");
            }
        }

        // Logic to filter max 3 per day would go here, saving "TodaysNotificationCount" in PlayerPrefs.
    }
}
