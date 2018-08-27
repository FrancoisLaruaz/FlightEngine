from Logger.logger import *
from datetime import datetime, timezone
import time
from datetime import timedelta


def conditionalPrint(text):
	try:
		if printValues=="YES":
			print(text)
	except Exception:
		LogError(traceback,"text = "+text)
	return 

	