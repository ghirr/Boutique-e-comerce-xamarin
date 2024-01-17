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
    public partial class Panier : ContentPage
    {
        ObservableCollection<Produit> produits;
        public Panier()
        {
            InitializeComponent();
            lister();
           // viderQuantite();
            CalculTotal();
        }

        private void viderQuantite()
        {
            foreach (Produit produit in produits)
            {
                produit.Quantite = 1;
            }

        }

        public void lister()
        {

            produits = new ObservableCollection<Produit>(App.Cart);
            listPanier.ItemsSource = produits;
        }

        private void CalculTotal()
        {
            decimal total = 0;
            foreach (Produit produit in produits)
            {
                if (produit.Quantite == 0) { produit.Quantite = 1; }
                total +=( produit.Prix * produit.Quantite);
            }

            // Mettez à jour le label du total
            totalPanier.Text = $"Total du panier : {total:C}";
        }
        private void Stepper_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            // Récupérez le Stepper
            Stepper stepper = (Stepper)sender;

            // Récupérez le produit correspondant
            Produit produit = (Produit)stepper.BindingContext;

            // Mettez à jour la quantité du produit
           // produit.Quantite = produit.;

            // Recalculez le total et mettez à jour le label
            CalculTotal();
        }
        private void SupprimerPanier(object sender, EventArgs e)
        { // Vérifiez si le produit existe toujours dans le panier

            ImageButton boutonModifier = (ImageButton)sender;

            // Obtenez le contexte de liaison (l'objet de type Produit associé à cette ligne)
            Produit produit = (Produit)boutonModifier.BindingContext;
                App.Cart.Remove(produit);
                lister();
                CalculTotal();
     

        }

        private async void Commander(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nomClient.Text))
            {
                await DisplayAlert("Erreur", "Taper votre nom", "OK");
                return;
            }
            // Vérifiez si le panier est vide
            if (App.Cart.Count == 0)
            {
                // Afficher un message d'erreur
                await DisplayAlert("Erreur", "Le panier est vide", "OK");
                return;
            }

            // Créez une nouvelle commande
            Commande commande = new Commande()
            {
                NomClient = nomClient.Text

            };

            Console.WriteLine(commande.Id);
            // Parcourez les produits du panier
            Console.WriteLine(listPanier.ItemsSource);
            Console.WriteLine(produits);
            Console.WriteLine(App.Cart);
            // Parcourez les produits du panier
            if (listPanier.ItemsSource != null)
            {
                foreach (Produit produit in listPanier.ItemsSource)
                {
                    if (produit != null)
                    {
                        Console.WriteLine(produit);
                        // Créez une nouvelle ligne de commande
                        LigneCommande ligneCommande = new LigneCommande()
                        {
                            IdProduit = produit.Id,
                            Quantite = produit.Quantite,
                            IdCommande = commande.Id
                        };
                        Console.WriteLine("mockla");
                        await App.Database.AjouterLigneCommandeAsync(ligneCommande);
                        Console.WriteLine("mockla");
                        // Ajoutez la ligne de commande à la commande
                        commande.LignesCommande.Add(ligneCommande);
                        Console.WriteLine("mockla");
                    }
                    else
                    {
                        Console.WriteLine("Élément de produit est null");
                    }
                }

                // Enregistrez la commande
                await App.Database.AjouterCommandeAsync(commande);

                // Vider le panier
                App.Cart.Clear();
                lister();
                nomClient.Text = "";

                

                // Afficher un message de confirmation
                await DisplayAlert("Confirmation", "Commande enregistrée", "OK");
            }
            else
            {
                Console.WriteLine("Liste des produits est null");
            }
        }

        }
}