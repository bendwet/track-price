FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base

#RUN apt-get update

RUN apt-get update && apt-get install -y xorg openbox libnss3 libasound2

#RUN apt-get install -y fonts-liberation gconf-service libappindicator1 libasound2 libatk1.0-0 libcairo2 libcups2 libfontconfig1 libgbm-dev libgdk-pixbuf2.0-0 libgtk-3-0 libicu-dev libjpeg-dev libnspr4 libnss3 libpango-1.0-0 libpangocairo-1.0-0 libpng-dev libx11-6 libx11-xcb1 libxcb1 libxcomposite1 libxcursor1 libxdamage1 libxext6 libxfixes3 libxi6 libxrandr2 libxrender1 libxss1 libxtst6 xdg-utils

RUN useradd -u 1001 spendy
USER spendy

WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["PriceRetriever/PriceRetriever.csproj", "PriceRetriever/"]
RUN dotnet restore "PriceRetriever/PriceRetriever.csproj"
COPY . .
WORKDIR "/src/PriceRetriever"
RUN dotnet build "PriceRetriever.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PriceRetriever.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PriceRetriever.dll"]
