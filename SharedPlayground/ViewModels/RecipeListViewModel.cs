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
                .RuleFor(r => r.RecipeName, f => string.Concat(f.Lorem.Words()))
                .RuleFor(r => r.Ingredients, f => f.Make(10, () => f.Lorem.Word()).ToArray())
                .RuleFor(r => r.ImageUrl, f => f.Image.PicsumUrl())
                .RuleFor(r => r.RecipeUrl, f => f.Internet.Url());

            for (var i = 0; i < 100; i++)
            {
                this.Recipes.Add(faker.Generate());
            }
        }
    }
}
