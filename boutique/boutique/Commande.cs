using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace boutique
{
   public class Commande
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public string NomClient { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<LigneCommande> LignesCommande { get; set; }

        public Commande()
        {
            // Initialize the LignesCommande list in the constructor
            LignesCommande = new List<LigneCommande>();
        }

    }

}
