namespace SobelFilterApp.Controllers

open System
open Microsoft.AspNetCore.Mvc
open SobelFilterApp.Services

[<ApiController>]
[<Route("[controller]")>]
type FilterController() =
    inherit ControllerBase()

    [<HttpPost("[action]")>]
    member _.Sobel([<FromForm>] formImage) : Async<string> =
        async {
            let computedImageData =
                FilterService.SobelEdgeDetection formImage
                |> Async.RunSynchronously

            return "data:image/jpeg;base64," + Convert.ToBase64String(computedImageData.ToArray())
        }
