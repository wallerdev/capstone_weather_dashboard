<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<WeatherStation.WeatherIncident>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Index
</asp:Content>

<asp:Content ID="extraScripts" ContentPlaceHolderID="ExtraScripts" runat="server">
    $(document).ready(function() { $(".details").colorbox({iframe:true, innerWidth:"80%", innerHeight:"80%"}); });
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Weather Incidents
    </h2>
    <table>
        <tr>
            <th>
                Location
            </th>
            <th>
                Event Type
            </th>
            <th>
                Date
            </th>
            <th>
            </th>
        </tr>
        <% foreach (var item in Model)
           { %>
        <tr>
            <td>
                <%= Html.Encode(item.Location) %>
            </td>
            <td>
                <%= Html.Encode(item.EventType) %>
            </td>
            <td>
                <%= Html.Encode(item.StartDate.ToString("yyyy-MM-dd")) %>
            </td>
            <td>
                <a href="http://www.google.com" class="details">Details</a>
            </td>
        </tr>
        <% } %>
    </table>
    <p>
        <%= Html.ActionLink("Create New", "Create") %>
    </p>
</asp:Content>