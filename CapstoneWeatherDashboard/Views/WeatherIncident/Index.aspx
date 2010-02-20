<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<WeatherStation.WeatherIncident>>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
    Weather Incidents
</asp:Content>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <link href="/demo/Content/colorbox.css" rel="stylesheet" type="text/css" />
    <link href="/demo/Content/WeatherIncident.css" rel="stylesheet" type="text/css" />

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
        </div>
    </div>
    <% } %>
</asp:Content>
