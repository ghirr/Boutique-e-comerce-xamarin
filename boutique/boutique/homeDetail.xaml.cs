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
    public partial class homeDetail : ContentPage
    {
        public homeDetail()
        {
            InitializeComponent();
        }


        private async void Shop(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Catégories());
        }
    }
}