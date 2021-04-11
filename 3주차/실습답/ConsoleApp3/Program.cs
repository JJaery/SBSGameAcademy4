using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    class Program
    {
        static World world = new World();
        static void Main(string[] args)
        {
            world.Init();

            foreach(Creature target in world.creatureList)
            {
                if(target is iSwimmable)
                {
                    (target as iSwimmable).Swim();
                }
            }

        }
    }

    interface iSwimmable
    {
        void Swim();
    }

    class World
    {
        public List<Creature> creatureList = new List<Creature>();
        public void Init()
        {
            for (int i = 0; i < 3; i++)
            {
                this.creatureList.Add(new Monkey());
            }

            creatureList.Add(new Whale());
            creatureList.Add(new Whale());
            creatureList.Add(new Whale());

            creatureList.Add(new Dolphin());
            creatureList.Add(new Dolphin());
            creatureList.Add(new Dolphin());

            creatureList.Add(new Eagle());
            creatureList.Add(new Eagle());
            creatureList.Add(new Eagle());

            creatureList.Add(new Sparrow());
            creatureList.Add(new Sparrow());
            creatureList.Add(new Sparrow());

            creatureList.Add(new Pigeon());
            creatureList.Add(new Pigeon());
            creatureList.Add(new Pigeon());

            creatureList.Add(new Penguin());
            creatureList.Add(new Penguin());
            creatureList.Add(new Penguin());

            creatureList.Add(new GoldFish());
            creatureList.Add(new GoldFish());
            creatureList.Add(new GoldFish());

            creatureList.Add(new Eel());
            creatureList.Add(new Eel());
            creatureList.Add(new Eel());
        }
    }

    abstract class Creature // 생성 불가능한 추상화 클래스
    {
    }

    abstract class Mammal : Creature // 포유류
    {
    }

    abstract class Birds : Creature// 조류
    {
    }

    abstract class Fish : Creature// 어류
    {
    }

    class Monkey : Mammal
    {
    }

    class Whale : Mammal, iSwimmable
    {
        public void Swim()
        {
            Console.WriteLine("[Whale] 수영했어요!");
        }
    }

    class Dolphin : Mammal, iSwimmable
    {
        public void Swim()
        {
            Console.WriteLine($"[{nameof(Dolphin)}] 수영했어요!");
        }
    }

    class Eagle : Birds
    {
    }

    class Sparrow : Birds
    {
    }

    class Pigeon : Birds
    {
    }

    class Penguin : Birds, iSwimmable
    {
        public void Swim()
        {
            Console.WriteLine($"[{nameof(Penguin)}] 수영했어요!");
        }
    }

    class GoldFish : Fish, iSwimmable
    {
        public void Swim()
        {
            Console.WriteLine($"[{nameof(GoldFish)}] 수영했어요!");
        }
    }

    class Eel : Fish, iSwimmable
    {
        public void Swim()
        {
            Console.WriteLine($"[{nameof(Eel)}] 수영했어요!");
        }
    }
}
