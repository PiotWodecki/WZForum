using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ForumWZ.Data
{
    public interface IUpload
    {
        CloudBlobContainer GetBlobContainer(string connectionString);
    }
}
