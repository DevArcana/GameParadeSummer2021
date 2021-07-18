using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Arena;

namespace Ability.Abilities
{
    public static class AbilityFactory
    {
        private static readonly Random Random = new Random();
        private const int OneCount = 10;
        private const int TwoCount = 11;
        private const int ThreeCount = 11;
        private const int AllCount = OneCount + TwoCount + ThreeCount;

        public static BaseAbility GetRandomAbility(GridEntity entity)
        {
            var randomNumber = Random.Next(AllCount);
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
                14 => new FatigueAbility(entity),
                15 => new JumpAbility(entity),
                16 => new PiercingArrowAbility(entity),
                17 => new SlamDashAbility(entity),
                18 => new StandUnitedAbility(entity),
                19 => new SuctionAbility(entity),
                20 => new SwitchAbility(entity),
                21 => new ArmageddonAbility(entity),
                22 => new CloneAbility(entity),
                23 => new ExecuteAbility(entity),
                24 => new ImplodeAbility(entity),
                25 => new KamikazeAbility(entity),
                26 => new MassacreAbility(entity),
                27 => new QueenStyleAbility(entity),
                28 => new ReinforceAbility(entity),
                29 => new SacrificeAbility(entity),
                30 => new SalvationAbility(entity),
                31 => new TeleportAbility(entity),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        public static BaseAbility GetRandomOneCostAbility(GridEntity entity)
        {
            var randomNumber = Random.Next(OneCount);
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
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        public static BaseAbility GetRandomTwoCostAbility(GridEntity entity)
        {
            var randomNumber = Random.Next(TwoCount);
            return randomNumber switch
            {
                0 => new AbsorbHealthAbility(entity),
                1 => new BuffAgilityAbility(entity),
                2 => new BuffFocusAbility(entity),
                3 => new BuffStrengthAbility(entity),
                4 => new FatigueAbility(entity),
                5 => new JumpAbility(entity),
                6 => new PiercingArrowAbility(entity),
                7 => new SlamDashAbility(entity),
                8 => new StandUnitedAbility(entity),
                9 => new SuctionAbility(entity),
                10 => new SwitchAbility(entity),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        public static BaseAbility GetRandomThreeCostAbility(GridEntity entity)
        {
            var randomNumber = Random.Next(ThreeCount);
            return randomNumber switch
            {
                0 => new ArmageddonAbility(entity),
                1 => new CloneAbility(entity),
                2 => new ExecuteAbility(entity),
                3 => new ImplodeAbility(entity),
                4 => new KamikazeAbility(entity),
                5 => new MassacreAbility(entity),
                6 => new QueenStyleAbility(entity),
                7 => new ReinforceAbility(entity),
                8 => new SacrificeAbility(entity),
                9 => new SalvationAbility(entity),
                10 => new TeleportAbility(entity),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}