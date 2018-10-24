FROM microsoft/dotnet:2.1-aspnetcore-runtime-alpine AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY PSD2Payment.csproj .
RUN dotnet restore
COPY . .
WORKDIR "/src/"
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
ENV DATABASE_TYPE InMemory
ENV DATABASE_NAME psd2
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "PSD2Payment.dll"]