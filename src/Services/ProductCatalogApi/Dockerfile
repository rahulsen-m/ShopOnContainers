FROM microsoft/dotnet:2.1.503-sdk AS build

WORKDIR /code

COPY . .

RUN dotnet restore

RUN dotnet publish --output /out/ --configuration Release

FROM microsoft/dotnet:2.1.503-sdk 

COPY --from=build /out /app/

WORKDIR /app

ENTRYPOINT ["dotnet","ProductCatalogApi.dll"]
