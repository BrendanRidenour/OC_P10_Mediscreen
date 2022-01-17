#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Mediscreen.DiabetesAssessment.WebApi/Mediscreen.DiabetesAssessment.WebApi.csproj", "src/Mediscreen.DiabetesAssessment.WebApi/"]
RUN dotnet restore "src/Mediscreen.DiabetesAssessment.WebApi/Mediscreen.DiabetesAssessment.WebApi.csproj"
COPY . .
WORKDIR "/src/src/Mediscreen.DiabetesAssessment.WebApi"
RUN dotnet build "Mediscreen.DiabetesAssessment.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Mediscreen.DiabetesAssessment.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mediscreen.DiabetesAssessment.WebApi.dll"]