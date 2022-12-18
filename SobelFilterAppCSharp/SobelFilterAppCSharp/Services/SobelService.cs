using SkiaSharp;

namespace SobelFilterAppCSharp.Services;

public static class SobelService
{
    public static async Task<byte[]> SobelEdgeDetection(IFormFile? formImage)
    {
        using var rStream = new MemoryStream();
        await formImage?.CopyToAsync(rStream)!;
        var b = SKBitmap.Decode(rStream.ToArray());
        var bb = b.Copy();
        var width = b.Width;
        var height = b.Height;

        var gx = new[,] {{-1, 0, 1}, {-2, 0, 2}, {-1, 0, 1}};
        var gy = new[,] {{1, 2, 1}, {0, 0, 0}, {-1, -2, -1}};

        var allPixR = new int[width, height];
        var allPixG = new int[width, height];
        var allPixB = new int[width, height];

        const int limit = 128 * 128;

        for (var i = 0; i < width; i++)
        {
            for (var j = 0; j < height; j++)
            {
                allPixR[i, j] = b.GetPixel(i, j).Red;
                allPixG[i, j] = b.GetPixel(i, j).Green;
                allPixB[i, j] = b.GetPixel(i, j).Blue;
            }
        }

        for (var i = 1; i < b.Width - 1; i++)
        {
            for (var j = 1; j < b.Height - 1; j++)
            {
                var newRx = 0;
                var newRy = 0;
                var newGx = 0;
                var newGy = 0;
                var newBx = 0;
                var newBy = 0;
                for (var wi = -1; wi < 2; wi++)
                {
                    for (var hw = -1; hw < 2; hw++)
                    {
                        var rc = allPixR[i + hw, j + wi];
                        newRx += gx[wi + 1, hw + 1] * rc;
                        newRy += gy[wi + 1, hw + 1] * rc;

                        var gc = allPixG[i + hw, j + wi];
                        newGx += gx[wi + 1, hw + 1] * gc;
                        newGy += gy[wi + 1, hw + 1] * gc;

                        var bc = allPixB[i + hw, j + wi];
                        newBx += gx[wi + 1, hw + 1] * bc;
                        newBy += gy[wi + 1, hw + 1] * bc;
                    }
                }

                if (newRx * newRx + newRy * newRy > limit || newGx * newGx + newGy * newGy > limit ||
                    newBx * newBx + newBy * newBy > limit)
                    bb.SetPixel(i, j, SKColors.White);
                else
                    bb.SetPixel(i, j, SKColors.Black);
            }
        }

        return bb.Encode(SKEncodedImageFormat.Jpeg, 100).ToArray();
    }
}