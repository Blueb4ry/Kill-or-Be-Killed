using kobk.csharp.game.player;

namespace kobk.csharp.game.player.attack
{
    public interface AtkAction
    {
        void OnEnable();
        void OnDisable();
        void OnAction(GamePlayerCharacter Atker);
    }
}