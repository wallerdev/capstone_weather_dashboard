<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="indexTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page
</asp:Content>
<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <% using (Html.BeginForm("Index", "WeatherIncident", FormMethod.Get))
       { %>
    <label for="zipCode">
        Zip Code:
    </label>
    <%= Html.TextBox("zipCode") %>
    <input type="submit" />
    <% } %>
</asp:Content>
