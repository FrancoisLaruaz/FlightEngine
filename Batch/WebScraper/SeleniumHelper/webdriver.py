from selenium import webdriver
from selenium.webdriver.firefox.firefox_binary import FirefoxBinary
from selenium.webdriver.common.desired_capabilities import DesiredCapabilities
from selenium.webdriver.common.by import By 
from selenium.webdriver.support.ui import WebDriverWait 
from selenium.webdriver.support import expected_conditions as EC 
from selenium.common.exceptions import TimeoutException
from selenium.webdriver.chrome.options import Options
from selenium.webdriver.common.proxy import Proxy, ProxyType
from Logger.logger import *
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.common.by import By
from datetime import datetime, timezone
import time
import json
from selenium.common.exceptions import NoSuchElementException
from Helper.constants import *
from Helper.utils import *
from fake_useragent import UserAgent
from selenium.webdriver.firefox.options import Options
from selenium.common.exceptions import NoSuchElementException
from selenium.webdriver.firefox.firefox_profile import AddonFormatError
from selenium.webdriver.common.alert import Alert
from selenium import webdriver
from selenium.webdriver.remote.webdriver import WebDriver
from random import randint
from time import sleep


def getWebDriver(fullproxy):
	# distil network : https://www.blackhatworld.com/seo/python-scraping-distil-protected-sites.988967/
	return getFirefoxDriver(fullproxy)
	#return getGoogleChromeDriver(fullproxy)

def getEdgeDriver(fullproxy):
	try:
		ExecutablePath='C:\\webdrivers\\MicrosoftWebDriver.exe'
		BinaryPath='C:\\webdrivers\\MicrosoftWebDriver.exe'

		#create capabilities
		capabilities = DesiredCapabilities.EDGE
		driver = webdriver.Edge(executable_path=ExecutablePath, capabilities=capabilities)
		return driver
	except Exception:
		LogError(traceback,"")
	return None	
	
def getPhantomJSDriver(fullproxy):
	try:
		ExecutablePath='C:\\webdrivers\\phantomjs.exe'
		BinaryPath='C:\\webdrivers\\phantomjs.exe'
		driver = webdriver.PhantomJS(executable_path=ExecutablePath)
		return driver
	except Exception:
		LogError(traceback,"fullproxy = "+fullproxy)
	return None		

def getIEDriver(fullproxy):
	try:
		ExecutablePath='C:\\webdrivers\\IEDriverServer.exe'
		BinaryPath='C:\\webdrivers\\IEDriverServer.exe'

		#create capabilities
		capabilities = DesiredCapabilities.INTERNETEXPLORER
		#capabilities.pop("ignoreZoomSetting", True);
		#delete platform and version keys
		capabilities.pop("platform", "WIN8")
		capabilities.pop("version", "11")
		capabilities.pop("browserName", "internet explorer")
		capabilities.pop("ie.usePerProcessProxy", True)
		capabilities.pop("usePerProcessProxy", True)
		capabilities.pop("INTRODUCE_FLAKINESS_BY_IGNORING_SECURITY_DOMAINS", True)
		capabilities.pop("InternetExplorerDriver.INTRODUCE_FLAKINESS_BY_IGNORING_SECURITY_DOMAINS", True)
		#capabilities.pop("nativeEvents", False)
		#capabilities.pop("ignoreProtectedModeSettings", True)
		capabilities.pop("ie.setProxyByServer", True)
		capabilities.pop("setProxyByServer", True)
		#capabilities.setCapability(InternetExplorerDriver.IE_USE_PRE_PROCESS_PROXY,true);
		if fullproxy!="":
			proxy=fullproxy.split(' ')[0]
			conditionalPrint("proxy used : "+proxy)
			prox = Proxy()
			prox.proxy_type = ProxyType.MANUAL
			prox.http_proxy = proxy
			prox.socks_proxy = proxy
			prox.ssl_proxy =proxy
			prox.noProxy =None
			prox.autodetect =None
			#prox["class"] ="org.openqa.selenium.Proxy"
			prox.add_to_capabilities(capabilities)
		driver = webdriver.Ie(executable_path=ExecutablePath, capabilities=capabilities)


		return driver
	except Exception:
		LogError(traceback,"fullproxy = "+fullproxy)
	return None

def getOperaDriver(fullproxy):
	# https://stackoverflow.com/questions/24719270/selenium-webdriver-and-opera-driver
	try:
		ExecutablePath='C:\\webdrivers\\operadriver.exe'
		BinaryPath='C:\\webdrivers\\operadriver.exe'
		if fullproxy!="":
			fullproxy=fullproxy.split(' ')[0]
			host=fullproxy.split(':')[0]
			port=fullproxy.split(':')[1]
		options = webdriver.ChromeOptions()
		options.add_argument("start-maximized"); 
		options.add_argument("disable-infobars"); 
		options.add_argument("--disable-extensions");
		options.add_argument("--disable-gpu"); 
		options.add_argument("--disable-dev-shm-usage"); 
		options.add_argument("--no-sandbox"); 
		options.binary_location = BinaryPath

		driver = webdriver.Opera(options=options,executable_path=ExecutablePath)


		return driver
	except Exception:
		LogError(traceback,"fullproxy = "+fullproxy)
	return None

