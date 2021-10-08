namespace Stratego.Core
{
    public class PlayerNPC : Player
    {
        public string Name { get; } = "Player2";
        public PlayerNPC(int maxPieces = 40) : base(maxPieces)
        {
        }
    }
}