<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<WeatherStation.WeatherIncident>>" %>

<asp:Content ID="indexTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page
</asp:Content>
<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <% foreach (var incident in Model)
       { %>
    <p>
        <%= incident.Location %>
        (<%= incident.EventType %>)
    </p>
    <% } %>
</asp:Content>
