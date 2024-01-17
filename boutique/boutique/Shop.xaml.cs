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
    public partial class Shop : ContentPage
    {
        ObservableCollection<Produit> produits;
        public Shop(int id)
        {
            InitializeComponent();
            listerProduits(id);
        }

      private async void   listerProduits(int id)
        {
            produits = new ObservableCollection<Produit>(await App.Database.ObtenirProduitsAsync(id));
            ProductsView.ItemsSource = produits;
        }

        private async void  Panier(object sender, EventArgs e)
        {
            Button boutonPanier = (Button)sender;

            // Obtenez le contexte de liaison (l'objet de type Categorie associé à cette ligne)
            Produit prod = (Produit)boutonPanier.BindingContext;
            if (!App.Cart.Any(p => p.Id == prod.Id))
            {
                // Add the product to the cart
                App.Cart.Add(prod);
            }
            else
            {
                await DisplayAlert("Alert", "le produit est deja ajouter", "OK");
                return;
            }


        }
    }
}