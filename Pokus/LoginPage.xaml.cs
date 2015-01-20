using System;
using System.Collections.Generic;

using Xamarin.Forms;

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
			var pass = passwordEntry.Text;
			var user = usernameEntry.Text;

//			await DisplayAlert("Clicked!",
//				pass+" " + user + "' has been clicked",
//				"OK");

			await Navigation.PushModalAsync(new MainPage());

		}

		public static Page GetMainPage()
		{
			var mainNav = new NavigationPage(new MainPage());
			return mainNav;
		}
	}
}

