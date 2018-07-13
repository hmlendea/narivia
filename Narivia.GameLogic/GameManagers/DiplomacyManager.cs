using System;
using System.Collections.Generic;
using System.Linq;

using Narivia.Common.Extensions;
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

            relations = new Dictionary<string, Relation>();
        }

        public void LoadContent()
        {

        }

        public void UnloadContent()
        {
            relations.Clear();
        }

        /// <summary>
        /// Gets the relation between two factions.
        /// </summary>
        /// <returns>The faction relation.</returns>
        /// <param name="sourceFactionId">Source faction identifier.</param>
        /// <param name="targetFactionId">Target faction identifier.</param>
        public Relation GetRelation(string sourceFactionId, string targetFactionId)
        => GetRelations().FirstOrDefault(r =>
               (r.SourceFactionId == sourceFactionId && r.TargetFactionId == targetFactionId) ||
               (r.SourceFactionId == targetFactionId && r.TargetFactionId == sourceFactionId));

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
        => relations.Values.Where(r =>
               (r.SourceFactionId == factionId || r.TargetFactionId == factionId) &&
               r.SourceFactionId != r.TargetFactionId);

        /// <summary>
        /// Changes the relations between two factions.
        /// </summary>
        /// <param name="sourceFactionId">Source faction identifier.</param>
        /// <param name="targetFactionId">Target faction identifier.</param>
        /// <param name="delta">Relations value delta.</param>
        public void ChangeRelations(string sourceFactionId, string targetFactionId, int delta)
        {
            Relation relation = GetRelation(sourceFactionId, targetFactionId);

            SetRelations(relation, relation.Value + delta);
        }

        /// <summary>
        /// Sets the relations between two factions.
        /// </summary>
        /// <param name="sourceFactionId">Source faction identifier.</param>
        /// <param name="targetFactionId">Target faction identifier.</param>
        /// <param name="value">Relations value.</param>
        public void SetRelations(string sourceFactionId, string targetFactionId, int value)
        {
            Relation relation = GetRelation(sourceFactionId, targetFactionId);

            SetRelations(relation, value);
        }

        public void SetRelations(Relation relation, int value)
        {
            relation.Value = Math.Max(-100, Math.Min(value, 100));
        }

        public void InitialiseFactionRelations(string sourceFactionId)
        {
            if (sourceFactionId == GameDefines.GaiaFactionIdentifier)
            {
                return;
            }

            foreach (Faction targetFaction in worldManager.GetFactions())
            {
                if (targetFaction.Id == sourceFactionId ||
                    targetFaction.Id == GameDefines.GaiaFactionIdentifier)
                {
                    continue;
                }

                Relation relation = new Relation
                {
                    Id = $"{sourceFactionId}:{targetFaction.Id}",
                    SourceFactionId = sourceFactionId,
                    TargetFactionId = targetFaction.Id,
                    Value = 0
                };

                relations.AddOrUpdate(relation.Id, relation);
            }
        }
    }
}
