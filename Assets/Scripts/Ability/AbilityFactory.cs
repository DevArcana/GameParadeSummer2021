using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Arena;

namespace Ability.Abilities
{
    public static class AbilityFactory
    {
        private static readonly Random _random = new Random();
        private static int count = 15;
        
        public static BaseAbility GetRandomAbility(GridEntity entity)
        {
            var randomNumber = _random.Next(count);
            return randomNumber switch
            {
                0 => new AbsorbHealthAbility(entity),
                1 => new ArmageddonAbility(entity),
                2 => new ExecuteAbility(entity),
                3 => new FireballAbility(entity),
                4 => new HealAllyAbility(entity),
                5 => new HealPlayerAbility(entity),
                6 => new KamikazeAbility(entity),
                7 => new LeechAbility(entity),
                8 => new MassacreAbility(entity),
                9 => new SacrificeAbility(entity),
                10 => new SalvationAbility(entity),
                11 => new StraightDashAbility(entity),
                12 => new SuctionAbility(entity),
                13 => new CloneAbility(entity),
                14 => new BuffStrengthAbility(entity),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}