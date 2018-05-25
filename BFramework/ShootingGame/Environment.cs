using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BFramework.ShootingGame
{
    public class Environment
    {
        public Environment(int number = 10)
        {
            creatureNumber = number;
            creatureID = new int[creatureNumber];
            creatures = new List<Creature>(creatureNumber);
            int ID = startNumber;
            
            for(int i = 0; i < creatureNumber; i++)
            {
                ID += i;
                creatureID[i] = ID;
                creatures.Add(new Creature() {
                    id = ID,
                    name = "Character_" + ID,
                });
            }
        }

        public int startNumber;
        public int creatureNumber;
        public int[] creatureID;
        public List<Creature> creatures;

        public void Refresh()
        {
            foreach(Creature creature in creatures)
            {
                creature.Update();
            }
        }
    }
}
