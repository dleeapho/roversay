FROM microsoft/dotnet-preview

RUN mkdir -p /dotnetapp
COPY . /dotnetapp
WORKDIR /dotnetapp
RUN dotnet restore
RUN dotnet build

