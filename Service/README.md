# service for stock

## config

```shell
# 重置依赖
dotnet nuget locals all --clear
dotnet restore
# 添加依赖
dotnet add package CsvHelper --version 30.0.1

# 运行
dotnet run -- arg1 arg2 ...
```

## AOT

<https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot/>
<https://github.com/dotnet/runtime/blob/main/src/coreclr/nativeaot/docs/compiling.md>

```shell
apt-get install clang zlib1g-dev

dotnet publish --help

dotnet publish -c Release -o ./target

# 查看
dpkg --print-architecture
# https://learn.microsoft.com/en-us/dotnet/core/rid-catalog
dotnet publish -r linux-x64 -c Release -o ./target
# 通过.csproj配置?
dotnet publish -c Release -p:PublishAot=true -p:PublishTrimmed=true --sc -r win-x64 -o ./target
```
