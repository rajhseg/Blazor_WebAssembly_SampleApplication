﻿function callCsharpMethod() {
    DotNet.invokeMethodAsync("WebAssemblyApp.Client", "GetMessageFromStaticMethod2").then((result) => {
        console.log(result);
        alert(result);
    });
}

function callCsharpInstanceMethod(instance) {
    console.log("called from csharp wasm");
    instance.invokeMethodAsync("GetMessageFromInstanceMethod").then((result) => {
        console.log(result);
        //alert(result);
    });
}

function expandTable(id) {
    var ele = document.getElementById(id);
    if (ele.style.display == "none" || ele.style.display == "") {
        ele.style.display = "block";
    } else {
        ele.style.display = "none";
    }
}