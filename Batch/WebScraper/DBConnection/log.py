import pypyodbc
from Logger.logger import LogError

connection_string ='Driver={SQL Server Native Client 11.0};Server=.;Database=Template;Trusted_Connection=Yes;'

def ExecuteQuery(query):
	try:
		connection = pypyodbc.connect(connection_string)

		SQL = 'SELECT * FROM dbo.City'

		cur = connection.cursor()
		cur.execute(SQL)

		cur.close()
		connection.close()
	except Exception:
		print(''.join(traceback.format_exc())+" and query = "+query)	
		LogError(traceback,query)	
	return