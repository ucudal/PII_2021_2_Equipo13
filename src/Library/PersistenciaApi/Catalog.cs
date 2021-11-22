//-------------------------------------------------------------------------
// <copyright file="Catalog.cs" company="Universidad Católica del Uruguay">
// Copyright (c) Programación II. Derechos reservados.
// </copyright>
//-------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Recipes
{
    public class Catalog
    {
        public IList<Product> Products { get; set; }
 
        public IList<Equipment> Equipments { get; set; }

        private static Catalog instance;

        private Catalog()
        {
            Initialize();
        }

        public static Catalog Instance
        {
            get{
                if (instance == null)
                {
                    instance = new Catalog();
                }

                return instance;
            }
        }

        public void Add(Product product)
        {
            this.Products.Add(product);
        }

        public void Remove(Product product)
        {
            this.Products.Remove(product);
        }

        public void Add(Equipment equipment)
        {
            this.Equipments.Add(equipment);
        }

        public void Remove(Equipment equipment)
        {
            this.Equipments.Remove(equipment);
        }

        public void Initialize()
        {
            this.Products = new List<Product>();
            this.Equipments = new List<Equipment>();
        }

        public void Find(Func<Product, bool> predicate, out Product product)
        {
            product = this.Products.FirstOrDefault(predicate);
        }

        public void Find(Func<Equipment, bool> predicate, out Equipment equipment)
        {
            equipment = this.Equipments.FirstOrDefault(predicate);
        }

        // public /*override*/ string ConvertToJson()
        // {
        //     // string result = "{\"Items\":[";

        //     // foreach (var item in this.content.Items)
        //     // {
        //     //     result = result + item.ConvertToJson() + ",";
        //     // }

        //     // result = result.Remove(result.Length - 1);
        //     // result = result + "]}";

        //     // string temp = JsonSerializer.Serialize(this.content);
        //     // return result;
        //     JsonSerializerOptions options = new()
        //     {
        //         ReferenceHandler = MyReferenceHandler.Instance,
        //         WriteIndented = true
        //     };

        //     return JsonSerializer.Serialize(this.content, options);            
        // }

        // public void LoadFromJson(string json)
        // {
        //     this.Initialize();
        //     // this.content = JsonSerializer.Deserialize<Content>(json);
        //     JsonSerializerOptions options = new()
        //     {
        //         ReferenceHandler = MyReferenceHandler.Instance,
        //         WriteIndented = true
        //     };

        //     this.content = JsonSerializer.Deserialize<Content>(json, options);
        // }

        // // Esta clase existe para representar el estado de la clase SharedObjects y poder serializarlo y deserializarlo y que pueda seguir siendo un singleton. La serialización en Json requiere un constructor público que la clase SharedObjects no puede tener por ser un singleton.
        // private class Content
        // {

        // }
    }
}