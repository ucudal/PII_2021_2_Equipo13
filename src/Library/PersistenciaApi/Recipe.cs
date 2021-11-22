//-------------------------------------------------------------------------
// <copyright file="Recipe.cs" company="Universidad Católica del Uruguay">
// Copyright (c) Programación II. Derechos reservados.
// </copyright>
//-------------------------------------------------------------------------

using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Recipes
{
    public class Recipe : IJsonConvertible
    {
        public Product FinalProduct { get; set; }

        [JsonConstructor]
        public Recipe()
        {
            // Intencionalmente en blanco
        }

        public Recipe(Product finalProduct)
        {
            this.FinalProduct = finalProduct;
        }

        [JsonInclude]
        public IList<Step> Steps { get; private set; } = new List<Step>();

        public void AddStep(Step step)
        {
            this.Steps.Add(step);
        }

        public void RemoveStep(Step step)
        {
            this.Steps.Remove(step);
        }

        public string ConvertToJson()
        {
            JsonSerializerOptions options = new()
            {
                ReferenceHandler = MyReferenceHandler.Instance,
                WriteIndented = true
            };

            return JsonSerializer.Serialize(this, options);
        }
    }
}