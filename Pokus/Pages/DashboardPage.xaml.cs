using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Diagnostics;
using Pokus.Controls;
using System.Net.Http;

namespace Pokus
{
	public partial class DashboardPage : ContentPage
	{
//		private WebView browser = new WebView ();


		public DashboardPage ()
		{
			InitializeComponent ();
//			InitializeDashboard ();



		}

		async void OnButtonClicked(object sender, EventArgs args)
		{
//			string cont = await ApiService.GetInstance ().HttpAsync ("https://cloud.infinario.com", HttpMethod.Get);
//			var parameters = new List<KeyValuePair<string, string>>();
//			parameters.Add(new KeyValuePair<string, string>("thing1", "hello"));
//			parameters.Add(new KeyValuePair<string, string>("thing2", "world"));
//			string cont = await ApiService.GetInstance ().HttpAsync ("http://test.skeletopedia.sk/post.php", HttpMethod.Post, parameters);

		}

		async public void InitializeDashboard(){
//			InfinarioWebViewer webViewer = new InfinarioWebViewer ();
			string companyId = await ApiService.GetInstance ().GetCompanyId ();
			string cont = await ApiService.GetInstance ().GetDashboardsListingForCompanyId (companyId);
			HtmlWebViewSource htmlSource = new HtmlWebViewSource ();
			htmlSource.Html = cont;
			WebViewer.Source = htmlSource;
			Debug.WriteLine ("YOLO\n"+cont);

		}





	}
}

