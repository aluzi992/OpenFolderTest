using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.OS.Storage;
using Android.Provider;
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
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(MyService))]
namespace OpenFolderTest.Droid
{
    public class MyService : IMyService
    {
        public void OpenFolder2()
        {
            StorageManager sm = (StorageManager)MainActivity.Instance.GetSystemService(Context.StorageService);
            Intent intent = sm.PrimaryStorageVolume.CreateOpenDocumentTreeIntent();
            String startDir = "Documents"; // You could change startDir to open other dirs, such as DCIM, Downloads...
            var uri1 = intent.GetParcelableExtra("android.provider.extra.INITIAL_URI");
            String scheme = uri1.ToString();
            scheme = scheme.Replace("/root/", "/document/");
            scheme += "%3A" + startDir;
            var uri = Android.Net.Uri.Parse(scheme);
            intent.PutExtra("android.provider.extra.INITIAL_URI", uri);
            //MainActivity.Instance.StartActivityForResult(intent, 200);
            MainActivity.Instance.StartActivity(intent);
        }

        public void OpenFolder()
        {
            Intent intent = new Intent(Intent.ActionView);
            var path = "/storage/emulated/0/Documents/BINOA/";
            var uri = FileProvider.GetUriForFile(MainActivity.AndroidContext, MainActivity.AndroidContext.PackageName + ".fileprovider", new Java.IO.File(path));
            intent.SetFlags(ActivityFlags.GrantReadUriPermission);
            intent.SetFlags(ActivityFlags.NewTask);
            intent.SetDataAndType(uri, "*/*");
            //intent.SetDataAndType(uri, "resource/folder");
            //intent.SetDataAndType(uri, DocumentsContract.Document.MimeTypeDir);
            MainActivity.AndroidContext.StartActivity(intent);
        }

        // Saving photos requires android.permission.WRITE_EXTERNAL_STORAGE in AndroidManifest.xml
        public async Task<bool> SaveToGallery(byte[] data, string folder, string filename)
        {
            try
            {
                File picturesDirectory;
                File folderDirectory;
                Android.Net.Uri u = null;
                if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Q)
                {
                    ContentValues values = new ContentValues();
                    values.Put(MediaStore.IMediaColumns.MimeType, "image/jpeg");
                    values.Put(MediaStore.IMediaColumns.DateAdded, Java.Lang.JavaSystem.CurrentTimeMillis());
                    values.Put(MediaStore.IMediaColumns.DisplayName, filename);
                    //var cr = MainActivity.Instance.ContentResolver;
                    var cr = MainActivity.AndroidContext.ContentResolver;
                    u = cr.Insert(MediaStore.Images.Media.ExternalContentUri, values);
                    var s = cr.OpenOutputStream(u);
                    s.Write(data, 0, data.Length);
                    s.Close();

                    values.Clear();
                    values.Put(MediaStore.IMediaColumns.IsPending, false);
                    int updated = cr.Update(u, values, null, null);

                }
                else
                {
                    picturesDirectory = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures);
                    folderDirectory = picturesDirectory;
                    if (!string.IsNullOrEmpty(folder))
                    {
                        folderDirectory = new File(picturesDirectory, folder);
                        folderDirectory.Mkdirs();
                    }

                    using (File bitmapFile = new File(folderDirectory, filename))
                    {
                        bitmapFile.CreateNewFile();

                        using (FileOutputStream outputStream = new FileOutputStream(bitmapFile))
                        {
                            await outputStream.WriteAsync(data);
                        }

                        // Make sure it shows up in the Photos gallery promptly.
                        MediaScannerConnection.ScanFile(MainActivity.Instance,
                                                        new string[] { bitmapFile.Path },
                                                        new string[] { "image/png", "image/jpeg" }, null);
                    }
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        public string getDataColumn(Android.Net.Uri uri, string selection, string[] selectionArgs)
        {
            var cr = MainActivity.AndroidContext.ContentResolver;
            Android.Database.ICursor cursor = null;
            string column = "_data";
            string[] projection = { column };

            try
            {
                cursor = cr.Query(uri, projection, selection, selectionArgs, null);
                if (cursor != null && cursor.MoveToFirst())
                {
                    int index = cursor.GetColumnIndexOrThrow(column);
                    return cursor.GetString(index);
                }
            }
            finally
            {
                if (cursor != null)
                    cursor.Close();
            }
            return null;
        }
    }
}