using UnityEngine;

public class DodgeRollState : IState
{
    PlayerController playerController;

    public DodgeRollState(PlayerController player)
    {
        this.playerController = player;
    }
}
