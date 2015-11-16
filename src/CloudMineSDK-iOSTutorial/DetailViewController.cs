using System;

using UIKit;
using CloudMineSDK.Services;
using CloudmineSDK.Model;
using System.Threading.Tasks;
using CloudMineSDK.Model.Responses;

namespace CloudMineSDKiOSTutorial
{
	public partial class DetailViewController : UIViewController
	{
		public object DetailItem { get; set; }
		IAppObjectService appObjSrvc { get; set; }

		public DetailViewController (IntPtr handle) : base (handle)
		{
			CMApplication app = new CMApplication (AppDelegate.APP_ID, AppDelegate.API_KEY);
			IRestWrapper api = new PCLRestWrapper ();
			appObjSrvc = new CMAppObjectService (app, api);
		}

		public void SetDetailItem (object newDetailItem)
		{
			if (DetailItem != newDetailItem) {
				DetailItem = newDetailItem;
				
				// Update the view
				ConfigureView ();
			}
		}

		void ConfigureView ()
		{
			// Update the user interface for the detail item
			if (IsViewLoaded && DetailItem != null)
				detailDescriptionLabel.Text = DetailItem.ToString ();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			// Perform any additional setup after loading the view, typically from a nib.
			HCP hcp = new HCP () {
				ProviderName = "CloudMine Data Hospital",
				ProviderAddress = "1217 Sansom Street | Suite 600, Philadelphia, PA 19107",
				ProviderEmployeeCount = 200
			};

			Task<CMObjectResponse> objResponse = appObjSrvc.SetObject<HCP> (hcp);
			objResponse.ContinueWith (res => {
				Console.Write (objResponse.Result.HasErrors);
			});

			//Task<CMObjectSearchResponse<HCP>> searchResponse = appObjSrvc.SearchObjects<HCP> (@"[__class__=""HCPMock"", ProviderEmployeeCount=200]");
			//searchResponse.Wait ();

			//Console.Write (searchResponse.Result.HasErrors);

			ConfigureView ();
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}


