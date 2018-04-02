using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BFramework.ShootingGame
{
    public class World
    {
        public World()
        {
            creatureNumber = 10;
            creatures = new List<Creature>(creatureNumber);
            
            for(int i = 0; i < creatureNumber; i++)
            {
                creatures.Add(new Creature() {
                    id = 10000 + i,
                    name = "Character_" + 10000 + i,
                });
            }
        }

        public int creatureNumber;
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
