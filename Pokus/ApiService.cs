using System;
using RestSharp.Portable;
using System.Net.Http;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Pokus
{
	public class ApiService
	{
		private static string URL_INFINARIO = "https://cloud.infinario.com";

		// specific parts URL
		private static string URL_DASHBOARD_API = "/api/dashboards/"; 
		private static string URL_LOGIN = "/login";

		private RestClient restClient;

		public ApiService ()
		{
			restClient = new RestClient(new Uri(URL_INFINARIO));


		}






		public async Task<Dashboard> GetDashboard(string id){
			var request = new RestRequest(URL_DASHBOARD_API, HttpMethod.Get);

			Dashboard dashboard = (Dashboard)await restClient.Execute<Dashboard> (request);
			return dashboard;
		}





		public async Task<bool>  LogIn(string username, string password){
			var request = new RestRequest(URL_LOGIN, HttpMethod.Post);
			request.AddQueryParameter ("username", username);
			request.AddQueryParameter ("password", password);
			var result = await restClient.Execute(request);

			Debug.WriteLine ((result));

			return true;			

		}


		public bool IsConnectedToInternet(){
			#if __IOS__
			if(!Reachability.IsHostReachable(URL_INFINARIO)) {
			return false;
			}
			else
			{
			return true;
			}


			#endif

			#if __ANDROID__
			var connectivityManager = (ConnectivityManager)GetSystemService(ConnectivityService);

			var activeConnection = connectivityManager.ActiveNetworkInfo;
			if ((activeConnection != null)  && activeConnection.IsConnected)
			{
			return true;
			}
			return false;
			#endif
			return false;
		}
	}
}

