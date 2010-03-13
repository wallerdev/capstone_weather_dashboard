<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="WeatherStation" %>
<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
    Weather Incidents
</asp:Content>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <link href="/demo/Content/colorbox.css" rel="stylesheet" type="text/css" />
    <link href="/demo/Content/WeatherIncident.css" rel="stylesheet" type="text/css" />

    <script src="/demo/Scripts/jquery.colorbox-min.js" type="text/javascript"></script>

    <script src="http://maps.google.com/maps?file=api&amp;v=2.x&amp;key=ABQIAAAAam0nwuIjjXo0_gZGpAyU2hRCy4l6b2RPYQNXTJn1LO8P79-4LxTFJKh9yf0ov08TsXwL824gW69e8w"
        type="text/javascript"></script>

    <script type="text/javascript" src="http://www.google.com/jsapi?key=ABQIAAAAam0nwuIjjXo0_gZGpAyU2hRCy4l6b2RPYQNXTJn1LO8P79-4LxTFJKh9yf0ov08TsXwL824gW69e8w"></script>

    <script src="/demo/Scripts/WeatherIncident.js" type="text/javascript"></script>

    <script type="text/javascript">
        // NCDC Weather Incidents

        urls.push('<%= Url.Action("Index", "NcdcWeatherIncident", new { state = ViewData["state"], county = ViewData["county"], incidentFilter = ViewData["incidentFilter"], startDate = ((DateTime)ViewData["startDate"]).ToShortDateString(), endDate = ((DateTime)ViewData["endDate"]).ToShortDateString() }) %>');

        // Weather Underground Incidents
        <% for(DateTime d = (DateTime)ViewData["startDate"]; d <= (DateTime)ViewData["endDate"]; d = d.AddDays(1))
          {%>
            urls.push('<%= Url.Action("Index", "WeatherUndergroundWeatherIncident", new { date = d.ToShortDateString(), airportCode = ViewData["airportCode"] }) %>');
        <%}%>

        totalUrls = urls.length;

        $(function() {
            if(urls.length > 0) {
                $.getJSON(urls.shift(), displayIncidents);
            }
        });
        
        var latitude = <%= ViewData["latitude"] %>;
        var longitude = <%= ViewData["longitude"] %>;     
        var homeAddress = '<%= ViewData["homeAddress"] %>';
        var searchString = '<%= ViewData["searchStringAsEnglish"] %>';
    </script>

</asp:Content>
<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <div id="progress">
        <p>
            <img src="/demo/Content/images/ajax-loader.gif" alt="Loading..." />
        </p>
        <p id="percent">
        </p>
    </div>
    <div id="results">
    </div>
</asp:Content>
