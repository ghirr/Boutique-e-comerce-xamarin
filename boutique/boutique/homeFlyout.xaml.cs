using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace boutique
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class homeFlyout : ContentPage
    {
        public ListView ListView;

        public homeFlyout()
        {
            InitializeComponent();

            BindingContext = new homeFlyoutViewModel();
            ListView = MenuItemsListView;
        }

        class homeFlyoutViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<homeFlyoutMenuItem> MenuItems { get; set; }

            public homeFlyoutViewModel()
            {
                MenuItems = new ObservableCollection<homeFlyoutMenuItem>(new[]
                {
                    new homeFlyoutMenuItem { Id = 0, Title = "Catégories" ,TargetType=typeof(Catégories)},
                    new homeFlyoutMenuItem { Id = 1, Title = "Panier",TargetType=typeof(Panier) },
                    new homeFlyoutMenuItem { Id = 2, Title = "Administration",TargetType=typeof(Administration) },
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}