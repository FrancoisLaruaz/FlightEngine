/*
				const time = Date.now();
				const callback = arguments[0];
				const handleDocumentLoaded = () => {
				  //document.getElementById("injected-time").innerHTML = navigator.webdriver;
				};
				if (document.readyState === "loading") {
				  document.addEventListener("DOMContentLoaded", handleDocumentLoaded);
				} else {
				  handleDocumentLoaded();
				}


document.getElementById("injected-time").innerHTML = navigator.webdriver;
*/
console.log('add script after')
// overwrite the `languages` property to use a custom getter
Object.defineProperty(navigator, "languages", {
  get: function() {
    return ["en", "es"];
  }
});

// Overwrite the `plugins` property to use a custom getter.
Object.defineProperty(navigator, "plugins", {
  get: () => new Array(Math.floor(Math.random() * 6) + 1),
});

// Pass the Webdriver test
Object.defineProperty(navigator, "webdriver", {
  get: () => false,
});

// hairline: store the existing descriptor
const elementDescriptor=Object.getOwnPropertyDescriptor(HTMLElement.prototype, "offsetHeight");

// redefine the property with a patched descriptor
Object.defineProperty(HTMLDivElement.prototype, "offsetHeight", {
    ...elementDescriptor,
  get: function() {
    if (this.id === "modernizr") {
      return 1;
    }
    return elementDescriptor.get.apply(this);
  },
});

["height", "width"].forEach(property => {
  // store the existing descriptor
  const imageDescriptor=Object.getOwnPropertyDescriptor(HTMLImageElement.prototype, property);

  // redefine the property with a patched descriptor
  Object.defineProperty(HTMLImageElement.prototype, property, {
    ...imageDescriptor,
    get: function() {
      // return an arbitrary non-zero dimension if the image failed to load
      if (this.complete && this.naturalHeight == 0) {
        return 24;
      }
      // otherwise, return the actual dimension
      return imageDescriptor.get.apply(this);
    },
  });
});

//document.getElementById("injected-time").innerHTML = navigator.webdriver;

const getParameter=WebGLRenderingContext.getParameter;
WebGLRenderingContext.prototype.getParameter=function(parameter) {
  // UNMASKED_VENDOR_WEBGL WebGLRenderingContext.prototype.VENDOR
  if (parameter === 37445) {
    return "Intel Open Source Technology Center";
  }
  // UNMASKED_RENDERER_WEBGL WebGLRenderingContext.prototype.RENDERER
  if (parameter === 37446) { 
    return "Mesa DRI Intel(R) Ivybridge Mobile";
  }
  return getParameter(parameter);
};