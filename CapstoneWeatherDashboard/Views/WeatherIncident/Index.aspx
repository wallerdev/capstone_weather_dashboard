<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="WeatherStation" %>
<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
    Weather Incidents
</asp:Content>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <link href="/demo/Content/colorbox.css" rel="stylesheet" type="text/css" />
    <link href="/demo/Content/WeatherIncident.css" rel="stylesheet" type="text/css" />

    <script src="/demo/Scripts/jquery.colorbox-min.js" type="text/javascript"></script>

    <script src="/demo/Scripts/WeatherIncident.js" type="text/javascript"></script>

    <script type="text/javascript">
        var urls = [];
        var allIncidents = [];
        
        function displayIncidents(incidents) {
            for(var i in incidents) {
                allIncidents.push(incidents[i]);
            }
            
            allIncidents = allIncidents.sort(function(a, b) {
                if(a.StartDateString < b.StartDateString) {
                    return -1;
                } else if(a.StartDateString > b.StartDateString) {
                    return 1;
                } else {
                    return 0;
                }
            });
            
            html = '';
            
            for(var i in allIncidents) {
                html += '<div class="result ' + allIncidents[i].EventTypeString + '">' + 
                            '<p class="distance">' + i + ' miles away</p>' +
                            '<p class="date">' + allIncidents[i].StartDateString + '</p>' +
                            '<p class="eventType">' + allIncidents[i].EventTypeInWords + '</p>' +
                            '<div class="additionalInfo">' + 
                                '<p>' + 
                                    '<label>Source:</label>' +
                                    '<a href="' + allIncidents[i].MoreInformationUrl + '">' +
                                        allIncidents[i].MoreInformationUrl +
                                    '</a>' +
                                '</p>' +
                            '</div>' +
                        '</div>'
            }
            
            $("#results").html(html);
        
            if(urls.length > 0) {
                $.getJSON(urls.shift(), displayIncidents);
            }
        }
        
        $(function() {

            // NCDC Weather Incidents

            urls.push('<%= Url.Action("Index", "NcdcWeatherIncident", new { state = ViewData["state"], county = ViewData["county"], startDate = ((DateTime)ViewData["startDate"]).ToShortDateString(), endDate = ((DateTime)ViewData["endDate"]).ToShortDateString() }) %>');

            // Weather Underground Incidents

            

            <% for(DateTime d = (DateTime)ViewData["startDate"]; d <= (DateTime)ViewData["endDate"]; d = d.AddDays(1))
              {%>
                
                urls.push('<%= Url.Action("Index", "WeatherUndergroundWeatherIncident", new { date = d.ToShortDateString(), airportCode = ViewData["airportCode"] }) %>');
            <%}%>

            if(urls.length > 0) {
                $.getJSON(urls.shift(), displayIncidents);
            }
        });
    </script>

</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Weather Incidents
    </h2>
    <div id="results">
    </div>
</asp:Content>
