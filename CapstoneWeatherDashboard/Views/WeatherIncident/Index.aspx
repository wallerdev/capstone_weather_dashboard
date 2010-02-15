<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<WeatherStation.WeatherIncident>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Index
</asp:Content>

<asp:Content ID="extraScripts" ContentPlaceHolderID="ExtraScripts" runat="server">
    $(document).ready(function() { 
        $(".details").colorbox({innerWidth:"50%", html: function() {
            return $(this).prev().html();
        }});
      });
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
                <div style="display: none;">
                    <p>
                        <strong>Location:</strong> <%= Html.Encode(item.Location) %>
                    </p>
                    <p>
                        <strong>Event Type:</strong> <%= Html.Encode(item.EventType) %>
                    </p>
                    <p>
                        <strong>Date:</strong> <%= Html.Encode(item.StartDate.ToString("yyyy-MM-dd")) %>
                    </p>
                </div>
                <a href="#" class="details">Details</a>
            </td>
        </tr>
        <% } %>
    </table>
    <p>
        <%= Html.ActionLink("Create New", "Create") %>
    </p>
</asp:Content>