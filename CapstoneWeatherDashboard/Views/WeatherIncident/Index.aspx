<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<WeatherStation.WeatherIncident>>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
    Weather Incidents
</asp:Content>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Content/colorbox.css" rel="stylesheet" type="text/css" />
    <link href="Content/WeatherIncident.css" rel="stylesheet" type="text/css" />

    <script src="/demo/Scripts/jquery.colorbox-min.js" type="text/javascript"></script>

    <script src="/demo/Scripts/WeatherIncident.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Weather Incidents
    </h2>
    <% foreach (var item in Model)
       { %>
    <div class="result <%= Html.Encode(item.EventType) %>">
        <p><%= Html.Encode(item.Location) %>, <%= Html.Encode(item.EventTypeInWords) %>, <%= Html.Encode(item.StartDate.ToString("yyyy-MM-dd")) %> </p>
    </div>
    <% } %>
    <table id="results">
        <% foreach (var item in Model)
           { %>
        <tr>
            <td>
                <%= Html.Encode(item.Location) %>
            </td>
            <td>
                <%= Html.Encode(item.EventTypeInWords) %>
            </td>
            <td>
                <%= Html.Encode(item.StartDate.ToString("yyyy-MM-dd")) %>
            </td>
            <td>
                <div style="display: none;">
                    <p>
                        <strong>Location:</strong>
                        <%= Html.Encode(item.Location) %>
                    </p>
                    <p>
                        <strong>Event Type:</strong>
                        <%= Html.Encode(item.EventType.ToString()) %>
                    </p>
                    <p>
                        <strong>Date:</strong>
                        <%= Html.Encode(item.StartDate.ToString("yyyy-MM-dd")) %>
                    </p>
                    <p>
                        <strong>More Information:</strong>
                        <%= string.Format("<a href='{0}' target='_blank'>{0}</a>", item.MoreInformationUrl.AbsoluteUri)  %>
                    </p>
                </div>
                <a href="#" class="details">Details</a>
            </td>
        </tr>
        <% } %>
    </table>

    <script src="http://maps.google.com/maps?file=api&amp;v=2.x&amp;key=ABQIAAAAam0nwuIjjXo0_gZGpAyU2hRCy4l6b2RPYQNXTJn1LO8P79-4LxTFJKh9yf0ov08TsXwL824gW69e8w"
        type="text/javascript"></script>

    <script type="text/javascript">
        var geocoder = null;
        geocoder = new GClientGeocoder();
        var address = "grand ledge, mi 48837";
        var marker;
        var lat;
        var lon;

        //        document.write(address);
        geocoder.getLocations(address, Set);

        function Set(response) {
            // Retrieve the object
            var place = response.Placemark[0];
            // Retrieve the latitude and longitude
            var point = new GLatLng(place.Point.coordinates[1],
                          place.Point.coordinates[0]);
            lat = place.Point.coordinates[1];
            lon = place.Point.coordinates[0];
            //            document.write(point);

        }
    </script>

    <script type="text/javascript" src="http://www.google.com/jsapi?key=ABQIAAAAam0nwuIjjXo0_gZGpAyU2hRCy4l6b2RPYQNXTJn1LO8P79-4LxTFJKh9yf0ov08TsXwL824gW69e8w"></script>

    <script type="text/javascript">

        google.load("maps", "2");
        // Call this function when the page has been loaded
        function initialize() {
            var map = new google.maps.Map2(document.getElementById("map"))
            var point3 = new GLatLng(lat, lon);
            var marker = new GMarker(point3);
            map.setCenter(point3, 11);

            map.addOverlay(marker);

            map.addControl(new GLargeMapControl());
        }
        google.setOnLoadCallback(initialize); 
    </script>

    <form id="form1" runat="server">
    <div>
        <div id="map" style="width: 400px; height: 400px">
        </div>
    </div>
    </form>
</asp:Content>
