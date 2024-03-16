# Blazor_WebAssembly_SampleApplication
This repo consists of sample blazor web assembly application with implementation of JWT authentication and Repository pattern

Following key **"JwtConfig:key": "CuYM000OLlMQG6VVLp1OH7Xzyw3eHuw1qvUC5dcGt8FLI"** in **appsettings json** should be placed or stored under secured environment like (KeyValut)

**Authorization Flow**
**Blazor Webassembly**
**Login with credentials -> tokengenerate return token -> delegateHandler(foreach httprequest Authorization header set) -> (based on JWTConfig in program.cs) Authorize Attribute validate.**


