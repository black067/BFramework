
namespace BFramework.ShootingGame
{
    public class Equipment
    {
        private string _name;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public virtual void Work()
        {
            
        }
    }

    public class Weapon
    {

    }
}
