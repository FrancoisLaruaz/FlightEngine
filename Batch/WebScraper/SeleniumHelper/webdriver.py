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
from selenium.common.exceptions import NoSuchElementException
from Helper.constants import *
from Helper.utils import *
from fake_useragent import UserAgent


def getOperaDriver(fullproxy):
	# https://stackoverflow.com/questions/24719270/selenium-webdriver-and-opera-driver
	try:
		path='C:\\webdrivers\\geckodriver.exe'
		fp = webdriver.FirefoxProfile()
		if fullproxy!="":
			fullproxy=fullproxy.split(' ')[0]
			host=fullproxy.split(':')[0]
			port=fullproxy.split(':')[1]
			#https://stackoverflow.com/questions/17082425/running-selenium-webdriver-with-a-proxy-in-python
			firefox_capabilities = DesiredCapabilities.FIREFOX
			firefox_capabilities['marionette'] = True
			webdriver.DesiredCapabilities.FIREFOX['proxy']={
				"httpProxy":fullproxy,
				"ftpProxy":fullproxy,
				"sslProxy":fullproxy,
				"noProxy":None,
				"proxyType":"MANUAL",
				"autodetect":False
			}		
			prox = Proxy()
			prox.proxy_type = ProxyType.MANUAL
			prox.http_proxy = fullproxy
			prox.socks_proxy = fullproxy
			prox.ssl_proxy =fullproxy	
			#prox.add_to_capabilities(firefox_capabilities)
			fp.set_preference("network.proxy.type", 1)
			fp.set_preference("network.proxy.http",host)
			fp.set_preference("network.proxy.http_port",int(port))
			fp.set_preference("network.proxy.https",host)
			fp.set_preference("network.proxy.https_port",int(port))
			fp.set_preference("network.proxy.ssl",host)
			fp.set_preference("network.proxy.ssl_port",int(port))  
			fp.set_preference("network.proxy.ftp",host)
			fp.set_preference("network.proxy.ftp_port",int(port))   
			fp.set_preference("network.proxy.socks",host)
			fp.set_preference("network.proxy.socks_port",int(port))   
			#fp.set_preference("general.useragent.override","Mozilla/5.0 (Macintosh; Intel Mac OS X 10_9_3) AppleWebKit/537.75.14 (KHTML, like Gecko) Version/7.0.3 Safari/7046A194A")
			fp.update_preferences()
		driver = webdriver.Firefox(executable_path=path)


		return driver
	except Exception:
		LogError(traceback,"host = "+host+" and port = "+port)
	return getGoogleChromeDriver(host+":"+port)

def getFirefoxDriver(fullproxy):
	try:
		path='C:\\webdrivers\\geckodriver.exe'
		fp = webdriver.FirefoxProfile()
		if fullproxy!="":
			fullproxy=fullproxy.split(' ')[0]
			host=fullproxy.split(':')[0]
			port=fullproxy.split(':')[1]
			#https://stackoverflow.com/questions/17082425/running-selenium-webdriver-with-a-proxy-in-python
			firefox_capabilities = DesiredCapabilities.FIREFOX
			firefox_capabilities['marionette'] = True
			webdriver.DesiredCapabilities.FIREFOX['proxy']={
				"httpProxy":fullproxy,
				"ftpProxy":fullproxy,
				"sslProxy":fullproxy,
				"noProxy":None,
				"proxyType":"MANUAL",
				"autodetect":False
			}		
			prox = Proxy()
			prox.proxy_type = ProxyType.MANUAL
			prox.http_proxy = fullproxy
			prox.socks_proxy = fullproxy
			prox.ssl_proxy =fullproxy	
			#prox.add_to_capabilities(firefox_capabilities)
			fp.set_preference("network.proxy.type", 1)
			fp.set_preference("network.proxy.http",host)
			fp.set_preference("network.proxy.http_port",int(port))
			fp.set_preference("network.proxy.https",host)
			fp.set_preference("network.proxy.https_port",int(port))
			fp.set_preference("network.proxy.ssl",host)
			fp.set_preference("network.proxy.ssl_port",int(port))  
			fp.set_preference("network.proxy.ftp",host)
			fp.set_preference("network.proxy.ftp_port",int(port))   
			fp.set_preference("network.proxy.socks",host)
			fp.set_preference("network.proxy.socks_port",int(port))   
			#fp.set_preference("general.useragent.override","Mozilla/5.0 (Macintosh; Intel Mac OS X 10_9_3) AppleWebKit/537.75.14 (KHTML, like Gecko) Version/7.0.3 Safari/7046A194A")
			fp.update_preferences()
		driver = webdriver.Firefox(executable_path=path)


		return driver
	except Exception:
		LogError(traceback,"host = "+host+" and port = "+port)
	return getGoogleChromeDriver(host+":"+port)
	
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

		
		browser = webdriver.Chrome(executable_path="C:\\webdrivers\\chromedriver.exe", chrome_options=option,desired_capabilities=capabilities)
		#browser.set_window_position(-10000, 0)

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

	