using Bogus;
using Drastic.ViewModels;
using Mastonet.Entities;
using SharedPlayground.Generators;
using System.Collections.ObjectModel;

namespace SharedPlayground.ViewModels
{
    public class MastonetViewModel : BaseViewModel
    {
        public ObservableCollection<Status> Statuses { get; set; } = new ObservableCollection<Status>();

        public MastonetViewModel(IServiceProvider services)
            : base(services)
        {
            var faker = new FakeStatus();
            for (var i = 0; i < 100; i++)
            {
                this.Statuses.Add(faker.Generate());
            }
        }
    }
}
