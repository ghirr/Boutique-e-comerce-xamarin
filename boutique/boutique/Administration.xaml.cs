using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace boutique
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Administration : ContentPage
    {
        public Administration()
        {
            InitializeComponent();
        }
        void Login(object sender, EventArgs e)
        {
            if (email.Text == "admin@gmail.com" && password.Text == "admin123")
            {
                Console.WriteLine("Mriguel");
                Navigation.PushAsync(new DashAdmin());

            }
            else
            {
                Console.WriteLine("Informations de connexion incorrectes !");
                //System.Diagnostics.Debug.WriteLine("moch mriguel");
               // Navigation.PushAsync(new Panier());
               // DependencyService.Get<IToast>().Show("Informations de connexion incorrectes");
            }
        }


    }

    internal interface IToast
    {
       
            void Show(string message);
        

    }
}