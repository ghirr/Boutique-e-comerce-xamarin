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
    public partial class DashAdmin : TabbedPage
    {
        ObservableCollection<Categorie> categoriesList;
        ObservableCollection<Produit> produitsList;
        ObservableCollection<Commande> commandesList;
        ObservableCollection<LigneCommande> lignesList;
        public DashAdmin()
        {
            InitializeComponent();
            ListerCategories();
            ListerProduits();
            VisualiserCommandesPassees();

           // ViderTablesAsync();

        }
        public async void ViderTablesAsync()
        {
            await App.Database.ViderTableLigneCommandeAsync();
            await App.Database.ViderTableCommandeAsync();
        }
        private async void OnCommandeSelected(object sender, SelectedItemChangedEventArgs e)
        {
            // Récupérez la commande sélectionnée
            Commande commande = (Commande)e.SelectedItem;

            try
            {
                // Obtenez les lignes de commande associées à la commande sélectionnée
                List<LigneCommande> lignesList = await App.Database.ObtenirLignesCommandeAsync(commande.Id);
                Console.WriteLine(lignesList.Count);
                if (lignesList != null && lignesList.Count > 0)
                {
                    Console.WriteLine(lignesList.Count);
                    // Affectez les lignes de commande à la source de données de la deuxième ListView
                    listViewLignesCommande.ItemsSource = lignesList;
                }
                else
                {
                    await DisplayAlert("Information", "Aucune ligne de commande trouvée.", "OK");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération des lignes de commande : {ex.Message}");
            }

     // Désélectionnez l'élément de la liste des commandes
     ((ListView)sender).SelectedItem = null;
        }
        //categories
        private async void AjouterCategorie(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nomCategorie.Text)) {
                await DisplayAlert("Erreur", "Veuillez remplir tous les champs correctement.", "OK");
                return;

            }

            else { 
            Categorie cat = new Categorie
            {
                Nom = nomCategorie.Text
            };

            await App.Database.AjouterCategorieAsync(cat);
            Console.WriteLine("ajout sar");
            ListerCategories();
            ListerProduits();
            CurrentPage = Children[1];
            nomCategorie.Text = string.Empty;
        }
        }
        public async void ListerCategories()
        {
            categoriesList = new ObservableCollection<Categorie>(await App.Database.ObtenirCategoriesAsync());
            categoriesListView.ItemsSource = categoriesList;
            // Associer la liste des catégories au Picker
            categoriePicker.ItemsSource = categoriesList;
            
            

            // Configurer le DisplayMemberPath pour spécifier le nom de la propriété à afficher
             categoriePicker.ItemDisplayBinding = new Binding("Nom");
        }

        private async void SupprimerCategorie(object sender, EventArgs e)
        {
            ImageButton boutonSupprimer = (ImageButton)sender;

            // Obtenez le contexte de liaison (l'objet de type Categorie associé à cette ligne)
            Categorie categorie = (Categorie)boutonSupprimer.BindingContext;

            // Obtenez l'ID de la catégorie
            int idCategorie = categorie.Id;
            await App.Database.SupprimerCategorieAsync(idCategorie);
            await App.Database.SupprimerProduitsByCategoryAsync(idCategorie);
            Console.WriteLine(idCategorie.ToString());
            Console.WriteLine("suppression");
            ListerCategories();
            ListerProduits();

        }
        private async void modifierCategorie(object sender, EventArgs e)
        {
            ImageButton boutonModifier = (ImageButton)sender;

            // Obtenez le contexte de liaison (l'objet de type Categorie associé à cette ligne)
            Categorie categorie = (Categorie)boutonModifier.BindingContext;

            // Obtenez l'ID de la catégorie
            int idCategorie = categorie.Id;
            String nomCategorie = categorie.Nom;
            await Navigation.PushAsync(new ModifierCategory(idCategorie,nomCategorie));
        }

        //-------------------------------------------------------------------------------------------------------
        //Produits
        private async void AjouterProduit(object sender, EventArgs e)
        {
            // Validation des champs
            if (string.IsNullOrWhiteSpace(nomProduitEntry.Text) ||
                string.IsNullOrWhiteSpace(descriptionProduitEntry.Text) ||
                string.IsNullOrWhiteSpace(imageProduitEntry.Text)||
                categoriePicker.SelectedItem == null ||
                !decimal.TryParse(prixProduitEntry.Text, out decimal prix))
            {
                await DisplayAlert("Erreur", "Veuillez remplir tous les champs correctement.", "OK");
                return;
            }
            else { 
            // Récupérer la catégorie sélectionnée dans le Picker
            Categorie selectedCategory = (Categorie)categoriePicker.SelectedItem;

            Produit p = new Produit
            {
                Nom = nomProduitEntry.Text,
                Description=descriptionProduitEntry.Text,
                Prix=int.Parse(prixProduitEntry.Text),
                UrlImage=imageProduitEntry.Text,
                IdCategorie=selectedCategory.Id

            };

            await App.Database.AjouterProduitAsync(p);
            Console.WriteLine("ajout sar");
            ListerProduits();
            CurrentPage = Children[3];

                // Vider les champs après l'ajout
                nomProduitEntry.Text = string.Empty;
                descriptionProduitEntry.Text = string.Empty;
                prixProduitEntry.Text = string.Empty;
                imageProduitEntry.Text = string.Empty;
                categoriePicker.SelectedItem = null;

            }
        }


        public async void ListerProduits()
        {
            produitsList = new ObservableCollection<Produit>(await App.Database.ObtenirProduitsAsync());
            produitsListView.ItemsSource = produitsList;

        }

        private async void OnItemLongPress(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is Produit selectedProduit)
            {
                // Retrieve the category for the selected product
                Categorie associatedCategorie = await App.Database.GetCategorieByIdAsync(selectedProduit.IdCategorie);
                // Display details using a popup or navigate to a details page
                await DisplayAlert("Produit Details", $"Nom: {selectedProduit.Nom}\nDescription: {selectedProduit.Description}\nPrix: {selectedProduit.Prix:C} \nCategorie: {associatedCategorie.Nom}", "OK");
            }
        }

        
                  private async void ModifierProduit(object sender, EventArgs e)
        {
            ImageButton boutonModifier = (ImageButton)sender;

            // Obtenez le contexte de liaison (l'objet de type Produit associé à cette ligne)
            Produit produit = (Produit)boutonModifier.BindingContext;

            await Navigation.PushAsync(new ModifierProduit(produit));
        }

        
                 private async void SupprimerProduit(object sender, EventArgs e)
        {
           ImageButton boutonModifier = (ImageButton)sender;

            // Obtenez le contexte de liaison (l'objet de type Produit associé à cette ligne)
            Produit produit = (Produit)boutonModifier.BindingContext;

            // Obtenez l'ID de la catégorie
            int idProduit = produit.Id;
            await App.Database.SupprimerProduitAsync(idProduit);
           // Console.WriteLine(idProduit.ToString());
            Console.WriteLine("suppression");
            ListerProduits();

        }

          private async void VisualiserCommandesPassees()
            {
                commandesList = new ObservableCollection<Commande>(await App.Database.ObtenirCommandesPasseesAsync());
               // lignesList = new ObservableCollection<LigneCommande>();

            // Afficher les commandes
            listViewCommandes.ItemsSource = commandesList;
            listViewCommandes.ItemSelected += OnCommandeSelected;

            // Afficher les lignes de commande
            listViewLignesCommande.ItemsSource = lignesList;
            /*   try
             {
                 // Récupérez la liste des commandes passées depuis la base de données
                 commandesList = new ObservableCollection<Commande>(await App.Database.ObtenirCommandesPasseesAsync());
                 lignesList = new ObservableCollection<LigneCommande>();

                 // Vérifiez si des commandes existent
                 if (commandesList != null && commandesList.Any())
                 {
                     foreach (Commande commande in commandesList)
                     {
                       //  Console.WriteLine($"Commande ID: {commande.Id}, Client: {commande.NomClient}");

                         foreach (LigneCommande ligneCommande in commande.LignesCommande)
                         {
                             // Console.WriteLine($"  Produit ID: {ligneCommande.IdProduit}, Quantité: {ligneCommande.Quantite}");
                             lignesList.Add(ligneCommande);

                         }
                     }
                 }
                 else
                 {
                     await DisplayAlert("Information", "Aucune commande passée trouvée.", "OK");
                 }
             }
             catch (Exception ex)
             {
                 Console.WriteLine($"Erreur lors de la récupération des commandes passées : {ex.Message}");
             }*/
        }



    }
}
