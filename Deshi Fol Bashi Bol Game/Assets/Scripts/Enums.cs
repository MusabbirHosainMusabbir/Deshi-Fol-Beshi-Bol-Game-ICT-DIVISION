using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    //enums for the state of the slingshot, the 
    //state of the game and the state of the 


    public enum SlingshotState
    {
        Idle,
        UserPulling,
        GuiltiFlying
    }

    public enum GameState
    {
        Start,
        GuiltiMovingToSlingshot,
        playing,
        Won ,
        Lost,
            next
    }


    public enum GuiltiState
    {
        BeforeThrown,
        Thrown
    }
    
}
