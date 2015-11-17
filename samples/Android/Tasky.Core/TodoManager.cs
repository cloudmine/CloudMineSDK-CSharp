using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CloudMineSDK.Model.Responses;

namespace Tasky.Core {
	/// <summary>
	/// Manager classes are an abstraction on the data access layers
	/// </summary>
	public static class TodoManager {

		static TodoManager ()
		{
		}
		
		public static Task<CMObjectFetchResponse<Todo>> GetTodo(string id)
		{
			return TodoService.GetTodo(id);
		}
		
		public static Task<CMObjectSearchResponse<Todo>> GetTodos ()
		{
			return TodoService.GetTodos();
		}
		
		public static Task<CMObjectResponse> SaveTodo (Todo item)
		{
			return TodoService.SaveTodo(item);
		}
		
		public static Task<CMObjectResponse> DeleteTodo(string id)
		{
			return TodoService.DeleteTodo(id);
		}
	}
}