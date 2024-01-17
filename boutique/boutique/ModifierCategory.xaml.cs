using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace boutique
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ModifierCategory : ContentPage
    {
        private int id;
        public ModifierCategory(int idCategorie,String nomCat)
        {
            InitializeComponent();
          this.id = idCategorie;
            nomCategorie.Text = nomCat;
            

        }

        private async void ModifierProduit(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nomCategorie.Text))
            {
                await DisplayAlert("Erreur", "Veuillez remplir tous les champs correctement.", "OK");
                return;

            }
            else
            {
                Categorie cat = new Categorie
                {
                    Id = this.id,
                    Nom = nomCategorie.Text
                };

                await App.Database.ModifierCategorieAsync(cat);
                //   DashAdmin.


                // Déclencher l'événement PopCompleted
                PopCompleted(this, EventArgs.Empty);
                await Navigation.PopAsync();

                Console.WriteLine("Modification effectuée");
                nomCategorie.Text = string.Empty;

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