from Logger.logger import *
from datetime import datetime, timezone
import time
from datetime import timedelta


def addDay(strDate,dayToAdd="0"):
	result=strDate
	try:
		if dayToAdd=="":
			dayToAdd="0"
		previousdate = datetime(int(strDate.split('/')[2]), int(strDate.split('/')[1]), int(strDate.split('/')[0]))
		newdate=previousdate+ timedelta(days=int(dayToAdd))
		result=newdate.strftime('%d/%m/%Y')
	except Exception:
		LogError(traceback,"strDate = "+strDate+" and dayToAdd = "+str(dayToAdd))
	return result

	