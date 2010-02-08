<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="indexTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page
</asp:Content>
<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <% using (Html.BeginForm("Index", "WeatherIncident", FormMethod.Get))
       { %>
    <p>
        <label for="zipCode">
            Zip Code:
        </label>
        <%= Html.TextBox("zipCode") %>
    </p>
    <p>
        <label for="startDate">
            Start Date:
        </label>
        <%= Html.TextBox("startDate") %>
    </p>
    <p>
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
