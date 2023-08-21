using Newtonsoft.Json;
using System;
using Plugin.FirebaseAuth;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DesignAlley
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        private const string ApiUrl = "https://objective-wright.69-49-231-148.plesk.page/checkingPhonenumbers/";
        public Login()
        {
            InitializeComponent();
        }

        
        private async void OnSubmitButtonClicked(object sender, EventArgs e)
        {
            string phoneNumber = phoneNumberEntry.Text;

            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                resultLabel.Text = "Please enter a phone number.";
                return;
            }

            var requestData = new { phoneNumber = phoneNumber };

            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    string jsonRequest = JsonConvert.SerializeObject(requestData);
                    StringContent content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await httpClient.PostAsync(ApiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();


                        var jsonResponse = JsonConvert.DeserializeObject<ResponseClass>(responseContent);


                        resultLabel.Text = "Phone number is registered.";

                       await Navigation.PushAsync(new ResendOtp());

                    }
                    else
                    {
                        resultLabel.Text = "Phone Number is not registered.";
                    }
                }
                catch (Exception ex)
                {
                    resultLabel.Text = "An error occurred: " + ex.Message;
                }
            }
        }
    }
}