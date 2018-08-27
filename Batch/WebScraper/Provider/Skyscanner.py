import os
import sys
from datetime import datetime, timezone
import traceback
from Logger.logger import *
from DBConnection.SQLConnect import *
from SeleniumHelper.webdriver import *
from selenium import webdriver 
from selenium.webdriver.common.by import By 
from selenium.webdriver.support.ui import WebDriverWait 
from selenium.webdriver.support import expected_conditions as EC 
from selenium.common.exceptions import TimeoutException
from selenium.webdriver.chrome.options import Options

def SearchSkyscanner(proxy,searchTripProviderId,origin,destination,direct,fromDate,toDate):
	result="KO|"
	try:
		conditionalPrint("** Begin Skyscanner **")
		url="https://www.skyscanner.com/transport/vols/yvra/las/180831/180907/?adults=1&children=0&adultsv2=1&childrenv2=&infants=0&cabinclass=economy&rtn=1&preferdirects=true&outboundaltsenabled=false&inboundaltsenabled=false&ref=home#results"
		browser=getGoogleChromeDriver(proxy)
		browser.get(url)
		result=waitForWebdriver(searchTripProviderId,browser,".od-resultpage-highlight-title",".dialog_error")

		if result=="OK":	
			if checkExistsByXpath(browser,"//div[@class='od-ui-dialog dialog dialog_error dialog-undefined od-center-dialogs']") :
				SetTripProviderAsSuccess(searchTripProviderId)
				result="OK"
				conditionalPrint ("** End Skyscanner : no resuld founds **\n")
				browser.quit()
				return result
			
			elements = browser.find_elements_by_xpath("//div[@class='result od-resultpage-wrapper    ']")
			for element in elements:
				count=count+ExtractData(element,url,fromDate,toDate,searchTripProviderId,maxStopNumber)
			elements = browser.find_elements_by_xpath("//div[@class='result od-resultpage-wrapper highlighted odf-box  ']")
			for element in elements:
				count=count+ExtractData(element,url,fromDate,toDate,searchTripProviderId,maxStopNumber)	
			if count>=0:
				result="OK|"+str(count)
			else:
				result="KO|Unexpected error"
		browser.quit()
		print ("** End Skyscanner **\n")
	except Exception:
		result="KO|"+''.join(traceback.format_exc())	
		LogError(traceback,"proxy = "+proxy+" and searchTripProviderId = "+searchTripProviderId+" and origin = "+origin+" and destination = "+destination+" and fromDate = "+fromDate+" and toDate = "+toDate+" and maxStopNumber = "+maxStopNumber)
		browser.quit()
	return result

