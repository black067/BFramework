
namespace BFramework.ShootingGame
{
    public class Equipment
    {
        private string _name;
        private int _id;

        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }
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

        public enum TYPE
        {
            CLOTHING,
            ACCESSORIES,
            WEAPON
        }

        public virtual void Work()
        {

        }
    }

    public class Clothing : Equipment
    {
        public override void Work()
        {

        }
    }
    
}
