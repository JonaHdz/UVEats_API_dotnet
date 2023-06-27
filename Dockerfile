FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /UVEats_API_dotnet
COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o bin


FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /UVEats_API_dotnet
COPY --from=build-env /UVEats_API_dotnet/bin .
ENTRYPOINT [ "dotnet", "API_PROYECTO.dll" ]