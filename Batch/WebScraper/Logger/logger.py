import os
import sys
from datetime import datetime, timezone
import traceback
import pypyodbc
from Helper.constants import *

def LogError(traceback,details=""):
	try:
		error=''.join(traceback.format_exc())	
		print ("ERROR CATCHED : "+error+"| details = "+details)
		date=datetime.now().strftime("%Y-%m-%d %H:%M:%S")
		query = """INSERT INTO [dbo].[Log4Net]
           ([Thread]
           ,[Level]
           ,[Logger]
           ,[Message]
           ,[Exception]
           ,[UserLogin]
		   ,[Date])
		VALUES
           (0
           ,'ERROR'
           ,'PYTHON ERROR'
           ,'"""+details.replace("'","''")[:8000]+"','"+error.replace("'","''")[:5000]+"','*** BATCH ***','"+date.replace("'","''")+".000')"
		connection = pypyodbc.connect(connection_string)
		cur = connection.cursor()
		cur.execute(query)
		cur.commit()
		cur.close()
		connection.close()
	except Exception:
		print(''.join(traceback.format_exc())+" and details = "+details)			
	return

