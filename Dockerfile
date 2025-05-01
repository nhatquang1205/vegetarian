FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
EXPOSE 80
EXPOSE 443
RUN apt-get update \
    && apt-get install -y --no-install-recommends apt-utils \
    && apt-get install -y --allow-unauthenticated \
       libgdiplus \
       libc6-dev \
       libx11-dev \
       unzip \
       nano \
       vim \
    && curl -sSL https://aka.ms/getvsdbgsh | /bin/sh /dev/stdin -v latest -l /root/vsdbg \
    && rm -rf /var/lib/apt/lists/*
WORKDIR /src

# Sao chép file csproj và restore các package
COPY ["vegetarian.csproj","./"]
RUN dotnet restore "./vegetarian.csproj"

# Sao chép toàn bộ mã nguồn và build ứng dụng
COPY . .
RUN dotnet publish "vegetarian.csproj" -c Release -o /app/publish

# Stage 2: Tạo image runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy output từ stage build
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "vegetarian.dll"]
