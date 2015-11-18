using System;
using System.Linq;
using System.Collections.Generic;

using Mono.Data.Sqlite;
using System.IO;
using System.Data;
using CloudmineSDK.Model;
using CloudMineSDK.Services;
using System.Threading.Tasks;
using CloudMineSDK.Model.Responses;

namespace Tasky.Core
{
	/// <summary>
	///
	/// </summary>
	public class TodoService 
	{
		private static CMApplication app { get; set; }
		private static IRestWrapper restWrapper { get; set; }
		private static IAppObjectService appObjectService { get; set; }

		protected static TodoService self;	

		static TodoService ()
		{
			self = new TodoService();
		}

		protected TodoService () {
			string appID = "";
			string apiKey = "";

			app = new CMApplication (appID, apiKey);
			restWrapper = new PCLRestWrapper ();
			appObjectService = new CMAppObjectService(app, restWrapper);
		}

		public static Task<CMObjectSearchResponse<Todo>> GetTodos ()
		{
			return  appObjectService.SearchObjects<Todo> (@"[__class__=""Todo""]");
		}

		public static Task<CMObjectFetchResponse<Todo>> GetTodo (string id) 
		{
			return appObjectService.GetObject<Todo> (id);
		}

		public static Task<CMObjectResponse> SaveTodo (Todo item) 
		{
			return appObjectService.SetObject<Todo> (item);
		}

		public static Task<CMObjectResponse> DeleteTodo(string id) 
		{
			return appObjectService.DeleteObject (id);
		}
	}
}