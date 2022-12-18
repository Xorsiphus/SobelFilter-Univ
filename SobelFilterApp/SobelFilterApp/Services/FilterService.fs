module SobelFilterApp.Services.FilterService

open System
open System.IO
open Microsoft.AspNetCore.Http
open SkiaSharp

let SobelEdgeDetection (formImage: IFormFile) : Async<SKData> =
    async {
        use rStream = new MemoryStream()

        formImage.CopyToAsync(rStream)
        |> Async.AwaitTask
        |> ignore

        let codec = SKCodec.Create(rStream)
        let b = SKBitmap.Decode(codec)
        let bb = b.Copy()

        let width = b.Width
        let height = b.Height

        let gx =
            [| [| -1; 0; 1 |]
               [| -2; 0; 2 |]
               [| -1; 0; 1 |] |]

        let gy =
            [| [| 1; 2; 1 |]
               [| -2; 0; 2 |]
               [| -1; 0; 1 |] |]

        let allPixR =
            Array2D.init width height (fun _ _ -> 0)

        let allPixG =
            Array2D.init width height (fun _ _ -> 0)

        let allPixB =
            Array2D.init width height (fun _ _ -> 0)

        let limit = 128 * 128

        for i in 0 .. width - 1 do
            for j in 0 .. height - 1 do
                allPixR[i, j] <- Convert.ToInt32(b.GetPixel(i, j).Red)
                allPixG[i, j] <- Convert.ToInt32(b.GetPixel(i, j).Green)
                allPixB[i, j] <- Convert.ToInt32(b.GetPixel(i, j).Blue)

        for i in 1 .. width - 2 do
            for j in 1 .. height - 2 do
                let mutable newRx = 0
                let mutable newRy = 0
                let mutable newGx = 0
                let mutable newGy = 0
                let mutable newBx = 0
                let mutable newBy = 0

                for wi in -1 .. 1 do
                    for hw in -1 .. 1 do
                        let rc = allPixR[i + hw, j + wi]
                        newRx <- gx[wi + 1][hw + 1] * rc
                        newRy <- gy[wi + 1][hw + 1] * rc

                        let gc = allPixG[i + hw, j + wi]
                        newGx <- gx[wi + 1][hw + 1] * gc
                        newGy <- gy[wi + 1][hw + 1] * gc

                        let bc = allPixB[i + hw, j + wi]
                        newBx <- gx[wi + 1][hw + 1] * bc
                        newBy <- gy[wi + 1][hw + 1] * bc

                if newRx * newRx + newRy * newRy > limit
                   || newGx * newGx + newGy * newGy > limit
                   || newBx * newBx + newBy * newBy > limit then
                    bb.SetPixel(i, j, SKColors.White)
                else
                    bb.SetPixel(i, j, SKColors.Black)

        return bb.Encode(SKEncodedImageFormat.Jpeg, 100)
    }
