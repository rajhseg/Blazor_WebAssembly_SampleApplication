# Blazor_WebAssembly_SampleApplication
This repo consists of sample blazor web assembly application with implementation of JWT authentication and Repository pattern

Following key **"JwtConfig:key": "CuYM000OLlMQG6VVLp1OH7Xzyw3eHuw1qvUC5dcGt8FLI"** in **appsettings json** should be placed or stored under secured environment like (KeyValut)

Authentication Flow

-> Login with credentials 

      -> tokengenerate return encoded mapping token which is mapped against actual JWT token 
            (save actual token in db)
      
            -> delegateHandler(foreach httprequest Authorization header set by sending the encoded token) 
            
                  -> Middleware will run before auth middleware , where we take the encoded token 
                  and get the actual token from db, then assign to Authorization header.

                  (or)
                  
                  -> instead of middleware we can use onMessageReceived event to assign the actual token 
                  from db. two ways we can do the reassign.
                  
                        -> (based on JWTConfig in program.cs) Authorize Attribute validate the Claims.


