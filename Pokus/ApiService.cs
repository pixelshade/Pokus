using System;
using RestSharp.Portable;
using System.Net.Http;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net;
using System.IO;
using System.Collections.Generic;

namespace Pokus
{
	public class ApiService
	{
		private static ApiService instance;

		private static string URL_INFINARIO = "https://cloud.infinario.com";
		private static string URL_TEST = "http://test.skeletopedia.sk/";

		// specific parts URL
		private static string URL_DASHBOARD_API = "/api/dashboards/"; 
		private static string URL_DASHBOARD_LISTING = "/api/dashboards?company_id=";

		private static string URL_PART_LOGIN = "/login";




		private RestClient restClient;
		private CookieContainer cookieJar;

		public static ApiService GetInstance(){
			if (instance == null) {
				instance = new ApiService ();
			}
			return instance;
		}

		private ApiService ()
		{
//			restClient = new RestClient(new Uri(URL_TEST));
			cookieJar = new CookieContainer();

		}

		public async Task<string> GetCookieValue(string name){
			Uri uri = new Uri(URL_INFINARIO);
//			Uri uri = new Uri("infinario.com");
		
			var responseCookies = cookieJar.GetCookies (uri);

			string cookies = "";
			string val = null;
			foreach (Cookie cookie in responseCookies)
			{
				string cookieName = cookie.Name;
				string cookieValue = cookie.Value;
				cookies += cookieName + "=" + cookieValue + ",";
				if (cookieName == name) 
					val = cookieValue;
			}
			Debug.WriteLine ("Cookies pre uri:"+uri+" su "+cookies);
			return val;
		}

		public async Task<string> GetCompanyId(){
			var val = await GetCookieValue ("portal_last_company_id");
			if(val != null) return val;

			return "1e9b6026-9cba-11e4-b72b-b083fed290ff";
		}


		public async Task<string> GetDashboardsListingForCompanyId(string id){
			string result = await HttpAsync(URL_INFINARIO + URL_DASHBOARD_LISTING + id , HttpMethod.Get);

			return result;
		}
	



//		public async Task<string> HttpAsyncPost(string uri, string data){
//			var httpClient = new HttpClient();
//			var response = await httpClient.PostAsync(uri, new StringContent(data));
//
//			response.EnsureSuccessStatusCode();
//
//			string content = await response.Content.ReadAsStringAsync();
//			return content;
//		}



		public async Task<string> HttpAsync (string URL, HttpMethod method, List<KeyValuePair<string,string>> parameters = null)
		{
			Debug.WriteLine ("Visiting " + URL+ " method:" + method);
			GetCookieValue (URL);
			var handler = new HttpClientHandler ();
			handler.AllowAutoRedirect = true;
			handler.UseCookies = true;
			var uri = new Uri (URL);
			var cks = cookieJar.GetCookies (uri);
			foreach (Cookie cookie in cks) {
				handler.CookieContainer.Add (uri, new Cookie(cookie.Name,cookie.Value));
			}



			using (var client = new HttpClient(handler))
			{
				HttpResponseMessage response;
//				var request = new HttpRequestMessage (method, URL);
//				request.Headers.

//				client.SendAsync(request);
				if (method == HttpMethod.Get) {
					response = await client.GetAsync (URL);
				} else {
					var content = new FormUrlEncodedContent(parameters);

					response = await client.PostAsync (URL, content);
				}

				Debug.WriteLine (response.RequestMessage.ToString ());
				Debug.WriteLine (response.ReasonPhrase.ToString ());

//				foreach (var hd in response.RequestMessage.Content.) {
//					Debug.WriteLine ("----------"+hd.Key);
//					foreach (var val in hd.Value)
//						Debug.WriteLine (val);
//				}

				response.EnsureSuccessStatusCode ();
				cookieJar =	handler.CookieContainer;
				var responseString = await response.Content.ReadAsStringAsync();
				return responseString;
			}






//			string urlParams = this.DictionaryParamsToString (parameters);
//			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create (new Uri (URL));
//
//
//			request.ContentType = "application/json";
//			request.Method = (method==HttpMethod.Get) ? "GET" : "POST";
//			request.CookieContainer = cookies;
//				
//
//
//			var values = new List<KeyValuePair<string, string>>();
//			values.Add(new KeyValuePair<string, string>("thing1", "hello"));
//			values.Add(new KeyValuePair<string, string>("thing2", "world"));
//
//			var content = new FormUrlEncodedContent(values);
//
//			var response = await client.PostAsync("http://www.example.com/recepticle.aspx", content);
//
//			var responseString = await response.Content.ReadAsStringAsync();
//
//
//
//			// Send the request to the server and wait for the response:
//			using (WebResponse response = await request.GetResponseAsync ())
//			{
//				// Get a stream representation of the HTTP web response:
//				using (Stream stream = response.GetResponseStream ())
//				{
//					StreamReader reader = new StreamReader(stream);
//					string text = reader.ReadToEnd();
//					return text;
//				}
//			}
		}

		public async Task<string> GetWelcomeSession(){
			const string start_csrf_field = "<input id=\"csrf_token\" name=\"csrf_token\" type=\"hidden\" value=\"";
			const string end_csrf_field = "\">";

			string result = await HttpAsync (URL_INFINARIO + URL_PART_LOGIN, HttpMethod.Get );
			Debug.WriteLine (result);
			if (result != null) {
				var csrf_val = result.Substring (result.IndexOf (start_csrf_field)+start_csrf_field.Length, 100);
				Debug.WriteLine (csrf_val);
				csrf_val = csrf_val.Substring(0,csrf_val.IndexOf(end_csrf_field));
				Debug.WriteLine (csrf_val);
				return csrf_val; 
			}
			return null;
		}

		public async Task<bool>  LogIn(string username, string password){
//			var request = new RestRequest(URL_LOGIN, HttpMethod.Post);

//			request.AddQueryParameter ("username", username);
//			request.AddQueryParameter ("password", password);
//			var result = await restClient.Execute<string>(request);
			username = "psuldo@gmail.com";
			password = "457811";


			string csrf_token = await GetWelcomeSession ();
			if (csrf_token != null) {
				List<KeyValuePair<string,string>> httpParams = new List<KeyValuePair<string, string>> ();
				httpParams.Add (new KeyValuePair<string,string>("csrf_token", csrf_token));
				httpParams.Add (new KeyValuePair<string,string>("username", username));
				httpParams.Add (new KeyValuePair<string,string>("password", password));
				var result = await HttpAsync (URL_INFINARIO + URL_PART_LOGIN, HttpMethod.Post, httpParams);

				Debug.WriteLine (result);

				return !result.Contains("<title>Sign In");
			}

			string val = await GetCookieValue ("session");
			return false;	
		}


		public bool IsConnectedToInternet(){
			#if __IOS__
			if(Reachability.IsHostReachable(URL_INFINARIO)){
				return true;
			}
			return false;


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

			return true;
		}





		private string DictionaryParamsToString(Dictionary<string, string> parameters){
			string paramString = "";
			if (parameters != null) {
				bool first = true;
				foreach (var val in parameters) {
					if (first) {
						first = false;
					} else {
						paramString += "&";
					}
					paramString += string.Format ("{0}={1}", val.Key, val.Value);
				}
			}
			return paramString;
		}
	}
}

