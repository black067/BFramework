using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFramework.ShootingGame
{
    public class Equipment
    {
        private string _name;
        private int _id;

        public int Id { get => _id; set => _id = value; }
        public string Name { get => _name; set => _name = value; }

        public enum TYPE
        {
            CLOTHING,
            ACCESSORIES,
            WEAPON
        }
    }

    public class Clothing : Equipment
    {

    }

    public class T
    {
        Clothing clothing;
        void TE()
        {
            clothing = new Clothing();
            clothing.Name = "A";
        }
    }
}
