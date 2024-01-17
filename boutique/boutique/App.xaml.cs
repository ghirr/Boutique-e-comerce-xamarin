using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using System.Collections.Generic;

namespace boutique
{
    public partial class App : Application
    {
        static BoutiqueDataBase database;
        public static List<Produit> Cart { get; private set; }
        public static BoutiqueDataBase Database
        {
            get
            {
                if (database == null)
                {
                    database = new BoutiqueDataBase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "boutique.db3"));
                }
                return database;
            }
        }
        public App()
        {
            InitializeComponent();

            MainPage = new home();
        }

        protected override void OnStart()
        {
            Cart = new List<Produit>();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
