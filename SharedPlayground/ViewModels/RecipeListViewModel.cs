using Bogus;
using Drastic.ViewModels;
using SharedPlayground.Models;
using System.Collections.ObjectModel;

namespace SharedPlayground.ViewModels
{
    public class RecipeListViewModel : BaseViewModel
    {
        public ObservableCollection<Recipe> Recipes { get; set; } = new ObservableCollection<Recipe>();

        public RecipeListViewModel(IServiceProvider services)
            : base(services)
        {
            var faker = new Faker<Recipe>()
                .RuleFor(r => r.RecipeName, f => f.Lorem.Word())
                .RuleFor(r => r.Ingredients, f => f.Make(10, () => f.Lorem.Word()))
                .RuleFor(r => r.ImageUrl, f => f.Lorem.Word())
                .RuleFor(r => r.RecipeUrl, f => f.Lorem.Word());

            for (var i = 0; i < 100; i++)
            {
                this.Recipes.Add(faker.Generate());
            }
        }
    }
}
