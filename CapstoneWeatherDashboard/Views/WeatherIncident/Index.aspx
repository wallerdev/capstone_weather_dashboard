<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WeatherIncidentModel>" %>
<%@ Import Namespace="CapstoneWeatherDashboard.Models"%>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
    Weather Incidents
</asp:Content>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <link href="/demo/Content/colorbox.css" rel="stylesheet" type="text/css" />
    <link href="/demo/Content/WeatherIncident.css" rel="stylesheet" type="text/css" />

    <script src="/demo/Scripts/jquery.colorbox-min.js" type="text/javascript"></script>
    
    <script src="http://maps.google.com/maps?file=api&amp;v=2.x&amp;key=ABQIAAAAam0nwuIjjXo0_gZGpAyU2hRCy4l6b2RPYQNXTJn1LO8P79-4LxTFJKh9yf0ov08TsXwL824gW69e8w" type="text/javascript"></script>

    <script type="text/javascript" src="http://www.google.com/jsapi?key=ABQIAAAAam0nwuIjjXo0_gZGpAyU2hRCy4l6b2RPYQNXTJn1LO8P79-4LxTFJKh9yf0ov08TsXwL824gW69e8w"></script>
    
    <script src="/demo/Scripts/WeatherIncident.js" type="text/javascript"></script>
    
    <script type="text/javascript">
        var homeAddress = "<%=Model.HomeAddress %>";
    </script>
</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Weather Incidents
    </h2>
    <% foreach (var item in Model.Incidents)
       { %>
    <div class="result <%= Html.Encode(item.EventType) %>">
        <p style="float: right;">
            <strong>2 miles away</strong>
        </p>
        <p style="float: left;">
            <strong>
                <%= Html.Encode(item.StartDate.ToString("yyyy-MM-dd")) %>
            </strong>
        </p>
        <p style="text-align: center">
            <strong>
                <%= Html.Encode(item.EventTypeInWords) %>
            </strong>
        </p>
        <div class="additionalInfo">
            <p>
                <label>
                    Source:
                </label>
                <a href="<%= item.MoreInformationUrl %>">
                    <%= item.MoreInformationUrl %>
                </a>
            </p>
            <div>
                <label>Map:</label>
                <div class="map" title="Eagle, MI" style="height:500px; width:500px"></div>
            </div>
        </div>
    </div>
    <% } %>
</asp:Content>
