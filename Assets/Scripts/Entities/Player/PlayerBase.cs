namespace TF.Entities.Player
{
    public class PlayerBase : EntityBase
    {
        public PlayerBase(EntityView view) : base(view)
        {
            
        }

        public override bool IsPlayer() => true;
    }
}