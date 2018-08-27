from Logger.logger import *
from datetime import datetime, timezone
import time

def getDurationMinute(strDuration):
	result=-1
	try:
		strDuration=strDuration.replace(" ", "").replace("'", "").replace("hours", "h").replace("hour", "h").replace("minutes", "").replace("minute", "").replace("min", "")
		hours=int(strDuration.split('h')[0])
		minutes=int(strDuration.split('h')[1])
		result=hours*60+minutes
	except Exception:
		result=-1
		LogError(traceback,"strDuration = "+strDuration)
	return result

	