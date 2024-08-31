FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /MovieApi

COPY . .

RUN dotnet restore "/MovieApi/MovieApi.csproj"

RUN dotnet publish -c release -o /out


FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /out

COPY --from=build /out .

ENTRYPOINT ["dotnet", "MovieApi.dll"]    
