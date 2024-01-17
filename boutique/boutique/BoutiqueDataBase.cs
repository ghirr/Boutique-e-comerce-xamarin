using SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace boutique
{
  public  class BoutiqueDataBase
    {
        private readonly SQLiteAsyncConnection _baseDeDonnees;

        public BoutiqueDataBase(string cheminBaseDeDonnees)
        {
            _baseDeDonnees = new SQLiteAsyncConnection(cheminBaseDeDonnees);
            _baseDeDonnees.CreateTableAsync<Categorie>().Wait();
           _baseDeDonnees.CreateTableAsync<Produit>().Wait();
            _baseDeDonnees.CreateTableAsync<LigneCommande>().Wait();
            _baseDeDonnees.CreateTableAsync<Commande>().Wait();
        }

        // Opérations sur les catégories
        public Task<List<Categorie>> ObtenirCategoriesAsync()
        {
             return _baseDeDonnees.Table<Categorie>().ToListAsync();
        }

        public Task<int> AjouterCategorieAsync(Categorie categorie)
        {
          return  _baseDeDonnees.InsertAsync(categorie);
        }

        public Task<int> ModifierCategorieAsync(Categorie categorie)
        {
         return  _baseDeDonnees.UpdateAsync(categorie);
        }

        public Task<int> SupprimerCategorieAsync(int idCategorie)
        {
          return _baseDeDonnees.DeleteAsync<Categorie>(idCategorie);
        }
        public Task<Categorie> GetCategorieByIdAsync(int idCategorie)
        {
            return _baseDeDonnees.GetAsync<Categorie>(idCategorie);
        }

        
         // Opérations sur les produits
        public async Task<List<Produit>> ObtenirProduitsAsync(int idCategorie)
        {
            return await _baseDeDonnees.Table<Produit>().Where(p => p.IdCategorie == idCategorie).ToListAsync();
        }
        
        
        public Task<List<Produit>> ObtenirProduitsAsync()
        {
             return _baseDeDonnees.Table<Produit>().ToListAsync();
        }

        public Task<int> AjouterProduitAsync(Produit produit)
        {
           return _baseDeDonnees.InsertAsync(produit);
        }
        
        
        public Task<int> ModifierProduitAsync(Produit produit)
        {
           return _baseDeDonnees.UpdateAsync(produit);
        }

        public Task<int> SupprimerProduitAsync(int idProduit)
        {
           return  _baseDeDonnees.DeleteAsync<Produit>(idProduit);
        }
       
    public  Task<int> SupprimerProduitsByCategoryAsync(int idCategorie)
    {
            return _baseDeDonnees.Table<Produit>().DeleteAsync(p => p.IdCategorie == idCategorie);
          
        }
        
        // Opérations sur les lignes de commande
        public Task<int> AjouterLigneCommandeAsync(LigneCommande ligneCommande)
        {
          return _baseDeDonnees.InsertAsync(ligneCommande);
        }
        public Task<List<Commande>> ObtenirCommandesPasseesAsync()
        {
            return _baseDeDonnees.Table<Commande>().ToListAsync(); ;
        }

        // Opérations sur les commandes
        public Task<int> AjouterCommandeAsync(Commande commande)
        {
            return _baseDeDonnees.InsertAsync(commande);
        }

        public Task<List<LigneCommande>> ObtenirLignesCommandeAsync(int idCommande)
        {
            return _baseDeDonnees.Table<LigneCommande>().ToListAsync();
        }

        public async Task ViderTableLigneCommandeAsync()
        {
            await _baseDeDonnees.DeleteAllAsync<LigneCommande>();
        }

        public async Task ViderTableCommandeAsync()
        {
            await _baseDeDonnees.DeleteAllAsync<Commande>();
        }

    }
}
