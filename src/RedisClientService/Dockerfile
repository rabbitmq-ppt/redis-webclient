#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
COPY ./bin/Debug/net5.0 ./
EXPOSE 80
ENTRYPOINT ["dotnet", "RedisClientService.dll"]