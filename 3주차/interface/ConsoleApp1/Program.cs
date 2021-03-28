using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Character a = new Character();
            a.damage = 20;
            a.name = "플레이어A";
            Character b = new Character();
            b.damage = 5;
            b.name = "플레이어B";

            a.Attack(b);
            b.Attack(a);
        }
    }

    class Character
    {
        public string name;
        public int hp = 100;
        public int damage = 10;

        public void Attack(Character target)
        {
            Console.WriteLine($"{this.name}가 {target.name}을 공격!");
            Console.WriteLine($"{target.hp} -> {target.hp - this.damage} ({this.damage})");
            target.Hitted(this.damage);
        }

        private void Hitted(int damage)
        {
            hp -= damage;
        }
    }
}
