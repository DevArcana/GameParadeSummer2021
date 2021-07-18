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
        private static int count = 23;
        
        public static BaseAbility GetRandomAbility(GridEntity entity)
        {
            var randomNumber = _random.Next(count);
            return randomNumber switch
            {
                0 => new ArrowAbility(entity),
                1 => new DiagonalDashAbility(entity),
                2 => new FireballAbility(entity),
                3 => new HealAllyAbility(entity),
                4 => new HealPlayerAbility(entity),
                5 => new LeechAbility(entity),
                6 => new PunchAbility(entity),
                7 => new StraightDashAbility(entity),
                8 => new AbsorbHealthAbility(entity),
                9 => new BuffAgilityAbility(entity),
                10 => new BuffFocusAbility(entity),
                11 => new BuffStrengthAbility(entity),
                12 => new KamikazeAbility(entity),
                13 => new PiercingArrowAbility(entity),
                14 => new SlamDashAbility(entity),
                15 => new SuctionAbility(entity),
                16 => new ArmageddonAbility(entity),
                17 => new CloneAbility(entity),
                18 => new ExecuteAbility(entity),
                19 => new MassacreAbility(entity),
                20 => new ReinforceAbility(entity),
                21 => new SacrificeAbility(entity),
                22 => new SalvationAbility(entity),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}