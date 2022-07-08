using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFuMonastery.Refactoring
{
    public class Human
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public Human() { }

        private static Human HumanWithName(string name)
        {
            Human human = new Human();
            human.Name = name;
            return human;
        }

        private static Human HumanWithDescription(string description)
        {
            Human human = new Human();
            human.Description = description;
            return human;
        }
    }
    public class Sword
    {
        public Rarity rarity { get; set; }
        public Material material { get; set; }
        public Sword() { }
        private static Sword SwordByMaterial(Material material)
        {
            Sword sword = new Sword();
            sword.material = material;
            return sword;
        }
        private static Sword SwordWithRarity(Rarity rarity)
        {
            Sword sword = new Sword();
            sword.rarity = rarity;
            return sword;
        }

    }
    public enum Rarity
    {
        Legendary,
        Simple
    }

    public enum Material
    {
        Diamond,
        Wooden
    }
}
