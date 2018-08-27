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
from Model.Trip import *
from Helper.providerHelper import *
import re
from DBConnection.trip import *
from Helper.dateHelper import *
from Helper.utils import *

def SearchEdreams(proxy,searchTripProviderId,origin,destination,maxStopNumber,fromDate,toDate):
	result="KO|"
	count=0
	try:
		print ("** Begin Edreams **")

		url="https://www.edreams.com/travel/#results/type=R;dep="+GetDateForUrl(fromDate)+";from="+origin+";to="+destination+";ret="+GetDateForUrl(toDate)+";collectionmethod=false;airlinescodes=false;internalSearch=true"
		if maxStopNumber=="0":
			url=url+";direct=true"
		
		browser=getGoogleChromeDriver(proxy)
		browser.get(url)
		result=waitForWebdriver(searchTripProviderId,browser,".od-resultpage-highlight-title",".dialog_error")
		
		if result=="OK":	
			if checkExistsByXpath(browser,"//div[@class='od-ui-dialog dialog dialog_error dialog-undefined od-center-dialogs']") :
				SetTripProviderAsSuccess(searchTripProviderId)
				result="OK"
				conditionalPrint ("** End Edreams : no resuld founds **\n")
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
		print ("** End Edreams **\n")
	except Exception:
		result="KO|"+''.join(traceback.format_exc())	
		LogError(traceback,"proxy = "+proxy+" and searchTripProviderId = "+searchTripProviderId+" and origin = "+origin+" and destination = "+destination+" and fromDate = "+fromDate+" and toDate = "+toDate+" and maxStopNumber = "+maxStopNumber)
		browser.quit()
	return result

def GetDateForUrl(date):
	result=""
	try:
		result=date[6:10]+"-"+date[3:5]+"-"+date[0:2]
	except Exception:
		LogError(traceback,"date = "+date)
	return	result
	
def ExtractData(element,url,fromDate,toDate,searchTripProviderId,maxStopNumber):
	count=0
	text=""
	time=""
	try:
		intMaxStopNumber=int(maxStopNumber)
		conditionalPrint("*** Begin study element ***")
		text=element.text				
		flightNumber=0
		#itinerary_group
		#data-itinerary-group-id
		if not checkExistsByXpath(element,".//span[@class='odf-icon odf-icon-combination-fl-train-a ficon-2xl']"):	
			OneWayTrips=[]
			ReturnTrips=[]
			ways=element.find_elements_by_xpath(".//div[@class='itinerary_group']");
			for way in ways:
				wayId=way.get_attribute("data-itinerary-group-id");
				conditionalPrint("\n wayId = "+wayId)
				flightNumber=0
				flights=way.find_elements_by_xpath(".//div[@class='odf-row-fluid odf-space-inner-top-s odf-space-inner-bottom-s sp_container']");
				for flight in flights:
					if flight!= None and  flight.text!= None and  flight.text.strip()!='' :
						conditionalPrint("flight = "+flight.text)
						flightNumber=flightNumber+1
						conditionalPrint("Flight number :"+str(flightNumber))
						rightDiv=flight.find_element_by_xpath(".//div[@class='odf-row-fluid odf-text-left odf-text-sm od-secondary-flight-info-time-stops-wrapper']")
						time=flight.find_element_by_xpath(".//div[@class='odf-row odf-h3']").text.replace(" ", "");
						conditionalPrint("time = "+time)
						hours=time.split('-')[0]
						if wayId=="1":
							baseDate=fromDate
						elif wayId=="2":
							baseDate=toDate
						if len(hours.split('+'))==2:
							departureTime=addDay(baseDate,hours.split('+')[1][0:1])+' '+hours.split('+')[0]
						else:
							departureTime=baseDate+' '+hours
						hours=time.split('-')[1]	
						if len(hours.split('+'))==2:						
							arrivalTime=addDay(baseDate,hours.split('+')[1][0:1])+' '+hours.split('+')[0]
						else:
							arrivalTime=baseDate+' '+hours
						conditionalPrint("departureTime = "+departureTime)
						conditionalPrint("arrivalTime = "+arrivalTime)
						duration=getDurationMinute(rightDiv.find_element_by_xpath(".//span[@class='odf-text-nowrap']").text);
						conditionalPrint("duration = "+str(duration))
						if checkExistsByXpath(rightDiv,".//span[@class='odf-text-nowrap number_stop ']"):
							stopNumber=rightDiv.find_element_by_xpath(".//span[@class='odf-text-nowrap number_stop ']").text.split(' ')[0];
							conditionalPrint("stop Number = "+stopNumber)
						else :
							stopNumber="0";
							conditionalPrint("stop Number = "+stopNumber)
						if stopNumber!=None and stopNumber!="" and intMaxStopNumber>=int(stopNumber):
							airline="*Unkwown*";
							airlineSrc=""					
							if checkExistsByXpath(flight,".//img[@class='od-resultpage-segment-itinerary-title-carrier-logo od-primary-description-carrier-icon']"):
								airlineLogo=flight.find_element_by_xpath(".//img[@class='od-resultpage-segment-itinerary-title-carrier-logo od-primary-description-carrier-icon']");
								airlineSrc=airlineLogo.get_attribute("src");
								airline=airlineLogo.get_attribute("alt");
							else :
								if checkExistsByXpath(flight,".//div[@class='odf-grid-col odf-col-span1 odf-tooltip-container odf-text-mono-color-03 hover-active-tooltip od-primary-info-airline ficon-condensed']"):
									airline="*Multiples*";

							conditionalPrint("airline = "+airline)
							conditionalPrint("airlineSrc = "+airlineSrc)
								
							cities=flight.find_element_by_xpath(".//div[@class='odf-row odf-text-sm od-primary-flight-info-cities hover-active-tooltip odf-text-mono-color-03 flight_info_cities odf-text-nowrap']").text;
							conditionalPrint("cities = "+cities)
							fromAirport=cities.split('(')[1].split(')')[0]
							toAirport=cities.split('(')[2].split(')')[0]
							conditionalPrint("fromAirport = "+fromAirport)
							conditionalPrint("toAirport = "+toAirport+"\n")
							trip=Trip(fromAirport,toAirport,duration,departureTime,arrivalTime,airline,airlineSrc,stopNumber)
							if wayId=="1":
								OneWayTrips.append(trip)
							elif wayId=="2":
								ReturnTrips.append(trip)				
			price=element.find_element_by_xpath(".//div[@class='od-price-container  ']").text.replace(" ", "").replace("*", "");
			currency=price[0:1]
			price=price[1:]
			conditionalPrint("currency = "+currency)
			conditionalPrint("price = "+price)
			for OneWayTrip in OneWayTrips :
				if len(ReturnTrips)>0 :
					for ReturnTrip in ReturnTrips :
						count=count+InsertTrip(searchTripProviderId,price,currency,url,OneWayTrip,ReturnTrip)
		else:
			conditionalPrint("Train spotted")

		conditionalPrint("*** End study element ***\n")
	except Exception:
		count=-99999999
		LogError(traceback,"url = "+url+" and element = "+text+" and fromDate = "+fromDate+" and toDate = "+toDate+" and searchTripProviderId = "+searchTripProviderId+" and time = "+time)
	return count
	
	
	

