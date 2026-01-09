using UnityEngine;

namespace RunForHumanity.Gameplay
{
    public static class LaneSystem
    {
        public const float LANE_DISTANCE = 3.0f; // Distance between lanes
        public const int LEFT_LANE = -1;
        public const int MIDDLE_LANE = 0;
        public const int RIGHT_LANE = 1;

        public static float GetXPosition(int laneIndex)
        {
            return laneIndex * LANE_DISTANCE;
        }
    }
}
