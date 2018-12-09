﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Data.Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class TemplateEntities1 : DbContext
    {
        public TemplateEntities1()
            : base("name=TemplateEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Airline> Airlines { get; set; }
        public virtual DbSet<Airport> Airports { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<CategoryType> CategoryTypes { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<EmailAudit> EmailAudits { get; set; }
        public virtual DbSet<EmailTypeLanguage> EmailTypeLanguages { get; set; }
        public virtual DbSet<Flight> Flights { get; set; }
        public virtual DbSet<Log4Net> Log4Net { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductSubType> ProductSubTypes { get; set; }
        public virtual DbSet<ProductType> ProductTypes { get; set; }
        public virtual DbSet<Provider> Providers { get; set; }
        public virtual DbSet<Province> Provinces { get; set; }
        public virtual DbSet<ScheduledTask> ScheduledTasks { get; set; }
        public virtual DbSet<SearchResult> SearchResults { get; set; }
        public virtual DbSet<SearchTrip> SearchTrips { get; set; }
        public virtual DbSet<SearchTripProvider> SearchTripProviders { get; set; }
        public virtual DbSet<SocialMediaConnection> SocialMediaConnections { get; set; }
        public virtual DbSet<SubProvince> SubProvinces { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<TaskLog> TaskLogs { get; set; }
        public virtual DbSet<Trip> Trips { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserFollow> UserFollows { get; set; }
        public virtual DbSet<ValidTopLevelDomain> ValidTopLevelDomains { get; set; }
        public virtual DbSet<Parameter> Parameters { get; set; }
        public virtual DbSet<SearchTripWish> SearchTripWishes { get; set; }
        public virtual DbSet<AirportsTrip> AirportsTrips { get; set; }
        public virtual DbSet<Continent> Continents { get; set; }
        public virtual DbSet<HistoricWeather> HistoricWeathers { get; set; }
    
        public virtual int DeleteNewsById(Nullable<int> newsId)
        {
            var newsIdParameter = newsId.HasValue ?
                new ObjectParameter("NewsId", newsId) :
                new ObjectParameter("NewsId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("DeleteNewsById", newsIdParameter);
        }
    
        public virtual int DeleteProductById_NoTransaction(Nullable<int> productId)
        {
            var productIdParameter = productId.HasValue ?
                new ObjectParameter("ProductId", productId) :
                new ObjectParameter("ProductId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("DeleteProductById_NoTransaction", productIdParameter);
        }
    
        public virtual int DeleteProductById_WithTransaction(Nullable<int> productId)
        {
            var productIdParameter = productId.HasValue ?
                new ObjectParameter("ProductId", productId) :
                new ObjectParameter("ProductId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("DeleteProductById_WithTransaction", productIdParameter);
        }
    
        public virtual int DeleteScheduledTaskById(Nullable<int> scheduledTaskId)
        {
            var scheduledTaskIdParameter = scheduledTaskId.HasValue ?
                new ObjectParameter("ScheduledTaskId", scheduledTaskId) :
                new ObjectParameter("ScheduledTaskId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("DeleteScheduledTaskById", scheduledTaskIdParameter);
        }
    
        public virtual int DeleteUserById(Nullable<int> userId)
        {
            var userIdParameter = userId.HasValue ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("DeleteUserById", userIdParameter);
        }
    
        public virtual int InsertInfo(string error_Message, string exception)
        {
            var error_MessageParameter = error_Message != null ?
                new ObjectParameter("Error_Message", error_Message) :
                new ObjectParameter("Error_Message", typeof(string));
    
            var exceptionParameter = exception != null ?
                new ObjectParameter("Exception", exception) :
                new ObjectParameter("Exception", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("InsertInfo", error_MessageParameter, exceptionParameter);
        }
    
        public virtual int InsertLog(string error_Message, string exception)
        {
            var error_MessageParameter = error_Message != null ?
                new ObjectParameter("Error_Message", error_Message) :
                new ObjectParameter("Error_Message", typeof(string));
    
            var exceptionParameter = exception != null ?
                new ObjectParameter("Exception", exception) :
                new ObjectParameter("Exception", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("InsertLog", error_MessageParameter, exceptionParameter);
        }
    
        public virtual int InsertTripWithTransaction(Nullable<int> searchTripProviderId, Nullable<decimal> price, string currencyCode, string url, string comment, string oneWayTrip_FromAirportCode, string oneWayTrip_ToAirportCode, string oneWayTrip_DepartureDate, string oneWayTrip_ArrivalDate, Nullable<int> oneWayTrip_Duration, Nullable<int> providerId, string oneWayTrip_AirlineName, string oneWayTrip_StopInformation, string oneWayTrip_AirlineLogoSrc, string oneWayTrip_FlightNumber, Nullable<int> oneWayTrip_Stops, string returnTrip_FromAirportCode, string returnTrip_ToAirportCode, string returnTrip_DepartureDate, string returnTrip_ArrivalDate, Nullable<int> returnTrip_Duration, string returnTrip_AirlineName, string returnTrip_AirlineLogoSrc, string returnTrip_StopInformation, string returnTrip_FlightNumber, Nullable<int> returnTrip_Stops)
        {
            var searchTripProviderIdParameter = searchTripProviderId.HasValue ?
                new ObjectParameter("SearchTripProviderId", searchTripProviderId) :
                new ObjectParameter("SearchTripProviderId", typeof(int));
    
            var priceParameter = price.HasValue ?
                new ObjectParameter("Price", price) :
                new ObjectParameter("Price", typeof(decimal));
    
            var currencyCodeParameter = currencyCode != null ?
                new ObjectParameter("CurrencyCode", currencyCode) :
                new ObjectParameter("CurrencyCode", typeof(string));
    
            var urlParameter = url != null ?
                new ObjectParameter("Url", url) :
                new ObjectParameter("Url", typeof(string));
    
            var commentParameter = comment != null ?
                new ObjectParameter("Comment", comment) :
                new ObjectParameter("Comment", typeof(string));
    
            var oneWayTrip_FromAirportCodeParameter = oneWayTrip_FromAirportCode != null ?
                new ObjectParameter("OneWayTrip_FromAirportCode", oneWayTrip_FromAirportCode) :
                new ObjectParameter("OneWayTrip_FromAirportCode", typeof(string));
    
            var oneWayTrip_ToAirportCodeParameter = oneWayTrip_ToAirportCode != null ?
                new ObjectParameter("OneWayTrip_ToAirportCode", oneWayTrip_ToAirportCode) :
                new ObjectParameter("OneWayTrip_ToAirportCode", typeof(string));
    
            var oneWayTrip_DepartureDateParameter = oneWayTrip_DepartureDate != null ?
                new ObjectParameter("OneWayTrip_DepartureDate", oneWayTrip_DepartureDate) :
                new ObjectParameter("OneWayTrip_DepartureDate", typeof(string));
    
            var oneWayTrip_ArrivalDateParameter = oneWayTrip_ArrivalDate != null ?
                new ObjectParameter("OneWayTrip_ArrivalDate", oneWayTrip_ArrivalDate) :
                new ObjectParameter("OneWayTrip_ArrivalDate", typeof(string));
    
            var oneWayTrip_DurationParameter = oneWayTrip_Duration.HasValue ?
                new ObjectParameter("OneWayTrip_Duration", oneWayTrip_Duration) :
                new ObjectParameter("OneWayTrip_Duration", typeof(int));
    
            var providerIdParameter = providerId.HasValue ?
                new ObjectParameter("ProviderId", providerId) :
                new ObjectParameter("ProviderId", typeof(int));
    
            var oneWayTrip_AirlineNameParameter = oneWayTrip_AirlineName != null ?
                new ObjectParameter("OneWayTrip_AirlineName", oneWayTrip_AirlineName) :
                new ObjectParameter("OneWayTrip_AirlineName", typeof(string));
    
            var oneWayTrip_StopInformationParameter = oneWayTrip_StopInformation != null ?
                new ObjectParameter("OneWayTrip_StopInformation", oneWayTrip_StopInformation) :
                new ObjectParameter("OneWayTrip_StopInformation", typeof(string));
    
            var oneWayTrip_AirlineLogoSrcParameter = oneWayTrip_AirlineLogoSrc != null ?
                new ObjectParameter("OneWayTrip_AirlineLogoSrc", oneWayTrip_AirlineLogoSrc) :
                new ObjectParameter("OneWayTrip_AirlineLogoSrc", typeof(string));
    
            var oneWayTrip_FlightNumberParameter = oneWayTrip_FlightNumber != null ?
                new ObjectParameter("OneWayTrip_FlightNumber", oneWayTrip_FlightNumber) :
                new ObjectParameter("OneWayTrip_FlightNumber", typeof(string));
    
            var oneWayTrip_StopsParameter = oneWayTrip_Stops.HasValue ?
                new ObjectParameter("OneWayTrip_Stops", oneWayTrip_Stops) :
                new ObjectParameter("OneWayTrip_Stops", typeof(int));
    
            var returnTrip_FromAirportCodeParameter = returnTrip_FromAirportCode != null ?
                new ObjectParameter("ReturnTrip_FromAirportCode", returnTrip_FromAirportCode) :
                new ObjectParameter("ReturnTrip_FromAirportCode", typeof(string));
    
            var returnTrip_ToAirportCodeParameter = returnTrip_ToAirportCode != null ?
                new ObjectParameter("ReturnTrip_ToAirportCode", returnTrip_ToAirportCode) :
                new ObjectParameter("ReturnTrip_ToAirportCode", typeof(string));
    
            var returnTrip_DepartureDateParameter = returnTrip_DepartureDate != null ?
                new ObjectParameter("ReturnTrip_DepartureDate", returnTrip_DepartureDate) :
                new ObjectParameter("ReturnTrip_DepartureDate", typeof(string));
    
            var returnTrip_ArrivalDateParameter = returnTrip_ArrivalDate != null ?
                new ObjectParameter("ReturnTrip_ArrivalDate", returnTrip_ArrivalDate) :
                new ObjectParameter("ReturnTrip_ArrivalDate", typeof(string));
    
            var returnTrip_DurationParameter = returnTrip_Duration.HasValue ?
                new ObjectParameter("ReturnTrip_Duration", returnTrip_Duration) :
                new ObjectParameter("ReturnTrip_Duration", typeof(int));
    
            var returnTrip_AirlineNameParameter = returnTrip_AirlineName != null ?
                new ObjectParameter("ReturnTrip_AirlineName", returnTrip_AirlineName) :
                new ObjectParameter("ReturnTrip_AirlineName", typeof(string));
    
            var returnTrip_AirlineLogoSrcParameter = returnTrip_AirlineLogoSrc != null ?
                new ObjectParameter("ReturnTrip_AirlineLogoSrc", returnTrip_AirlineLogoSrc) :
                new ObjectParameter("ReturnTrip_AirlineLogoSrc", typeof(string));
    
            var returnTrip_StopInformationParameter = returnTrip_StopInformation != null ?
                new ObjectParameter("ReturnTrip_StopInformation", returnTrip_StopInformation) :
                new ObjectParameter("ReturnTrip_StopInformation", typeof(string));
    
            var returnTrip_FlightNumberParameter = returnTrip_FlightNumber != null ?
                new ObjectParameter("ReturnTrip_FlightNumber", returnTrip_FlightNumber) :
                new ObjectParameter("ReturnTrip_FlightNumber", typeof(string));
    
            var returnTrip_StopsParameter = returnTrip_Stops.HasValue ?
                new ObjectParameter("ReturnTrip_Stops", returnTrip_Stops) :
                new ObjectParameter("ReturnTrip_Stops", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("InsertTripWithTransaction", searchTripProviderIdParameter, priceParameter, currencyCodeParameter, urlParameter, commentParameter, oneWayTrip_FromAirportCodeParameter, oneWayTrip_ToAirportCodeParameter, oneWayTrip_DepartureDateParameter, oneWayTrip_ArrivalDateParameter, oneWayTrip_DurationParameter, providerIdParameter, oneWayTrip_AirlineNameParameter, oneWayTrip_StopInformationParameter, oneWayTrip_AirlineLogoSrcParameter, oneWayTrip_FlightNumberParameter, oneWayTrip_StopsParameter, returnTrip_FromAirportCodeParameter, returnTrip_ToAirportCodeParameter, returnTrip_DepartureDateParameter, returnTrip_ArrivalDateParameter, returnTrip_DurationParameter, returnTrip_AirlineNameParameter, returnTrip_AirlineLogoSrcParameter, returnTrip_StopInformationParameter, returnTrip_FlightNumberParameter, returnTrip_StopsParameter);
        }
    
        public virtual int sp_alterdiagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));
    
            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_alterdiagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }
    
        public virtual int sp_creatediagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));
    
            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_creatediagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }
    
        public virtual int sp_dropdiagram(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_dropdiagram", diagramnameParameter, owner_idParameter);
        }
    
        public virtual ObjectResult<sp_helpdiagramdefinition_Result> sp_helpdiagramdefinition(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagramdefinition_Result>("sp_helpdiagramdefinition", diagramnameParameter, owner_idParameter);
        }
    
        public virtual ObjectResult<sp_helpdiagrams_Result> sp_helpdiagrams(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagrams_Result>("sp_helpdiagrams", diagramnameParameter, owner_idParameter);
        }
    
        public virtual int sp_renamediagram(string diagramname, Nullable<int> owner_id, string new_diagramname)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var new_diagramnameParameter = new_diagramname != null ?
                new ObjectParameter("new_diagramname", new_diagramname) :
                new ObjectParameter("new_diagramname", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_renamediagram", diagramnameParameter, owner_idParameter, new_diagramnameParameter);
        }
    
        public virtual int sp_upgraddiagrams()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_upgraddiagrams");
        }
    
        public virtual int UpdatePricesWithCurrenciesRates()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("UpdatePricesWithCurrenciesRates");
        }
    
        public virtual int SetTripAttractiveness(Nullable<int> tripId)
        {
            var tripIdParameter = tripId.HasValue ?
                new ObjectParameter("TripId", tripId) :
                new ObjectParameter("TripId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SetTripAttractiveness", tripIdParameter);
        }
    }
}
