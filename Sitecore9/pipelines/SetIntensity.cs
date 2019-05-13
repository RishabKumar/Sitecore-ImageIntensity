using Sitecore.Data.Fields;
using Sitecore.Pipelines.Upload;
using Sitecore.SecurityModel;
using System.Drawing;

namespace Sitecore9.pipelines
{
    public class SetIntensity
    {
        public void Process(UploadArgs args)
        {
            Bitmap bmp = new Bitmap(args.Files[0].InputStream);
            var w = bmp.Width;
            var h = bmp.Height;
            var R = 0;
            var G = 0;
            var B = 0;

            for (int i = 0; i < w; i++)
            {
                for(int j = 0; j < h; j++)
                {
                    var color = bmp.GetPixel(i, j);
                    R += color.R;
                    G += color.G;
                    B += color.B;
                }
            }
            var totalPixels = w * h;
            R = R / totalPixels;
            G = G / totalPixels;
            B = B / totalPixels;
            var result = string.Empty;
            if (((R * 299) + (G * 587) + (B * 114)) > 186000)
            {
                result = "font color should be BLACK";
            }
            else
            {
                result = "font color should be WHITE";
            }

            var item = args.UploadedItems[0];
            using (new SecurityDisabler())
            {
                item.Editing.BeginEdit();
                LookupField lf = item.Fields["Intensity"];  // The media item should have "Intensity" field for this to work.
                lf.Value = result;
                item.Editing.EndEdit();
            }

        }
    }
}