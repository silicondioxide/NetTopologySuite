﻿using System.IO;

namespace NetTopologySuite.IO
{
    public partial class MyShapeFileReader
    {
        private Stream GetStream(string filePath)
        {
            return File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        }
    }
}