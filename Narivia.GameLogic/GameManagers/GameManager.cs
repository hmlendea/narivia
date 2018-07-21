using System;
using System.Collections.Generic;
using System.Linq;

using Narivia.GameLogic.Enumerations;
using Narivia.GameLogic.Events;
using Narivia.Common.Extensions;
using Narivia.Models;
using Narivia.Models.Enumerations;
using Narivia.Settings;

namespace Narivia.GameLogic.GameManagers
{
    /// <summary>
    /// Game manager.
    /// </summary>
    public class GameManager : IGameManager
    {
        readonly IAttackManager attackManager;
        readonly IDiplomacyManager diplomacyManager;
        readonly IEconomyManager economyManager;
        readonly IHoldingManager holdingManager;
        readonly IMilitaryManager militaryManager;
        readonly IWorldManager worldManager;

        /// <summary>
        /// Occurs when a player province is attacked.
        /// </summary>
        public event BattleEventHandler PlayerProvinceAttacked;

        /// <summary>
        /// Occurs when a faction was destroyed.
        /// </summary>
        public event FactionEventHandler FactionDestroyed;

        /// <summary>
        /// Occurs when a faction was revived.
        /// </summary>
        public event FactionEventHandler FactionRevived;

        /// <summary>
        /// Occurs a faction won the game.
        /// </summary>
        public event FactionEventHandler FactionWon;

        /// <summary>
        /// Gets the world identifier.
        /// </summary>
        /// <value>The world identifier.</value>
        public string WorldId { get; private set; }

        /// <summary>
        /// Gets the player faction identifier.
        /// </summary>
        /// <value>The player faction identifier.</value>
        public string PlayerFactionId { get; private set; }

        /// <summary>
        /// Gets the turn.
        /// </summary>
        /// <value>The turn.</value>
        public int Turn { get; private set; }

        public GameManager(
            IAttackManager attackManager,
            IDiplomacyManager diplomacyManager,
            IEconomyManager economyManager,
            IHoldingManager holdingManager,
            IMilitaryManager militaryManager,
            IWorldManager worldManager)
        {
            this.attackManager = attackManager;
            this.diplomacyManager = diplomacyManager;
            this.economyManager = economyManager;
            this.holdingManager = holdingManager;
            this.militaryManager = militaryManager;
            this.worldManager = worldManager;
        }

        public void LoadContent(string worldId, string factionId)
        {
            WorldId = worldId;
            PlayerFactionId = factionId;

            foreach (Province province in worldManager.GetProvinces())
            {
                worldManager.InitialiseProvince(province.Id);
            }

            foreach (Faction faction in worldManager.GetFactions().Where(f => f.Type.IsActive))
            {
                worldManager.InitialiseFaction(faction.Id);
                diplomacyManager.InitialiseFactionRelations(faction.Id);
                holdingManager.InitialiseFactionHoldings(faction.Id);
                militaryManager.InitialiseFactionMilitary(faction.Id);
            }

            Turn = 0;
        }

        public void UnloadContent()
        {
            diplomacyManager.UnloadContent();
            economyManager.UnloadContent();
            holdingManager.UnloadContent();
            militaryManager.UnloadContent();
            worldManager.UnloadContent();
        }

        /// <summary>
        /// Advances the game by one turn.
        /// </summary>
        public void NextTurn()
        {
            foreach (Faction faction in worldManager.GetFactions().Where(f => f.Type.IsActive))
            {
                // Economy
                faction.Wealth += economyManager.GetFactionIncome(faction.Id);
                faction.Wealth -= economyManager.GetFactionOutcome(faction.Id);

                // Recruit
                militaryManager.GetArmy(faction.Id, GameDefines.MilitiaUnitIdentifier).Size += militaryManager.GetFactionRecruitment(faction.Id);

                // A.I.
                if (faction.Id == PlayerFactionId)
                {
                    continue;
                }

                AiBuild(faction.Id);
                AiRecruit(faction.Id);

                string provinceId = attackManager.ChooseProvinceToAttack(faction.Id);

                if (militaryManager.GetFactionTroopsAmount(faction.Id) < GetWorld().MinTroopsPerAttack ||
                    string.IsNullOrEmpty(provinceId))
                {
                    continue;
                }

                AttackProvince(faction.Id, provinceId);
            }

            foreach (Province province in worldManager.GetProvinces())
            {
                province.Locked = false;
            }

            UpdateFactionsAliveStatus();
            CheckForWinner();

            Turn += 1;
        }

        /// <summary>
        /// Gets the culture of a faction.
        /// </summary>
        /// <returns>The culture.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public Culture GetFactionCulture(string factionId)
        {
            Faction faction = worldManager.GetFaction(factionId);
            return worldManager.GetCulture(faction.CultureId);
        }

        /// <summary>
        /// Gets the flag of a factions.
        /// </summary>
        /// <returns>The faction flag.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public Flag GetFactionFlag(string factionId)
        {
            Faction faction = worldManager.GetFaction(factionId);
            return worldManager.GetFlag(faction.FlagId);
        }

