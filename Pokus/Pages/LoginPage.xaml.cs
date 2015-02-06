using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Diagnostics;

namespace Pokus
{
	public partial class LoginPage : ContentPage
	{


		public LoginPage ()
		{
			InitializeComponent ();
		}



		async void OnButtonClicked(object sender, EventArgs args)
		{

//			if (!ApiService.GetInstance ().IsConnectedToInternet ()) {
//				DisplayAlert ("Offline", "Currently you are working offline", "Ok");
//			} else {
//				await Navigation.PushModalAsync(new MainPage());
//			}
			var pass = passwordEntry.Text;
			var user = usernameEntry.Text;

			bool isLoggedIn = await ApiService.GetInstance ().LogIn (user, pass);
			if (isLoggedIn) {
				await Navigation.PushModalAsync(new MainPage());
			}


		}




		public static Page GetMainPage()
		{
			var mainNav = new NavigationPage(new MainPage());
			return mainNav;
		}
	}
}

