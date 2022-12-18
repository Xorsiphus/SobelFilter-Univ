namespace SobelFilterApp

#nowarn "20"

open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting

module Program =
    let exitCode = 0

    [<EntryPoint>]
    let main args =

        let builder =
            WebApplication.CreateBuilder(args)

        builder.Services.AddControllers()

        builder.Services.AddCors (fun policies ->
            policies.AddDefaultPolicy (fun p ->
                p
                    .WithOrigins(
                        "https://localhost:7189"
                    )
                    .AllowAnyMethod()
                    .AllowAnyHeader
                |> ignore))

        let app = builder.Build()

        app.UseHttpsRedirection()

        app.UseAuthorization()
        app.MapControllers()
        app.UseCors()

        app.Run()

        exitCode
