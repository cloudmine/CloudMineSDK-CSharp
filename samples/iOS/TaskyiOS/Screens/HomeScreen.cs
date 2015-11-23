using System;
using System.Collections.Generic;
using System.Linq;
using UIKit;
using MonoTouch.Dialog;
using Tasky.Core;
using Tasky.ApplicationLayer;

namespace Tasky.Screens {

	/// <summary>
	/// A UITableViewController that uses MonoTouch.Dialog - displays the list of Tasks
	/// </summary>
	public class HomeScreen : DialogViewController {
		// 
		List<Todo> tasks;
		
		// MonoTouch.Dialog individual TaskDetails view (uses /AL/TaskDialog.cs wrapper class)
		BindingContext context;
		TaskDialog taskDialog;
		Todo currentTask;
		DialogViewController detailsScreen;

		public HomeScreen () : base (UITableViewStyle.Plain, null)
		{
			Initialize ();
		}
		
		protected void Initialize()
		{
			NavigationItem.SetRightBarButtonItem (new UIBarButtonItem (UIBarButtonSystemItem.Add), false);
			NavigationItem.RightBarButtonItem.Clicked += (sender, e) => { ShowTaskDetails(new Todo()); };
		}
		
		protected void ShowTaskDetails(Todo task)
		{
			currentTask = task;
			taskDialog = new TaskDialog (task);
			context = new BindingContext (this, taskDialog, "Task Details");
			detailsScreen = new DialogViewController (context.Root, true);
			ActivateController(detailsScreen);
		}
		public void SaveTask()
		{
			context.Fetch (); // re-populates with updated values
			currentTask.Name = taskDialog.Name;
			currentTask.Notes = taskDialog.Notes;
			TodoService.SaveTodo(currentTask).ContinueWith ( res => {
				Console.WriteLine ("SAVED");
			});
			NavigationController.PopViewController (true);
		}
		public void DeleteTask ()
		{
			if (!string.IsNullOrEmpty (currentTask.ID)) {
				TodoService.DeleteTodo (currentTask.ID).ContinueWith ( res => {
					Console.WriteLine ("DELETED");
				});
			}
			NavigationController.PopViewController (true);
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			
			// reload/refresh
			//PopulateTable();
			AsyncPopulateTable ();
		}
		
		protected void PopulateTable()
		{
			if (tasks == null)
				tasks = new List<Todo> (); 
			// make the CloudMine call via service (aync) and add the sections
			TodoService.GetTodos ().ContinueWith (res => {
				tasks = res.Result.Success.Values.ToList();
				var rows = from t in tasks
				          select (Element)new StringElement ((t.Name == "" ? "<new task>" : t.Name), t.Notes);
				var s = new Section ();
				s.AddAll (rows);
				Root = new RootElement ("Tasky") { s }; 
			});
		}

		protected async void AsyncPopulateTable()
		{
			var todos = await TodoService.GetTodos ();

			tasks = todos.Success.Values.ToList ();
			var rows = from t in tasks
				select (Element)new StringElement ((t.Name == "" ? "<new task>" : t.Name), t.Notes);
			var s = new Section ();
			s.AddAll (rows);
			Root = new RootElement ("Tasky") { s }; 
		}

		public override void Selected (Foundation.NSIndexPath indexPath)
		{
			var task = tasks[indexPath.Row];
			ShowTaskDetails(task);
		}
		public override Source CreateSizingSource (bool unevenRows)
		{
			return new EditingSource (this);
		}
		public void DeleteTaskRow(int rowId)
		{
			TodoService.DeleteTodo(tasks[rowId].ID).ContinueWith ( res => {
				Console.WriteLine ("DELETED");
			});
		}
	}
}