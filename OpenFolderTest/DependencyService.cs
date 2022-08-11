using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OpenFolderTest
{
    public interface IMyService
    {
        void OpenFolder();
        Task<bool> SaveToGallery(byte[] data, string folder, string filename);
    }
}
