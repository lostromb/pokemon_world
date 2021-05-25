using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonRpg
{
    [Flags]
    public enum ElementalType : uint
    {
        Normal = 0x1 << 0,
        Fire = 0x1 << 1,
        Water = 0x1 << 2,
        Electric = 0x1 << 3,
        Grass = 0x1 << 4,
        Ice = 0x1 << 5,
        Fighting = 0x1 << 6,
        Poison = 0x1 << 7,
        Ground = 0x1 << 8,
        Flying = 0x1 << 9,
        Psychic = 0x1 << 10,
        Bug = 0x1 << 11,
        Rock = 0x1 << 12,
        Ghost = 0x1 << 13,
        Dragon = 0x1 << 14,
        Dark = 0x1 << 15,
        Steel = 0x1 << 16,
        Fairy = 0x1 << 17,
    }
}