        /// <summary>
        /// Gets the faction capital.
        /// </summary>
        /// <returns>The faction capital province.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public Province GetFactionCapital(string factionId)
        => worldManager.GetFactionCapital(factionId);

        /// <summary>
        /// Gets the world.
        /// </summary>
        /// <returns>The world.</returns>
        public World GetWorld()
        => worldManager.GetWorld();

        /// <summary>
        /// The player faction will attack the specified province.
        /// </summary>
        /// <param name="provinceId">Province identifier.</param>
        public BattleResult PlayerAttackProvince(string provinceId)
            => AttackProvince(PlayerFactionId, provinceId);

        void UpdateFactionsAliveStatus()
        {
            foreach (Faction faction in worldManager.GetFactions())
            {
                bool wasAlive = faction.Type.IsActive;
                int provincesCount = worldManager.GetFactionProvinces(faction.Id).Count();

                if (!wasAlive &&
                    provincesCount > 0 &&
                    faction.Type == FactionType.Inactive)
                {
                    FactionRevived(this, new FactionEventArgs(faction.Id));
                    faction.Type = FactionType.Active;
                }
                else if (wasAlive && provincesCount == 0)
                {
                    FactionDestroyed(this, new FactionEventArgs(faction.Id));
                    faction.Type = FactionType.Inactive;
                }
            }
        }

        void CheckForWinner()
        {
            IEnumerable<Faction> factions = worldManager.GetFactions();

            if (factions.Count(f => f.Type.IsActive) > 1)
            {
                return;
            }

            Faction faction = factions.FirstOrDefault(f => f.Type.IsActive);

            FactionWon?.Invoke(this, new FactionEventArgs(faction.Id));
        }

        BattleResult AttackProvince(string factionId, string provinceId)
        {
            Province province = worldManager.GetProvince(provinceId);
            string oldProvinceFactionId = province.FactionId;

            BattleResult result = attackManager.AttackProvince(factionId, provinceId);

            if (diplomacyManager.GetRelation(factionId, oldProvinceFactionId).Value > 0)
            {
                diplomacyManager.SetRelations(factionId, oldProvinceFactionId, 0);
            }

            if (result == BattleResult.Victory)
            {
                diplomacyManager.ChangeRelations(factionId, oldProvinceFactionId, -10);
            }

            switch (province.Type)
            {
                case ProvinceType.Capital:
                    diplomacyManager.ChangeRelations(factionId, oldProvinceFactionId, -10);
                    break;

                case ProvinceType.Province:
                    diplomacyManager.ChangeRelations(factionId, oldProvinceFactionId, -5);
                    break;
            }

            switch (province.State)
            {
                case ProvinceState.Sovereign:
                    diplomacyManager.ChangeRelations(factionId, oldProvinceFactionId, -5);
                    break;

                case ProvinceState.Occupied:
                    diplomacyManager.ChangeRelations(factionId, oldProvinceFactionId, -2);
                    break;
            }

            if (oldProvinceFactionId == PlayerFactionId)
            {
                BattleEventArgs e = new BattleEventArgs(provinceId, factionId, result);

                PlayerProvinceAttacked?.Invoke(this, e);
            }

            return result;
        }

        void AiBuild(string factionId)
        {
            Faction faction = worldManager.GetFaction(factionId);

            // TODO: Reduce all this duplicated code

            while (faction.Wealth >= GetWorld().HoldingsPrice)
            {
                List<Province> validSovereignProvinces = worldManager
                    .GetFactionProvinces(factionId)
                    .Where(r => r.State == ProvinceState.Sovereign && holdingManager.ProvinceHasEmptyHoldingSlots(r.Id)).ToList();

                if (validSovereignProvinces.Count < 1)
                {
                    break;
                }

                HoldingType type = Enum.GetValues(typeof(HoldingType)).Cast<HoldingType>().Where(x => x != HoldingType.Empty).GetRandomElement();

                holdingManager.BuildHolding(validSovereignProvinces.GetRandomElement().Id, type);
            }

            while (faction.Wealth >= GetWorld().HoldingsPrice)
            {
                List<Province> validOccupiedProvinces = worldManager
                    .GetFactionProvinces(factionId)
                    .Where(r => r.State == ProvinceState.Occupied && holdingManager.ProvinceHasEmptyHoldingSlots(r.Id)).ToList();

                if (validOccupiedProvinces.Count < 1)
                {
                    break;
                }

                HoldingType type = Enum.GetValues(typeof(HoldingType)).Cast<HoldingType>().Where(x => x != HoldingType.Empty).GetRandomElement();

                holdingManager.BuildHolding(validOccupiedProvinces.GetRandomElement().Id, type);
            }
        }

        void AiRecruit(string factionId)
        {
            int minPrice = militaryManager.GetUnits().Min(u => u.Price);

            Faction faction = worldManager.GetFaction(factionId);

            while (faction.Wealth >= minPrice)
            {
                string unitId = militaryManager.GetUnits().Select(u => u.Id).GetRandomElement();

                militaryManager.RecruitUnits(factionId, unitId, 1);
            }
        }
    }
}
