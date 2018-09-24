var script=document.createElement("script");
script.src=browser.extension.getURL("myscript.js");
script.async=false;
document.documentElement.appendChild(script);