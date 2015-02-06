using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Pokus.Controls
{
	public partial class InfinarioWebViewer : ContentView
	{
		private static string URL_INFINARIO = "https://cloud.infinario.com/";


		public InfinarioWebViewer ()
		{
			InitializeComponent ();
			webViewer.Source = URL_INFINARIO;

		}



		public void Navigate(string URL){
			webViewer.Source = URL;
		}



		public void DisplayDashBoard(Dashboard dashboard){

		}
	}
}

