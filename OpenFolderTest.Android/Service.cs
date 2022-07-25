using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Core.Content;
using Java.IO;
using OpenFolderTest.Droid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[assembly: Xamarin.Forms.Dependency(typeof(MyService))]
namespace OpenFolderTest.Droid
{
    public class MyService : IMyService
    {
        public void OpenFolder()
        {
            Intent intent = new Intent(Intent.ActionView);
            var path = "/storage/emulated/0/Documents/";
            var uri = FileProvider.GetUriForFile(MainActivity.AndroidContext, MainActivity.AndroidContext.PackageName + ".provider", new Java.IO.File(path));
            intent.SetFlags(ActivityFlags.GrantReadUriPermission);
            intent.SetFlags(ActivityFlags.NewTask);
            intent.SetDataAndType(uri, "*/*");
            MainActivity.AndroidContext.StartActivity(intent);
        }
    }
}