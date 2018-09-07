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
from selenium.webdriver.common.action_chains import ActionChains
import numpy as np
import scipy.interpolate as si

def SearchTest(proxy,searchTripProviderId,origin,destination,maxStopNumber,fromDate,toDate):
	result="KO|"
	try:
		conditionalPrint("** Begin Test **")
		url="https://codepen.io/falldowngoboone/pen/PwzPYv"
		url="https://www.edreams.com/travel/#results/type=R;dep="+GetDateForUrl(fromDate)+";from="+origin+";to="+destination+";ret="+GetDateForUrl(toDate)+";collectionmethod=false;airlinescodes=false;internalSearch=true?hello"
		if maxStopNumber=="0":
			url=url+";direct=true"
		print("url = " +url)
		idElementTocheck='mydiv'
		url="https://www.edreams.com"		
		url="file:///D:/DEV/FlightEngine/Batch/WebScraper/Test/test.html"
		driver=getFirefoxDriver("")
		driver.get(url)
		result=waitForWebdriver(searchTripProviderId,driver,"#"+idElementTocheck,".dialog_error")
		FakeMouseMove(driver,idElementTocheck)	
		conditionalPrint ("** End Test **\n")
		os.system("pause")
	except Exception:
		result="KO|"+''.join(traceback.format_exc())	
		LogError(traceback,"proxy = "+proxy+" and searchTripProviderId = "+searchTripProviderId+" and origin = "+origin+" and destination = "+destination+" and fromDate = "+fromDate+" and toDate = "+toDate+" and maxStopNumber = "+maxStopNumber)
		if driver!=None:
			driver.quit()
	return result
	
def FakeMouseMove(driver,startElementId):
	action =  ActionChains(driver);
	#curve base
	points = [[0, 0], [0, 2], [2, 3], [4, 0], [6, 3], [8, 2], [8, 0]]; #curve base
	points = np.array(points)

	x = points[:,0]
	y = points[:,1]


	t = range(len(points))
	ipl_t = np.linspace(0.0, len(points) - 1, 100)

	x_tup = si.splrep(t, x, k=3)
	y_tup = si.splrep(t, y, k=3)

	x_list = list(x_tup)
	xl = x.tolist()
	x_list[1] = xl + [0.0, 0.0, 0.0, 0.0]

	y_list = list(y_tup)
	yl = y.tolist()
	y_list[1] = yl + [0.0, 0.0, 0.0, 0.0]

	x_i = si.splev(ipl_t, x_list) #x interolate values
	y_i = si.splev(ipl_t, y_list) #y_interpolate values
	startElement = driver.find_element_by_id(startElementId) #drawer
	#First, go to your start point or Element
	action.move_to_element(startElement);
	action.perform();

	for mouse_x, mouse_y in zip(x_i, y_i):
		action.move_by_offset(mouse_x,mouse_y);
		action.perform();	
	return
	
def GetDateForUrl(date):
	result=""
	try:
		result=date[6:10]+"-"+date[3:5]+"-"+date[0:2]
	except Exception:
		LogError(traceback,"date = "+date)
	return	result
