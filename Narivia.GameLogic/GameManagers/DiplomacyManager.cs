using System;
using System.Collections.Generic;
using System.Linq;

using Narivia.Common.Extensions;
using Narivia.GameLogic.GameManagers.Interfaces;
using Narivia.Models;
using Narivia.Settings;

namespace Narivia.GameLogic.GameManagers
{
    public sealed class DiplomacyManager : IDiplomacyManager
    {
        readonly IWorldManager worldManager;

        Dictionary<string, Relation> relations;

        public DiplomacyManager(IWorldManager worldManager)
        {
            this.worldManager = worldManager;
        }

        public void LoadContent()
        {
            relations = new Dictionary<string, Relation>();

            List<Faction> factions = worldManager.GetFactions().ToList();

            factions.ForEach(s => factions.ForEach(t => InitialiseRelation(s.Id, t.Id)));
        }

        public void UnloadContent()
        {
            relations.Clear();
        }

        /// <summary>
        /// Gets the relations.
        /// </summary>
        /// <returns>The relations.</returns>
        public IEnumerable<Relation> GetRelations()
        => relations.Values;

        /// <summary>
        /// Gets the relations of a faction.
        /// </summary>
        /// <returns>The relations of a faction.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public IEnumerable<Relation> GetFactionRelations(string factionId)
        => relations.Values.Where(r => r.SourceFactionId == factionId &&
                                       r.SourceFactionId != r.TargetFactionId);

        /// <summary>
        /// Changes the relations between two factions.
        /// </summary>
        /// <param name="sourceFactionId">Source faction identifier.</param>
        /// <param name="targetFactionId">Target faction identifier.</param>
        /// <param name="delta">Relations value delta.</param>
        public void ChangeRelations(string sourceFactionId, string targetFactionId, int delta)
        {
            Relation sourceRelation = relations.Values.FirstOrDefault(r => r.SourceFactionId == sourceFactionId &&
                                                                           r.TargetFactionId == targetFactionId);
            Relation targetRelation = relations.Values.FirstOrDefault(r => r.SourceFactionId == targetFactionId &&
                                                                           r.TargetFactionId == sourceFactionId);

            int oldRelations = sourceRelation.Value;
            sourceRelation.Value = Math.Max(-100, Math.Min(sourceRelation.Value + delta, 100));
            targetRelation.Value = sourceRelation.Value;
        }

        /// <summary>
        /// Sets the relations between two factions.
        /// </summary>
        /// <param name="sourceFactionId">Source faction identifier.</param>
        /// <param name="targetFactionId">Target faction identifier.</param>
        /// <param name="value">Relations value.</param>
        public void SetRelations(string sourceFactionId, string targetFactionId, int value)
        {
            Relation sourceRelation = relations.Values.FirstOrDefault(r => r.SourceFactionId == sourceFactionId &&
                                                                           r.TargetFactionId == targetFactionId);
            Relation targetRelation = relations.Values.FirstOrDefault(r => r.SourceFactionId == targetFactionId &&
                                                                           r.TargetFactionId == sourceFactionId);

            sourceRelation.Value = Math.Max(-100, Math.Min(value, 100));
            targetRelation.Value = Math.Max(-100, Math.Min(value, 100));
        }

        void InitialiseRelation(string sourceFactionId, string targetFactionId)
        {
            if (sourceFactionId == targetFactionId ||
                sourceFactionId == GameDefines.GAIA_FACTION ||
                targetFactionId == GameDefines.GAIA_FACTION)
            {
                return;
            }

            Relation relation = new Relation
            {
                Id = $"{sourceFactionId}:{targetFactionId}",
                SourceFactionId = sourceFactionId,
                TargetFactionId = targetFactionId,
                Value = 0
            };

            relations.AddOrUpdate(relation.Id, relation);
        }
    }
}
