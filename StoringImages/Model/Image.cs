using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoringImages.Model
{
    public class Image
    {
        public string FileName { get; set; }
        public byte[] ImageData { get; set; }
        public long ImageSize { get; set; }

        public Image(string fileName, byte[] imageData, long imageSize)
        {
            FileName = fileName;
            ImageData = imageData;
            ImageSize = imageSize;
        }
    }
}
