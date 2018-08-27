class Trip(object):
	def __init__(self, fromAirportCode,toAirportCode,duration,departureDate,arrivalDate,airlineName,airlineLogo,stops):
		self.fromAirportCode = fromAirportCode
		self.toAirportCode= toAirportCode
		self.duration=duration
		self.departureDate=departureDate
		self.arrivalDate=arrivalDate
		self.airlineName=airlineName
		self.airlineLogo=airlineLogo
		self.stops=stops
