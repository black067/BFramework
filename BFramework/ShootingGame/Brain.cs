
namespace BFramework.ShootingGame
{
    public class Brain
    {
        public class Decision
        {
            public int power;
        }
        public virtual void Work(ref Creature.Attribute attributes, ref Creature.Command command)
        {

        }
    }

    public class BBrain : Brain
    {
        public override void Work(ref Creature.Attribute attributes, ref Creature.Command command)
        {

        }
    }
}