# Patch in support for WebExtensions in Firefox.
# See: https://intoli.com/blog/firefox-extensions-with-selenium/
# https://stackoverflow.com/questions/51439377/selenium-webdriver-firefox-headless-inject-javascript-to-modify-browser-propert
class FirefoxProfileWithWebExtensionSupport(webdriver.FirefoxProfile):
    def _addon_details(self, addon_path):
        try:
            return super()._addon_details(addon_path)
        except AddonFormatError:
            try:
                with open(os.path.join(addon_path, "manifest.json"), "r") as f:
                    manifest = json.load(f)
                    return {
                        "id": manifest["applications"]["gecko"]["id"],
                        "version": manifest["version"],
                        "name": manifest["name"],
                        "unpack": False,
                    }
            except Exception:
                LogError(traceback,"")
	
def getFirefoxDriver(fullproxy):
	try:
		path='C:\\webdrivers\\geckodriver.exe'
		capabilities = DesiredCapabilities.FIREFOX
		capabilities['marionette'] = True
		#capabilities['CapabilityType.UNEXPECTED_ALERT_BEHAVIOUR'] = org.openqa.selenium.UnexpectedAlertBehaviour.ACCEPT
		opts = Options()
		#opts.set_headless()
		#assert opts.headless
		
		profile = webdriver.FirefoxProfile()
		profile.set_preference("general.useragent.override", UserAgent().random)
		profile.set_preference("browser.cache.disk.enable", False)
		profile.set_preference("browser.cache.memory.enable", False)
		profile.set_preference("browser.cache.offline.enable", False)
		profile.set_preference("network.http.use-cache", False)
		profile.update_preferences()
		
		if fullproxy!="":
			proxy=fullproxy.split(' ')[0]
			host=proxy.split(':')[0]
			port=proxy.split(':')[1]
			conditionalPrint("proxy used : "+proxy)
			prox = Proxy()
			socksVersion=5
			proxy=fullproxy.split(' ')[0]
			prox.proxy_type = ProxyType.MANUAL
			prox.http_proxy = proxy
			prox.ftp_proxy = proxy
			prox.ssl_proxy =proxy
			prox.add_to_capabilities(capabilities)
		driver = webdriver.Firefox(executable_path=path, capabilities=capabilities, firefox_options = opts)


		return driver
	except Exception:
		LogError(traceback,"fullproxy = "+fullproxy)
	return None
	
def waitForWebdriver(searchTripProviderId,browser,css_selectorOK,css_selectorKO=""):
	result="KO|"
	try:
		if css_selectorKO!="":
			css_selector=css_selectorOK+", "+css_selectorKO
		else:
			css_selector=css_selectorOK
		conditionalPrint("begin wait for "+css_selector+" : "+datetime.now().strftime('%Y-%m-%d %H:%M:%S.%f')[:-3])
		wait = WebDriverWait(browser, timeout)		
		wait.until(EC.presence_of_element_located((By.CSS_SELECTOR, css_selector)))
		result="OK"
		conditionalPrint("end wait : "+datetime.now().strftime('%Y-%m-%d %H:%M:%S.%f')[:-3])	
	except Exception:
		result="KO|waitForWebdriver"
		LogError(traceback,"searchTripProviderId ="+searchTripProviderId+" and css_selectorOK = "+css_selectorOK+" and css_selectorKO = "+css_selectorKO)
	return result			

def send(driver, cmd, params={}):
  resource = "/session/%s/chromium/send_command_and_get_result" % driver.session_id
  url = driver.command_executor._url + resource
  body = json.dumps({'cmd': cmd, 'params': params})
  response = driver.command_executor._request('POST', url, body)
  if response['status']:
    raise Exception(response.get('value'))
  return response.get('value')

def add_script(driver, script):
  send(driver, "Page.addScriptToEvaluateOnNewDocument", {"source": script})
	
def getGoogleChromeDriver(fullproxy):
	try:
		capabilities = webdriver.DesiredCapabilities.CHROME
		if fullproxy!="":
			proxy=fullproxy.split(' ')[0]
			conditionalPrint("proxy used : "+proxy)
			prox = Proxy()
			prox.proxy_type = ProxyType.MANUAL
			prox.http_proxy = proxy
			prox.socks_proxy = proxy
			prox.ssl_proxy =proxy
			prox.add_to_capabilities(capabilities)
		
		WINDOW_SIZE = "1920,1080"
		option = webdriver.ChromeOptions()
		#option.add_argument("--flag-switches-begin")
		#option.add_argument("--incognito")
		#option.add_argument("start-maximized")
		option.add_argument("disable-infobars")
		#option.add_argument("--disable-extensions")
		#option.add_argument(r'user-data-dir=C:\\Users\\franc\\AppData\\Local\\Google\\Chrome\\User Data\\Profile 1')
		#option.add_argument('--profile-directory=Profile 1')
		ua = UserAgent()
		userAgent = ua.random
		#option.add_argument(f'user-agent={userAgent}')
		#option.add_argument("--disable-gpu")
		#option.add_argument("--disable-infobars")
		#option.add_argument("--disable-notifications")
		#option.add_argument("--disable-extensions")
		if hideBrowser=="YES":
			option.add_argument("--headless")  
			option.add_argument("--window-size=%s" % WINDOW_SIZE)		

		WebDriver.add_script = add_script
		browser = webdriver.Chrome(executable_path="C:\\webdrivers\\chromedriver.exe", chrome_options=option,desired_capabilities=capabilities)
		#browser.set_window_position(-10000, 0)
		browser.add_script("""
		  if (window.self === window.top) { // if main document
			console.log('add script before START');


	
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
				console.log('add script before END');
		  }
		  """)
		return browser
	except Exception:
		LogError(traceback,"fullproxy = "+fullproxy)
	return None	
	
def checkExistsByXpath(webElement,xpath):
	try:
		webElement.find_element_by_xpath(xpath)
	except NoSuchElementException:
		return False
	return True	

	