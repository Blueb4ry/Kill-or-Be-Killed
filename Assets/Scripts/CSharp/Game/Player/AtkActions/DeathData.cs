using System;

namespace kobk.csharp.game.player.attack
{
    [Serializable]
    public class DeathData
    {
        public readonly int AtkerId;
        public readonly string Message;

        public DeathData(int aid, string msg)
        {
            AtkerId = aid;
            Message = msg;
        }

        public DeathData()
        {
            AtkerId = -1;
            Message = string.Empty;
        }
    }
}