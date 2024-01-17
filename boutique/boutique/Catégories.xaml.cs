using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace boutique
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Catégories : ContentPage
    {
        ObservableCollection<Categorie> categories;
        public Catégories()
        {
            InitializeComponent();
            PopulateCategories();
        }

        private async void PopulateCategories()
        {
            categories = new ObservableCollection<Categorie>(await App.Database.ObtenirCategoriesAsync()); // Implement this method to get your category names
            foreach (Categorie categoryName in categories)
            {
                
                Button categoryButton = new Button
                {
                    Text = categoryName.Nom,
                    Margin = new Thickness(10),
                    BackgroundColor = Color.FromHex("#00A170"), // Set the background color
                    TextColor = Color.White, // Set the text color
                    CornerRadius = 20, // Set the corner radius
                    Padding = new Thickness(10, 5, 10, 5),
                    ImageSource = "shop.png",
                    ContentLayout = new Button.ButtonContentLayout(Button.ButtonContentLayout.ImagePosition.Right, 0), // Position the image to the right
                    Command = new Command(() => CategoryButton_Clicked(categoryName.Id)),
                };


                categoryStackLayout.Children.Add(categoryButton);
            }

        }
        private void CategoryButton_Clicked(int id)
        {
            Navigation.PushAsync(new Shop(id));

            }
    }
}