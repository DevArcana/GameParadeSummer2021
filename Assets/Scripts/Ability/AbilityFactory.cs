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
        private static int count = 31;
        
        public static BaseAbility GetRandomAbility(GridEntity entity)
        {
            var randomNumber = _random.Next(count);
            return randomNumber switch
            {
                0 => new ArrowAbility(entity),
                1 => new DiagonalDashAbility(entity),
                2 => new ExposeAbility(entity),
                3 => new FireballAbility(entity),
                4 => new HealAllyAbility(entity),
                5 => new HealPlayerAbility(entity),
                6 => new LeechAbility(entity),
                7 => new PunchAbility(entity),
                8 => new StraightDashAbility(entity),
                9 => new StrengthenAbility(entity),
                10 => new AbsorbHealthAbility(entity),
                11 => new BuffAgilityAbility(entity),
                12 => new BuffFocusAbility(entity),
                13 => new BuffStrengthAbility(entity),
                14 => new JumpAbility(entity),
                15 => new PiercingArrowAbility(entity),
                16 => new SlamDashAbility(entity),
                17 => new StandUnitedAbility(entity),
                18 => new SuctionAbility(entity),
                19 => new SwitchAbility(entity),
                20 => new ArmageddonAbility(entity),
                21 => new CloneAbility(entity),
                22 => new ExecuteAbility(entity),
                23 => new ImplodeAbility(entity),
                24 => new KamikazeAbility(entity),
                25 => new MassacreAbility(entity),
                26 => new QueenStyleAbility(entity),
                27 => new ReinforceAbility(entity),
                28 => new SacrificeAbility(entity),
                29 => new SalvationAbility(entity),
                30 => new TeleportAbility(entity),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}