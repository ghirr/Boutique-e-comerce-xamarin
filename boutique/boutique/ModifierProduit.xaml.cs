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
    public partial class ModifierProduit : ContentPage
    {
        ObservableCollection<Categorie> categoriesList;
        int idProduit;
        public ModifierProduit(Produit p)
        {
            InitializeComponent();
            listercategories();
            nomProduitEntry.Text = p.Nom;
            prixProduitEntry.Text = p.Prix.ToString();
            descriptionProduitEntry.Text = p.Description;
            imageProduitEntry.Text = p.UrlImage;
            idProduit = p.Id;


        }

        private async void listercategories()
        {
            categoriesList = new ObservableCollection<Categorie>(await App.Database.ObtenirCategoriesAsync());
            // Associer la liste des catégories au Picker
            categoriePicker.ItemsSource = categoriesList;



            // Configurer le DisplayMemberPath pour spécifier le nom de la propriété à afficher
            categoriePicker.ItemDisplayBinding = new Binding("Nom");
        }
        private async void Modifierproduit(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nomProduitEntry.Text) ||
               string.IsNullOrWhiteSpace(descriptionProduitEntry.Text) ||
               string.IsNullOrWhiteSpace(imageProduitEntry.Text) ||
               categoriePicker.SelectedItem == null ||
               !decimal.TryParse(prixProduitEntry.Text, out decimal prix))
            {
                await DisplayAlert("Erreur", "Veuillez remplir tous les champs correctement.", "OK");
                return;
            }
            else
            {
                // Récupérer la catégorie sélectionnée dans le Picker
                Categorie selectedCategory = (Categorie)categoriePicker.SelectedItem;
                Produit produit = new Produit
                {
                    Id = idProduit,
                    Nom = nomProduitEntry.Text,
                    Description = descriptionProduitEntry.Text,
                    Prix = int.Parse(prixProduitEntry.Text),
                    UrlImage = imageProduitEntry.Text,
                    IdCategorie = selectedCategory.Id
                };

                await App.Database.ModifierProduitAsync(produit);
                //   DashAdmin.


                // Déclencher l'événement PopCompleted
                PopCompleted(this, EventArgs.Empty);
                await Navigation.PopAsync();

                Console.WriteLine("Modification effectuée");

            }

        }
        private void PopCompleted(object sender, EventArgs e)
        {
            // Accéder à l'instance de DashAdmin
            DashAdmin dashAdminPage = Navigation.NavigationStack.LastOrDefault(p => p is DashAdmin) as DashAdmin;

            if (dashAdminPage != null)
            {
                dashAdminPage.ListerCategories(); // Appeler la fonction lister()
                dashAdminPage.ListerProduits();
            }
        }
    }
}