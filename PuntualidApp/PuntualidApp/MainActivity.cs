using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PuntualidApp
{
    [Activity(Label = "PuntualidApp", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        int count = 1;

        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.MyButton);

            button.Click += delegate { button.Text = string.Format("{0} clicks!", count++); };         
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://services.unac.edu.co/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // HTTP GET
            HttpResponseMessage response = await client.GetAsync("api/Addresses/GetNivelesNomen");
            if (response.IsSuccessStatusCode)
            {
                var nomen = await response.Content.ReadAsStringAsync();
                Console.WriteLine(nomen);
                Console.ReadLine();
            }
        }
    }
}

