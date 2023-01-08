using Bogus;
using Drastic.ViewModels;
using SharedPlayground.Models;
using System.Collections.ObjectModel;

namespace SharedPlayground.ViewModels
{
    public class RecipeListViewModel : BaseViewModel
    {
        public List<Recipe> allRecipies = new List<Recipe>();
        public ObservableCollection<Recipe> Recipes { get; set; } = new ObservableCollection<Recipe>();

        public RecipeListViewModel(IServiceProvider services)
            : base(services)
        {
            var faker = new Faker<Recipe>()
                .RuleFor(r => r.RecipeName, f => string.Concat(f.Lorem.Words()))
                .RuleFor(r => r.Ingredients, f => f.Make(10, () => f.Lorem.Word()).ToArray())
                .RuleFor(r => r.ImageUrl, f => f.Image.PicsumUrl())
                .RuleFor(r => r.RecipeUrl, f => f.Internet.Url());

            this.allRecipies = faker.GenerateBetween(80, 120).ToList();
            this.Apply();
        }

        public void Apply(string searchTerm = "")
        {
            this.Recipes.Clear();
            var items = string.IsNullOrEmpty(searchTerm) ? this.allRecipies : this.allRecipies.Where(n => n.RecipeName.Contains(searchTerm));

            foreach (var item in items)
            {
                this.Recipes.Add(item);
            }
        }
    }
}
