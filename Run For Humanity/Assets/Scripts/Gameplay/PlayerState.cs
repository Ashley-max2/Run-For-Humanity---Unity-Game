namespace RunForHumanity.Gameplay
{
    /// <summary>
    /// Estados posibles del jugador
    /// </summary>
    public enum PlayerState
    {
        Idle,       // Quieto (inicio del juego)
        Running,    // Corriendo normal
        Jumping,    // En el aire
        Sliding,    // Deslizándose
        Dead        // Muerto
    }
}
