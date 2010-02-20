<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="WeatherStation"%>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
    Weather Incidents
</asp:Content>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <link href="/demo/Content/colorbox.css" rel="stylesheet" type="text/css" />
    <link href="/demo/Content/WeatherIncident.css" rel="stylesheet" type="text/css" />

    <script src="/demo/Scripts/jquery.colorbox-min.js" type="text/javascript"></script>

    <script src="/demo/Scripts/WeatherIncident.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function() {
            var urls = [];

            // NCDC Weather Incidents

            urls.push('<%= Url.Action("Index", "NcdcWeatherIncident", new { state = ViewData["state"], county = ViewData["county"], startDate = ((DateTime)ViewData["startDate"]).ToShortDateString(), endDate = ((DateTime)ViewData["endDate"]).ToShortDateString() }) %>');

            // Weather Underground Incidents

            

            <% for(DateTime d = (DateTime)ViewData["startDate"]; d <= (DateTime)ViewData["endDate"]; d = d.AddDays(1))
              {%>
                
                urls.push('<%= Url.Action("Index", "WeatherUndergroundWeatherIncident", new { date = d.ToShortDateString(), airportCode = ViewData["airportCode"] }) %>');
            <%}%>

            for(var i in urls) {
                // $.get(urls[i])
            }
            $.get('');
        });
    </script>

</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Weather Incidents
    </h2>
</asp:Content>
