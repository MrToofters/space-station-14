﻿using System.Diagnostics.CodeAnalysis;
using Content.Server.Botany.Components;
using Content.Shared.Botany;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Reagent;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Random;
using Robust.Shared.Serialization.Manager.Attributes;

namespace Content.Server.Chemistry.ReagentEffects.PlantMetabolism
{
    [ImplicitDataDefinitionForInheritors]
    public abstract class PlantAdjustAttribute : ReagentEffect
    {
        [Dependency] private readonly IRobustRandom _robustRandom = default!;

        [DataField("amount")] public float Amount { get; protected set; } = 1;
        [DataField("prob")] public float Prob { get; protected set; } = 1; // = (80);

        /// <summary>
        ///     Checks if the plant holder can metabolize the reagent or not. Checks if it has an alive plant by default.
        /// </summary>
        /// <param name="plantHolder">The entity holding the plant</param>
        /// <param name="plantHolderComponent">The plant holder component</param>
        /// <param name="entityManager">The entity manager</param>
        /// <param name="mustHaveAlivePlant">Whether to check if it has an alive plant or not</param>
        /// <returns></returns>
        public bool CanMetabolize(EntityUid plantHolder, [NotNullWhen(true)] out PlantHolderComponent? plantHolderComponent,
            IEntityManager entityManager,
            bool mustHaveAlivePlant = true)
        {
            plantHolderComponent = null;

            if (!entityManager.TryGetComponent(plantHolder, out plantHolderComponent)
                                    || mustHaveAlivePlant && (plantHolderComponent.Seed == null || plantHolderComponent.Dead))
                return false;

            if (Prob >= 1f)
                return true;

            return !(Prob <= 0f) && _robustRandom.Prob(Prob);
        }
    }
}
