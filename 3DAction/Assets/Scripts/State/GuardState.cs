using UnityEngine;

public class GuardState : IState
{
    PlayerController playerController;

    public GuardState(PlayerController player)
    {
        this.playerController = player;
    }
}
