using System.Collections.Generic;
using ProjectAutomata;
using UnityEngine;
using System;
using System.Timers;
using Newtonsoft.Json;

namespace CustomMod
{
    public class CustomMod : Mod
    {
        [Serializable]
        public class MyBook
        {
            public List<MyRecipe> recipes;
        }

        [Serializable]
        public class MyRecipe
        {
            public string name;
            public List<MyIngredient> ingredients;
        }

        [Serializable]
        public class MyIngredient
        {
            public string name;
            public int amount;
            public string formula;
        }

        public override void OnModWasLoaded()
		{
            Debug.Log("Loading CustomMod");

            // Export as JSON all recipes
            string json;
            MyBook book = new MyBook {
                recipes = new List<MyRecipe>(),
            };

            foreach (var formula in GameData.instance.GetAssets<Recipe>())
            {
                MyRecipe recipe = new MyRecipe
                {
                    name = formula.name,
                    ingredients = new List<MyIngredient>(),
                };

                if(formula.ingredients.Count() > 0)
                {
                    foreach (var product in formula.ingredients)
                    {
                        MyIngredient ingredient = new MyIngredient
                        {
                            amount = product.amount,
                        };

                        if (product.definition)
                        {
                            ingredient.name = product.definition.name;
                            ingredient.formula = product.definition.price.formula;
                        }

                        recipe.ingredients.Add(ingredient);
                    }
                }

                book.recipes.Add(recipe);
            }

            json = JsonConvert.SerializeObject(book);
            Debug.Log(json);


            // Change some game formulae and Vehicle attributes
            // Source: https://github.com/pjf/TransportCostsRebalanced/blob/master/source/MyClass.cs
            var formulae = new Dictionary<string, string>
            {
                { "AwhDispatchCost",               "(   0 + 10 * distance) * difficulty" },
                { "ManualDestinationDispatchCost", "(   0 + 10 * distance) * difficulty" },
                { "TruckDepotDispatchCost",        "( 150 + 10 * distance) * difficulty" },
                { "TrainTerminalDispatchCost",     "( 500 + 12 * distance) * difficulty" },
                { "BoatDepotDispatchCost",         "( 500 + 16 * distance) * difficulty" },
                { "ZeppelinFieldDispatchCost",     "(1000 + 30 * distance) * difficulty" }
            };

            foreach (var formula in formulae) {
                GameData.instance.GetAsset<Formula>(formula.Key).formula = formula.Value;
            }

            GameData.instance.GetAsset<Vehicle>("CargoBoat").maxSpeed = 1;

        }
    }
}
