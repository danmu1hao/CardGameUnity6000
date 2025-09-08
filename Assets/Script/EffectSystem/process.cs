using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TimingEnum
{
    Test
}
public class process
{
    TimingEnum timing;
    #region  content
    Player currentPlayer;
    #endregion
    public process(TimingEnum timing)
    {
        this.timing = timing;
        currentPlayer = BattleSystem.instance.currentPlayer;
    }


}
