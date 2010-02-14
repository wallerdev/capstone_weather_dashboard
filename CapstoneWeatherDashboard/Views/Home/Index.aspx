<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="indexTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Auto-Owners Incident Verification System
</asp:Content>
<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <% using (Html.BeginForm("Index", "WeatherIncident", FormMethod.Get))
       { %>
    <h1>Address Search</h1>
    <p>
        <label for="policies">Policy Holder:</label>
        <%= Html.DropDownList("policies", (SelectList)ViewData["Policies"], "") %>
    </p>
    <p>
        <label for="address">
            Address:
        </label>
        <%= Html.TextBox("address") %>
    </p>
    <p>
        <label for="city">
            City:
        </label>
        <%= Html.TextBox("city") %>
        <label for="state">
            State:
        </label>
        <%= Html.TextBox("state") %>
        <label for="zipCode">
            Zip Code:
        </label>
        <%= Html.TextBox("zipCode") %>
    </p>
    <p>
        <label for="latitude">
            Latitude:
        </label>
        <%= Html.TextBox("latitude") %>
        <label for="longitude">
            Longitude:
        </label>
        <%= Html.TextBox("longitude") %>
    </p>
    <p>
        <label for="startDate">
            Start Date:
        </label>
        <%= Html.TextBox("startDate") %>
        <label for="endDate">
            End Date:
        </label>
        <%= Html.TextBox("endDate") %>
    </p>
    <p>
        <input type="submit" />
    </p>
    <% } %>
</asp:Content>
<asp:Content ID="extraScripts" ContentPlaceHolderID="ExtraScripts" runat="server">
    $(document).ready(function() { $("#startDate").datepicker(); $("#endDate").datepicker();
    });
</asp:Content>
